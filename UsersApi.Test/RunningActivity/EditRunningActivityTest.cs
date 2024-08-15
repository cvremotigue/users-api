using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using UsersApi.Data.Contexts;
using UsersApi.Features.RunningActivity;

namespace UsersApi.Test.RunningActivity
{
    public class EditRunningActivityTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        [Fact]
        public async Task GivenEditRunningActivity_WhenRequestIsValid_ReturnsOk()
        {
            var factoryWithMockService = GetFactoryWithMockService();
            using var scope = factoryWithMockService.Services.CreateScope();
            var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
            dataSeeder.Cleanup();
            var userId = dataSeeder.SeedUser();
            var id = dataSeeder.SeedRunningActivity(userId);
            var client = factoryWithMockService.CreateClient();
            var request = new EditRunningActivityRequest()
            {
                Distance = 5,
                StartDate = DateTime.UtcNow.AddHours(-1),
                EndDate = DateTime.UtcNow,
                Location = "updated location"
            };

            var response = await client.PutAsJsonAsync($"runningactivity/{id}", request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<RunningActivityDetails>();
            result.Should().NotBeNull();
            result!.Location.Should().Be(request.Location);
        }

        [Fact]
        public async Task GivenEditRunningActivity_WhenActivityDoesNotExist_ReturnsNotFound()
        {
            var factoryWithMockService = GetFactoryWithMockService();
            var client = factoryWithMockService.CreateClient();
            var request = new EditRunningActivityRequest()
            {
                Distance = 5,
                StartDate = DateTime.UtcNow.AddHours(-1),
                EndDate = DateTime.UtcNow,
                Location = "updated location"
            };

            var response = await client.PutAsJsonAsync($"runningactivity/1", request);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GivenEditRunningActivity_WhenActivityRequestIsInvalid_ReturnsNotFound()
        {
            var client = factory.CreateClient();
            var request = new EditRunningActivityRequest()
            {
                Distance = 0,
                StartDate = DateTime.UtcNow.AddHours(1),
                EndDate = DateTime.UtcNow.AddHours(1),
                Location = "updated location"
            };

            var response = await client.PutAsJsonAsync($"runningactivity/1", request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            result!.Extensions["errors"]!.ToString().Should().Contain("Invalid Distance.");
            result!.Extensions["errors"]!.ToString().Should().Contain("Invalid StartDate.");
            result!.Extensions["errors"]!.ToString().Should().Contain("Invalid EndDate.");
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
