using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using UsersApi.Data.Contexts;
using UsersApi.Features.User;

namespace UsersApi.Test.User
{
    public class GetUserTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        [Fact]
        public async Task GivenGetUser_UserDoesNotExist_ReturnNotFound()
        {
            var client = factory.CreateClient();

            var response = await client.GetAsync($"user/{new Guid()}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GivenGetUser_UserExists_ReturnOk()
        {
            var factoryWithMockService = GetFactoryWithMockService();

            var client = factoryWithMockService.CreateClient();

            using var scope = factoryWithMockService.Services.CreateScope();
            var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
            dataSeeder.Cleanup();
            var id = dataSeeder.SeedUser();

            var response = await client.GetAsync($"user/{id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<UserDetails>();
            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
                
        }

        [Fact]
        public async Task GivenGetUserList_UserExists_ReturnOk()
        {
            var factoryWithMockService = GetFactoryWithMockService();

            var client = factoryWithMockService.CreateClient();

            using var scope = factoryWithMockService.Services.CreateScope();
            var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
            dataSeeder.Cleanup();
            dataSeeder.SeedUser();

            var response = await client.GetAsync($"user/list");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<List<UserDetails>>();
            result.Should().NotBeNull();
            result!.Count.Should().Be(1);
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
