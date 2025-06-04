using Microsoft.EntityFrameworkCore;
using MultiTenantInventory.Infrastructure.Persistence;
using MultiTenantInventory.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register the DbContext with the connection string from configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString,
        npgsql => npgsql.MigrationsAssembly("MultiTenantInventory.Infrastructure")
    )
);

// Register the ITenantProvider service
builder.Services.AddScoped<ITenantProvider, TenantProvider>();
// Register AutoMapper with the assembly containing the mapping profiles
//builder.Services.AddAutoMapper(typeof(MappingProfile));

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.Run();