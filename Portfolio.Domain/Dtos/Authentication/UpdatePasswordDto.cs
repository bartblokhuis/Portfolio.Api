using System.ComponentModel.DataAnnotations;

namespace Portfolio.Domain.Dtos.Authentication
{
    public class UpdatePasswordDto
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string OldPassword { get; set; }
    }
}
