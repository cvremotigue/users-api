using System.ComponentModel.DataAnnotations;

namespace UsersApi.Features.User
{
    public class CreateUserRequest
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
        public bool IsDeleted => false;
        public DateTimeOffset CreatedAt => DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt => DateTimeOffset.UtcNow;
    }
}
