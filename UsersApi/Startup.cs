using FluentValidation;
using Serilog;
using UsersApi.Features.RunningActivity;
using UsersApi.Features.User;
using UsersApi.Infrastructure;
using UsersApi.Validations;

namespace UsersApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureService(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperStartupExtensions));
            services.AddDataAccess(Configuration);
            services.AddScoped<UserService>();
            services.AddScoped<RunningActivityService>();
            services.AddScoped<UserValidationService>();
            services.AddScoped<RunningActivityValidationService>();
            services.AddScoped<IValidator<CreateUserRequest>, CreateUserValidator>();
            services.AddScoped<IValidator<EditUserRequest>, EditUserValidator>();
            services.AddScoped<IValidator<CreateRunningActivityRequest>, CreateRunningActivityValidator>();
            services.AddScoped<IValidator<EditRunningActivityRequest>, EditRunningActivityValidator>();
            services.AddScoped<DataSeeder>();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddAutoMapper(typeof(AutoMapperStartupExtensions));
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            app.ExecuteStartupTasks();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
