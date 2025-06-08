using Microsoft.EntityFrameworkCore;
using MultiTenantInventory.API.Middleware;
using MultiTenantInventory.Application.Common.Interfaces;
using MultiTenantInventory.Infrastructure.Extensions;
using MultiTenantInventory.Infrastructure.Persistence;
using MultiTenantInventory.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Register the application database context with Entity Framework Core
builder.Services.AddApplicationDbContext(builder.Configuration);

// Register jwt authentication and authorization services
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

// Register the ITenantProvider service
builder.Services.AddScoped<ITenantProvider, TenantProvider>();
// Register the IJwtTokenService for JWT token generation
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
// Register AutoMapper with the assembly containing the mapping profiles
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//builder.Services.AddOpenApi();

var app = builder.Build();

// Ensure migrations are applied at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MultiTenantInventory API V1");
        c.RoutePrefix = "swagger"; // Swagger UI at /swagger (e.g. https://localhost:5001/swagger)
    });
}

// Register TenantResolutionMiddleware to resolve the tenant from the request
app.UseMiddleware<TenantResolutionMiddleware>();
// Register ExceptionHandlingMiddleware to handle exceptions globally
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();