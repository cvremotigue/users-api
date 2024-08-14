using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using UsersApi.Data.Contexts;
using UsersApi.Features.User;

namespace UsersApi.Test.User
{
    public class CreateUserTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        [Fact]
        public async Task GivenCreateUser_WhenRequestIsInvalid_ReturnsBadRequest()
        {
            var client = factory.CreateClient();
            var request = new CreateUserRequest()
            {
                Birthdate = new DateTime(1987, 01, 01),
                FirstName = "First name",
                LastName = "Last name",
                Height = 0,
                Weight = 0
            };

            var response = await client.PostAsJsonAsync("user", request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var result = await response.Content.ReadAsStringAsync();
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GivenCreateUser_WhenUserIsCreated_ReturnsCreatedStatus()
        {
            var client = GetClientWithMockService();
            var request = new CreateUserRequest()
            {
                Birthdate = new DateTime(1987, 01, 01),
                FirstName = "First name",
                LastName = "Last name",
                Height = 163,
                Weight = 60
            };

            var response = await client.PostAsJsonAsync("user", request);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        private HttpClient GetClientWithMockService()
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

            return factoryWithMockService.CreateClient();
        }
    }
}
