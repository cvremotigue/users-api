namespace UsersApi.Features.RunningActivity
{
    public class RunningActivityDetails
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Location { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public double Distance { get; set; }
        public TimeOnly Duration => GetDuration();
        public double AveragePace => GetAveragePace();

        private TimeOnly GetDuration()
        {
            var duration = EndDate - StartDate;
            return new TimeOnly(duration.Hours, duration.Minutes, duration.Seconds);
        }

        private double GetAveragePace()
        {
            var timeSec = (Duration.Hour * 3600) + (Duration.Minute * 60) + (Duration.Second);
            return Distance / (timeSec * 3600);
        }
    }
}
