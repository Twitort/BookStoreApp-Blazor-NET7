using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.User
{
	public class LoginUserDTO
	{
		// Email also serves as user name:
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }

	}
}