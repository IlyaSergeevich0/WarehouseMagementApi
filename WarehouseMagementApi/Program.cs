using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WarehouseMagementApi.Data;
using WarehouseMagementApi.Services;

internal sealed class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Add Swagger services
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options => {
            options.SwaggerDoc("v1", new OpenApiInfo {
                Title = "Warehouse Management API",
                Version = "v1",
                Description = "API for managing warehouses, storage zones, and products.",
                Contact = new OpenApiContact {
                    Name = "Support",
                    Email = "defaultdev0x101@gmail.com"
                }
            });
        });

        // Add PostgreSQL
        builder.Services.AddDbContext<WarehousesDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection")));

        // Add Redis
        builder.Services.AddStackExchangeRedisCache(options => {
            options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "WarehouseApp_";
        });

        // Add Authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

        // Register AuthService
        builder.Services.AddScoped<AuthService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.Logger.LogInformation("Using Development mode!");

            app.UseDeveloperExceptionPage();

            // Enable Swagger in development
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Warehouse Management API v1");
                options.RoutePrefix = string.Empty; // Swagger UI at the root
            });            
        }

        app.UseHttpsRedirection();

        // Add Authentication middleware
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();

        app.Logger.LogInformation("Application started successfully.");
    }
}