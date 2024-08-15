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
    public class EditUserTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        [Fact]
        public async Task GivenEditUser_UserExists_ReturnOk()
        {
            var factoryWithMockService = GetFactoryWithMockService();
            var client = factoryWithMockService.CreateClient();
            using var scope = factoryWithMockService.Services.CreateScope();
            var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
            dataSeeder.Cleanup();
            var id = dataSeeder.SeedUser();

            var request = new EditUserRequest()
            {
                Birthdate = new DateTime(1987, 01, 01),
                FirstName = " Updated First name",
                LastName = "Updated Last name",
                Height = 163,
                Weight = 60
            };

            var response = await client.PutAsJsonAsync($"user/{id}", request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<UserDetails>();
            result.Should().NotBeNull();
            result!.FirstName.Should().Be(request.FirstName);
            result!.LastName.Should().Be(request.LastName);
        }

        [Fact]
        public async Task GivenEditUser_UserDoesNotExists_ReturnNotFound()
        {
            var factoryWithMockService = GetFactoryWithMockService();
            var client = factoryWithMockService.CreateClient();
            var request = new EditUserRequest()
            {
                Birthdate = new DateTime(1987, 01, 01),
                FirstName = " Updated First name",
                LastName = "Updated Last name",
                Height = 163,
                Weight = 60
            };

            var response = await client.PutAsJsonAsync($"user/{new Guid()}", request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GivenEditUser_RequestHasInvalidParameters_ReturnBadRequest()
        {
            var factoryWithMockService = GetFactoryWithMockService();
            var client = factoryWithMockService.CreateClient();
            var request = new EditUserRequest()
            {
                Birthdate = DateTimeOffset.UtcNow,
                FirstName = " Updated First name",
                LastName = "Updated Last name",
                Height = 0,
                Weight = 0
            };

            var response = await client.PutAsJsonAsync($"user/{new Guid()}", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            result!.Extensions["errors"]!.ToString().Should().Contain("Invalid birth date.");
            result!.Extensions["errors"]!.ToString().Should().Contain("Invalid weight.");
            result!.Extensions["errors"]!.ToString().Should().Contain("Invalid height.");
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
