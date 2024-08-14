using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using UsersApi.Data.Contexts;

namespace UsersApi.Test.User
{
    public class DeleteUserTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        [Fact]
        public async Task GivenDeleteUser_WhenUserExists_ReturnNoContent()
        {
            var factoryWithMockService = GetFactoryWithMockService();
            var client = factoryWithMockService.CreateClient();
            using var scope = factoryWithMockService.Services.CreateScope();
            var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
            dataSeeder.Cleanup();
            var id = dataSeeder.SeedUser();

            var response = await client.DeleteAsync($"user/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GivenDeleteUser_WhenUserDoesNotExists_ReturnNotFound()
        {
            var factoryWithMockService = GetFactoryWithMockService();
            var client = factoryWithMockService.CreateClient();

            var response = await client.DeleteAsync($"user/{new Guid()}");

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
