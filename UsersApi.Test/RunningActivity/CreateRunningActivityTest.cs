using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UsersApi.Features.User;
using UsersApi.Features.RunningActivity;
using Microsoft.EntityFrameworkCore;
using UsersApi.Data.Contexts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;

namespace UsersApi.Test.RunningActivity
{
    public class CreateRunningActivityTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        [Fact]
        public async Task GivenCreateRunningActivity_WhenRequestIsInvalid_ReturnsBadRequest()
        {
            var client = factory.CreateClient();
            var request = new CreateRunningActivityRequest()
            {
                Distance = 0,
                StartDate = DateTime.UtcNow.AddHours(1),
                EndDate = DateTime.UtcNow.AddHours(1),
                Location = "location",
                UserId = default
            };

            var response = await client.PostAsJsonAsync("runningactivity", request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            result!.Extensions["errors"]!.ToString().Should().Contain("Invalid Distance.");
            result!.Extensions["errors"]!.ToString().Should().Contain("Invalid StartDate.");
            result!.Extensions["errors"]!.ToString().Should().Contain("Invalid EndDate.");
            result!.Extensions["errors"]!.ToString().Should().Contain("Invalid UserId.");
        }

        [Fact]
        public async Task GivenCreateRunningActivity_WhenActivityIsCreated_ReturnsCreatedStatus()
        {
            var factoryWithMockService = GetFactoryWithMockService();
            using var scope = factoryWithMockService.Services.CreateScope();
            var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
            dataSeeder.Cleanup();
            var id = dataSeeder.SeedUser();
            var client = factoryWithMockService.CreateClient();
            var request = new CreateRunningActivityRequest()
            {
                Distance = 5,
                StartDate = DateTime.UtcNow.AddHours(-1),
                EndDate = DateTime.UtcNow,
                Location = "location",
                UserId = id
            };

            var response = await client.PostAsJsonAsync("runningactivity", request);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
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
