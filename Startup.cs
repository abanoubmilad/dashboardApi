using AutoMapper;
using DashboardApi.Helpers;
using DashboardApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DashboardApi
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors();
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
			services.AddAutoMapper();

			var appSettingsSection = Configuration.GetSection("AppSettings");
			services.Configure<AppSettings>(appSettingsSection);

			var appSettings = appSettingsSection.Get<AppSettings>();

			// jwt authentication
			AuthUtility.ConfigureJwtAuth(appSettings, services);

			// configure DI
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IEmailService, EmailService>();
			services.AddScoped<IDashboardService, DashboardService>();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			// DB
			services.AddDbContext<DataContext>();
			using (var context = services.BuildServiceProvider().GetRequiredService<DataContext>())
			{
				context.Database.Migrate();

				// data seeding
				var admin = context.DashboardUsers.FirstOrDefault(x => x.Email == appSettings.DashboardAdminEmail);
				if (admin == null)
				{
					context.DashboardUsers.Add(DashboardConfig.CreateAdminDashboardUser());
					context.SaveChanges();
				}
			}

			if (appSettings.EnableDocs)
			{
				// Docs
				services.AddSwaggerGen(c =>
				{
					c.SwaggerDoc(appSettings.ApiVersion, new Info
					{
						Title = appSettings.ApiName,
						Version = appSettings.ApiVersion
					});
					var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.XML");
					c.IncludeXmlComments(xmlPath);
				});
			}
		}

		// configure HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			// global cors policy
			app.UseCors(x => x
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader());

			app.UseAuthentication();
			app.UseMvc();

			var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();

			if (appSettings.EnableDocs)
			{
				// Docs
				app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", appSettings.ApiName);
					c.RoutePrefix = appSettings.DocumentationBaseUrl;
				});
			}
		}
	}
}
