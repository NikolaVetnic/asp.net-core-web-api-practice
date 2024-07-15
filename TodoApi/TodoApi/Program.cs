using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

// Add DI services
builder.Services.AddDbContext<TodoDbContext>(opt => opt.UseInMemoryDatabase("TodoItems"));
builder.Services.AddControllers();

var app = builder.Build();

// Configure pipeline
//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//}

//app.UseHttpsRedirection();

//app.UseRouting();

//app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program; // This partial class is needed for the integration tests