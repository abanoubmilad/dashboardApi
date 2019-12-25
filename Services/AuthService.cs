using DashboardApi.Dtos;
using DashboardApi.Entities;
using DashboardApi.Entities.Data;
using DashboardApi.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace DashboardApi.Services
{
	public interface IAuthService
	{
		void CheckDashboardWriteAccessToUser(int dashboardUserId, int requestedUserId);
		void CheckDashboardReadAccessToUser(int dashboardUserId, int requestedUserId);
		void CheckDashbaordInvitation(string email, string confirmationCode);
	}

	public class AuthService : IAuthService
	{
		private DataContext _context;

		public AuthService(DataContext context)
		{
			_context = context;

		}


		public void CheckDashboardReadAccessToUser(int dashboardUserId, int requestedUserId)
		{
			if (dashboardUserId != requestedUserId && dashboardUserId != DashboardConfig.DashboardUserAdminId)
				throw new AppException("Not authorized to view this user content");
		}


		public void CheckDashboardWriteAccessToUser(int dashboardUserId, int requestedUserId)
		{
			if (dashboardUserId != requestedUserId && dashboardUserId != DashboardConfig.DashboardUserAdminId)
				throw new AppException("Not authorized to edit this user content");
		}


		public void CheckDashbaordInvitation(string email, string confirmationCode)
		{
			var invitation = _context.DashboardInvitations.FirstOrDefault(x => x.Email == email);

			if (invitation == null)
				throw new AppException("You need an invitation to complete this step");
			if (!AuthUtility.VerifyPasswordHash(confirmationCode, invitation.ConfirmationHash, invitation.ConfirmationSalt))
				throw new AppException("Your confirmation code is invalid");
		}

	}
}