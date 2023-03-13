using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.User
{
	public class UserDTO : LoginUserDTO
    {
        // Email also serves as user name:

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Role { get; set; }
    }
}