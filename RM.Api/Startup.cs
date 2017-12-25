using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RM.Services.Repository;
using RM.Services.Interfaces;
using RM.Data;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using RM.Data.Models;
using Microsoft.AspNetCore.Identity;
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

      services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<RMContext>()
        .AddDefaultTokenProviders();

      services.Configure<IdentityOptions>(options =>
      {
        // Password settings
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = false;
        options.Password.RequiredUniqueChars = 6;

        // Lockout settings
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
        options.Lockout.MaxFailedAccessAttempts = 10;
        options.Lockout.AllowedForNewUsers = true;

        // User settings
        options.User.RequireUniqueEmail = true;
      });

      services.AddTransient<IProjectStatusRepository, ProjectStatusRepository>();
      services.AddTransient<IUserRepository, UserRepository>();
      services.AddTransient<IProjectRepository, ProjectRepository>();
      services.AddTransient<IRiskRepository, RiskRepository>();

      services.AddMvc();

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
      });
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseAuthentication();

      using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
      {
        var context = serviceScope.ServiceProvider.GetRequiredService<RMContext>();
        context.Database.EnsureCreated();

        //context.Database.Migrate();
        //var databaseCreator = (RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>();
        //databaseCreator.CreateTables();
      }

      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
      });

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

      app.UseMvc();
    }
  }
}