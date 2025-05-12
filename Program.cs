using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Load configuration
var configuration = builder.Configuration;

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();            // Swagger endpoint discovery
builder.Services.AddSwaggerGen();                      // Swagger UI

// Configure Entity Framework and SQL Server
builder.Services.AddDbContext<BookContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Enable CORS for frontend (adjust origin as needed)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(); // visit /swagger for docs
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

// Optional HTTPS redirection (disabled for local dev)
// app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
