using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using UsersApi.Data.Contexts;
using UsersApi.Features.RunningActivity;

namespace UsersApi.Test.RunningActivity
{
    public class GetRunningActivityTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        [Fact]
        public async Task GivenGetRunningActivity_WhenActivityExists_ReturnsCreatedStatus()
        {
            var factoryWithMockService = GetFactoryWithMockService();
            using var scope = factoryWithMockService.Services.CreateScope();
            var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
            dataSeeder.Cleanup();
            var userId = dataSeeder.SeedUser();
            var id = dataSeeder.SeedRunningActivity(userId);
            var client = factoryWithMockService.CreateClient();

            var response = await client.GetAsync($"runningactivity/{userId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<List<RunningActivityDetails>>();
            result.Should().NotBeNull();
            result!.Count.Should().Be(1);
        }

        [Fact]
        public async Task GivenGetRunningActivity_WhenActivityDoesNotatedStatus()
        {
            var factoryWithMockService = GetFactoryWithMockService();
            var client = factoryWithMockService.CreateClient();

            var response = await client.GetAsync($"runningactivity/{new Guid()}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<List<RunningActivityDetails>>();
            result.Should().BeNullOrEmpty();
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
