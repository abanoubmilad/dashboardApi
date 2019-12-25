using System;

namespace DashboardApi.Entities.Data
{
	public class DashboardInvitation
	{
		public string Email { get; set; }
		public InvitationStatus Status { get; set; }
		public DateTime TimeStamp { get; set; }
		public byte[] ConfirmationHash { get; set; }
		public byte[] ConfirmationSalt { get; set; }
		public DashboardInvitation() { }
	}

}
