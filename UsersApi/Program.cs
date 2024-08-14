using UsersApi;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureService(builder.Services);

var app = builder.Build();

startup.Configure(app, builder.Environment);

public partial class Program { }