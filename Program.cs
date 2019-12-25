using DashboardApi.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace DashboardApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			BuildWebHost(args, AppSettings.GetAppSettings()).Run();
		}

		public static IWebHost BuildWebHost(string[] args, AppSettings appSettings) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.UseUrls(appSettings.Host + ":" + appSettings.Port)
				.Build();
	}
}
