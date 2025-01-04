using OpenCVAspNetTest;
using OpenCVAspNetTest.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddHostedService<ImageCreationHostedService>();

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapFallbackToFile("./index.html");

app.MapHub<ImageHub>("/image");

app.Run();
