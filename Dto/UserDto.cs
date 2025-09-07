using System.ComponentModel.DataAnnotations;

namespace Tap2Test_Api.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = string.Empty;


        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;


        [Required, Phone]
        public string Phone { get; set; } = string.Empty;


        [Range(0, 120)]
        public int Age { get; set; }
    }
}
