using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using RM.Data;
using RM.Data.Repository;
using RM.Data.Repository.Contract;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace RM.Api
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      string connectionString = Configuration.GetConnectionString("RMConnection");
      services.AddDbContext<RMContext>(option => option.UseSqlServer(connectionString));

      services.AddTransient<IUserRepository, UserRepository>();
      services.AddTransient<IProjectStatusRepository, ProjectStatusRepository>();
      services.AddTransient<IProjectRepository, ProjectRepository>();
      services.AddTransient<IRiskStatusRepository, RiskStatusRepository>();
      services.AddTransient<IRiskRepository, RiskRepository>();

      services.AddCors(options =>
      {
        options.AddPolicy("CorsPolicy",
                  builder => builder.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials());
      });

      //services.AddMvcCore()
      //  .AddApiExplorer()
      //  .AddAuthorization();

      services.AddMvc()
        .AddJsonOptions(options =>
        {
          options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
          options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        });

      services.AddAuthentication("Bearer")
        .AddIdentityServerAuthentication(options =>
        {
          options.Authority = Configuration.GetValue<string>("IdentityServer");
          options.RequireHttpsMetadata = false;

          options.ApiName = "RiskManagement";
        });

      services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" }));
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
      {
        var context = serviceScope.ServiceProvider.GetRequiredService<RMContext>();
        bool databaseExist = false;

        try { context.Database.EnsureCreated(); }
        catch { databaseExist = true; }

        if (databaseExist)
        {
          try
          {
            var databaseCreator = (RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>();
            databaseCreator.CreateTables();
          }
          catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
      }

      app.UseSwagger();
      app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

      app.UseExceptionHandler(configure =>
      {
        configure.Run(async context =>
        {
          var ex = context.Features
          .Get<IExceptionHandlerFeature>()
          .Error;

          context.Response.StatusCode = 500;
          await context.Response.WriteAsync($"{ex.Message}").ConfigureAwait(false);
        });
      });

      app.UseCors("CorsPolicy");
      app.UseAuthentication();
      app.UseMvc();

      app.Run(async (context) =>
      {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        await context.Response.WriteAsync($"No endpoint found for request {context.Request.Path}").ConfigureAwait(false);
      });
    }
  }
}