namespace UsersApi.Features.User
{
    public class UserDetails
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public DateTimeOffset Birthdate { get; set; }
        public bool IsDeleted { get; set; }
        public string Name => $"{FirstName} {LastName}";
        public double BMI => Weight / (Math.Pow(Height / 100.0, 2));

        public int Age => GetAge();

        private int GetAge()
        {
            var currentDate = DateTimeOffset.UtcNow;
            var age = currentDate.Year - Birthdate.Year;
            if (currentDate.Month < Birthdate.Month || (currentDate.Month == Birthdate.Month && currentDate.Day < Birthdate.Day))
            {
                age--;
            }

            return age;
        }
    }
}
