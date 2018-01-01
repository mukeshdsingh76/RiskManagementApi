using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RM.Auth.Services;
using RM.Data;
using RM.Data.Models;
using RM.Services.Interfaces;
using RM.Services.Repository;
using System;

namespace RM.Auth
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
      services.AddDbContext<RMIdentityContext>(option => option.UseSqlServer(connectionString));

      services.AddIdentity<User, IdentityRole>
        (config =>
        {
          config.SignIn.RequireConfirmedEmail = true;
          config.Password.RequireDigit = false;
          config.Password.RequireLowercase = false;
          config.Password.RequiredLength = 6;
          config.Password.RequireNonAlphanumeric = false;
          config.Password.RequireUppercase = false;
          config.Lockout.MaxFailedAccessAttempts = 10;
          config.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<RMIdentityContext>()
        .AddDefaultTokenProviders();

      services.AddTransient<IUserRepository, UserRepository>();
      services.AddTransient<IEmailSender, EmailSender>();
      services.AddMvc();

      services.AddIdentityServer()
        .AddDeveloperSigningCredential()
        .AddInMemoryPersistedGrants()
        .AddInMemoryIdentityResources(Config.GetIdentityResources())
        .AddInMemoryApiResources(Config.GetApiResources())
        .AddInMemoryClients(Config.GetClients())
        .AddAspNetIdentity<User>();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseBrowserLink();
        app.UseDatabaseErrorPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }

      app.UseStaticFiles();

      app.UseAuthentication();
      app.UseIdentityServer();

      using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
      {
        var context = serviceScope.ServiceProvider.GetRequiredService<RMIdentityContext>();
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

      app.UseMvc(routes =>
            {
              routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");
            });
    }
  }
}
