using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using UsersApi.Data.Contexts;

namespace UsersApi.Test.RunningActivity
{
    public class DeleteRunningActivityTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        [Fact]
        public async Task GivenDeleteRunningActivity_WhenRecordExists_ReturnsNoContent()
        {
            var factoryWithMockService = GetFactoryWithMockService();
            using var scope = factoryWithMockService.Services.CreateScope();
            var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
            dataSeeder.Cleanup();
            var userId = dataSeeder.SeedUser();
            var id = dataSeeder.SeedRunningActivity(userId);
            var client = factoryWithMockService.CreateClient();

            var response = await client.DeleteAsync($"runningactivity/{id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GivenDeleteRunningActivity_WhenRecordDoesNotExists_ReturnsNoContent()
        {
            var client = factory.CreateClient();

            var response = await client.DeleteAsync("runningactivity/1");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private WebApplicationFactory<Program> GetFactoryWithMockService()
        {
            var factoryWithMockService = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(x =>
                {
                    x.Remove(x.Single(a => a.ServiceType == typeof(DbContextOptions<UserDbContext>)));
                    x.Remove(x.Single(a => a.ServiceType == typeof(UserDbContext)));
                    x.AddDbContext<UserDbContext>(a =>
                    {
                        a.UseInMemoryDatabase("Tests");
                    });
                });
            });

            return factoryWithMockService;
        }
    }
}
