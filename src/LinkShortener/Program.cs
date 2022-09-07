using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LinkShortener.Pl;
using LinkShortener.Dal;
using LinkShortener.Pl.Interfaces;
using LinkShortener.Dal.Interfaces;
using LinkShortener.Controllers;
using Serilog;
using FluentValidation;


var confBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, false);
var config = confBuilder.Build();

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()
	.WriteTo.File("log-.txt", rollingInterval:RollingInterval.Day)
	.CreateLogger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ILinkRepository,LinkRepository>();
builder.Services.AddScoped<IHasher,Hasher>();
builder.Services.AddScoped<ILinkConverter,ShortLinkConverter>();
builder.Services.AddSingleton<IConfiguration>(config);
builder.Services.AddSingleton(x => new LinkRepositorySettings(
	x.GetRequiredService<IConfiguration>().GetValue<string>("ConnectionStrings:PostgresTable")));
builder.Services.AddScoped<ILinkService,LinkService>();
//builder.Services.AddScoped<IValidator,LinkValidator>(x => new LinkValidator(x.GetRequiredService<IConfiguration>().GetValue<int>("MaxLength")));
builder.Services.AddValidatorsFromAssemblyContaining<LinkValidator>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.MapControllers();
app.Run();