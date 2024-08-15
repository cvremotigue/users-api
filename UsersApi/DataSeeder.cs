using UsersApi.Core.Entities;
using UsersApi.Data.Contexts;
using UsersApi.Features.RunningActivity;

namespace UsersApi
{
    public class DataSeeder
    {
        private readonly UserDbContext _dataContext;

        public DataSeeder(UserDbContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public Guid SeedUser()
        {
            var user = new Core.Entities.Users() 
            {
                Birthdate = new DateTime(1987, 1, 1),
                FirstName = "Test1",
                LastName = "TestLast1",
                Height = 163,
                Weight = 60,
            };

            _dataContext.Add(user);
            _dataContext.SaveChanges();
            return user.Id;
        }

        public int SeedRunningActivity(Guid id)
        {
            var request = new Core.Entities.RunningActivities()
            {
                Distance = 5,
                StartDate = DateTime.UtcNow.AddHours(-1),
                EndDate = DateTime.UtcNow,
                Location = "location",
                UserId = id
            };

            _dataContext.Add(request);
            _dataContext.SaveChanges();
            return request.Id;
        }

        public void Cleanup()
        {
            foreach (var entity in _dataContext.RunningActivities)
                _dataContext.RunningActivities.Remove(entity);
            

            foreach (var entity in _dataContext.Users)
                _dataContext.Users.Remove(entity);

            _dataContext.SaveChanges();
        }
    }
}
