using DashboardApi.Entities;

namespace DashboardApi.Helpers
{
	public class DashboardConfig
	{

		public const string AllowedRandomChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz1234567890";
		public const int EmailConfirmarionCodeLength = 16;
		public const int DashboardUserAdminId = 1;
		public const string DashboardInvitationEmailSubject = "Dashboard Invitation";

		public static DashboardUser CreateAdminDashboardUser()
		{
			AuthUtility.CreatePasswordHash(AppSettings.GetAppSettings().DashboardAdminPassword, out byte[] hash, out byte[] salt);
			return new DashboardUser
			{
				Id = DashboardUserAdminId,
				FullName = "Admin",
				Email = AppSettings.GetAppSettings().DashboardAdminEmail,
				Role = Role.Admin,
				PasswordHash = hash,
				PasswordSalt = salt
			};
		}

	}
}
