using DashboardApi.Entities;
using DashboardApi.Entities.Data;
using System;

namespace DashboardApi.Dtos
{
	public class DashboardInvitationDto
	{
		public string Email { get; set; }
		public string Status { get; set; }
		public DateTime TimeStamp { get; set; }

		public DashboardInvitationDto() { }
		public DashboardInvitationDto(DashboardInvitation dashboardInvitation)
		{
			Email = dashboardInvitation.Email;
			Status = dashboardInvitation.Status.ToInvitationStatusString();
			TimeStamp = dashboardInvitation.TimeStamp;
		}


	}

}
