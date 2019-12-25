using System.ComponentModel.DataAnnotations;

namespace DashboardApi.ServiceModels
{
	public class InviteDashbaord
	{
		[Required(ErrorMessage = "The email address is required")]
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string Email { get; set; }
	}

}