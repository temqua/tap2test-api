using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tap2Test_Api.Dto;

namespace Tap2Test_Api.Data.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required, Phone, MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Range(0, 120)]
        public int Age { get; set; }

        public UserDto ToDto() => new()
        {
            Id = Id,
            Name = Name,
            Email = Email,
            Phone = Phone,
            Age = Age
        };
    }
}