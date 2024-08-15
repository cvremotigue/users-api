using Serilog;
using UsersApi;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var startup = new Startup(builder.Configuration);
startup.ConfigureService(builder.Services);

var app = builder.Build();

startup.Configure(app, builder.Environment);

public partial class Program { }