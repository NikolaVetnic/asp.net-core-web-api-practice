using Discount.Grpc.Data;
using Discount.Grpc.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddDbContext<DiscountContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Database")));

// This was required to circumvent the "The SSL connection could not be established. Cannot determine the frame size or a corrupted frame was received." exception
builder.WebHost.ConfigureKestrel(options =>
{
    var env = builder.Environment.EnvironmentName;

    options.ListenAnyIP(8080, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2; // Listen on port 8080 for Docker
    });

    // Optional: Add development fallback
    if (env == "Development")
        options.ListenAnyIP(5002, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http2; // Local testing fallback
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMigration();
app.MapGrpcService<DiscountService>();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();