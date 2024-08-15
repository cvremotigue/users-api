namespace UsersApi.Features.RunningActivity
{
    public class EditRunningActivityRequest
    {
        public string Location { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public double Distance { get; set; }
        public DateTimeOffset UpdatedAt => DateTimeOffset.UtcNow;
    }
}
