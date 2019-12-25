using System;

namespace DashboardApi.Entities
{
	public enum InvitationStatus
	{
		Pending = 0,
		Accepted = 1
	}

	public static class InvitationStatusUtil
	{
		public static string ToInvitationStatusString(this InvitationStatus type)
		{
			return Enum.GetName(typeof(InvitationStatus), type);
		}
	}
}