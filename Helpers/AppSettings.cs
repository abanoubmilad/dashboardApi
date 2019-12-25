using Microsoft.Extensions.Configuration;
using System.IO;

namespace DashboardApi.Helpers
{
	public class AppSettings
	{
		public static AppSettings GetAppSettings()
		{
			var config = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: false)
			.Build();
			return config.GetSection("AppSettings").Get<AppSettings>();
		}
		public string Secret { get; set; }
		public string Host { get; set; }
		public string Port { get; set; }
		public int TokenExpiryInDays { get; set; }
		public string DashBoardDBConnectionString { get; set; }
		public string ApiVersion { get; set; }
		public string ApiName { get; set; }
		public string DocumentationBaseUrl { get; set; }
		public string DBServerConnectionString { get; set; }
		public bool EnableDocs { get; set; }

		// Email service
		public string SMTPServer { get; set; }
		public int SMTPPort { get; set; }
		public bool EnableSSL { get; set; }
		public string CredentialsEmail { get; set; }
		public string CredentialsPassword { get; set; }

		// Admin
		public string DashboardAdminEmail { get; set; }
		public string DashboardAdminPassword { get; set; }

		// Dashboard Invitation
		public string FrontendRegistrationUrl { get; set; }
		public string RegistrationQueryEmail { get; set; }
		public string RegistrationQueryConfirmationCode { get; set; }
		public string FrontendHost { get; set; }
	}
}