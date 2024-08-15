using System.ComponentModel.DataAnnotations;

namespace UsersApi.Features.RunningActivity
{
    public class CreateRunningActivityRequest
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public DateTimeOffset StartDate { get; set; }
        [Required]
        public DateTimeOffset EndDate { get; set; }
        [Required]
        public double Distance { get; set; }

        public DateTimeOffset CreatedAt => DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt => DateTimeOffset.UtcNow;
    }
}
