using DashboardApi.Dtos;
using DashboardApi.Entities;
using DashboardApi.Entities.Data;
using DashboardApi.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DashboardApi.Services
{
	public interface IDashboardService
	{
		bool SendDashboardInvitaitonEmail(string email);
		IList<DashboardInvitationDto> GetDashboardInvitaitonEmails();

	}

	public class DashboardService : IDashboardService
	{
		private readonly DataContext _context;
		private readonly IEmailService _emailService;
		private readonly AppSettings _appSettings;
		private static readonly object _syncRoot = new object();


		public DashboardService(DataContext context, IEmailService emailService,
			IOptions<AppSettings> appSettings)
		{
			_context = context;
			_emailService = emailService;
			_appSettings = appSettings.Value;

		}


		public IList<DashboardInvitationDto> GetDashboardInvitaitonEmails()
		{
			return _context.DashboardInvitations
				.Select(x => new DashboardInvitationDto(x)).ToList();
		}

		public bool SendDashboardInvitaitonEmail(string email)
		{
			if (_context.DashboardUsers.Where(x => x.Email == email).FirstOrDefault() != null)
				throw new AppException("Email is already assocaited with a dashboard user");

			string confirmationCode = AuthUtility.CreateEmailConfirmationCode();
			AuthUtility.CreatePasswordHash(confirmationCode, out byte[] hash, out byte[] salt);

			var invited = _context.DashboardInvitations.Where(x => x.Email == email).FirstOrDefault();
			if (invited != null)
			{
				invited.TimeStamp = System.DateTime.UtcNow;
				invited.ConfirmationHash = hash;
				invited.ConfirmationSalt = salt;
			}
			else
			{
				_context.DashboardInvitations.Add(new DashboardInvitation
				{
					Email = email,
					Status = InvitationStatus.Pending,
					TimeStamp = System.DateTime.UtcNow,
					ConfirmationHash = hash,
					ConfirmationSalt = salt
				});

			}

			_context.SaveChanges();

			var url = $"{_appSettings.FrontendHost}/{_appSettings.FrontendRegistrationUrl}?{_appSettings.RegistrationQueryEmail}={email}&{_appSettings.RegistrationQueryConfirmationCode}={confirmationCode}";

			var body = "You have been invited to Dashbaord<br>" +
				 $"Dashboard: {_appSettings.FrontendHost}<br>" +
				 $"Email: {email}<br>" +
				 $"Confirmation Code: {confirmationCode}<br>" +
				"Please click the following link to complete your registration process<br>" +
				$"<a href='{url}'>{url}</a>";

			return _emailService.SendEmail(email, DashboardConfig.DashboardInvitationEmailSubject, body);
		}
	}

}
