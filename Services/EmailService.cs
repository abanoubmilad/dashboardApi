using DashboardApi.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;

namespace DashboardApi.Services
{
	public interface IEmailService
	{
		bool SendEmail(string receiverEmail, string subject, string body);
		bool SendEmail(string receiverEmail, string subject, string body,
		   string host, int port,
		   bool enableSsl,
		   string senderEmail,
		   string senderEmailPassword);
	}

	public class EmailService : IEmailService
	{
		private DataContext _context;
		private readonly AppSettings _appSettings;

		public EmailService(DataContext context, IOptions<AppSettings> appSettings)
		{
			_context = context;
			_appSettings = appSettings.Value;

		}
		public bool SendEmail(string receiverEmail, string subject, string body)
		{
			return SendEmail(receiverEmail, subject, body,
		   _appSettings.SMTPServer, _appSettings.SMTPPort,
		   _appSettings.EnableSSL,
		   _appSettings.CredentialsEmail,
		   _appSettings.CredentialsPassword);
		}

		public bool SendEmail(string receiverEmail, string subject, string body,
			string host, int port,
			bool enableSsl,
			string senderEmail,
			string senderEmailPassword)
		{
			var smtpClient = new SmtpClient
			{
				Host = host,
				Port = port,
				EnableSsl = enableSsl,
				UseDefaultCredentials = true,
				Timeout = 3000,
				Credentials = new NetworkCredential(senderEmail, senderEmailPassword)
			};

			using (var message = new MailMessage(senderEmail, receiverEmail)
			{
				Subject = subject,
				Body = body,
				IsBodyHtml = true,
			}) try
				{
					var task = smtpClient.SendMailAsync(message);
					task.Wait();
					if (task.IsCompletedSuccessfully)
						return true;
					return false;

				}
				catch (Exception e)
				{
					throw new AppException(e.Message);
				}


		}
	}
}
