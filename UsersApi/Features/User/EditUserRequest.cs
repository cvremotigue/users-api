using System.ComponentModel.DataAnnotations;

namespace UsersApi.Features.User
{
    public class EditUserRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public double Height { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public DateTimeOffset Birthdate { get; set; }
        public DateTimeOffset UpdatedAt => DateTimeOffset.UtcNow;
    }
}
