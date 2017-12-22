using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RM.Services.Repository;
using RM.Services.Interfaces;
using RM.Data;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;

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

      app.UseMvc();
    }
  }
}