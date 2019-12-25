using System.ComponentModel.DataAnnotations;

namespace DashboardApi.ServiceModels
{
	public class LoginUser
	{
		[Required(ErrorMessage = "The email address is required")]
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
	}
}