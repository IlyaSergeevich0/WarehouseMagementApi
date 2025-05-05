using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WarehouseManagementApi.Data;
using WarehouseManagementApi.Services;
using WarehouseManagementApi.Swagger;

internal sealed class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Add Swagger services
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options => {
            options.DocumentFilter<LowercaseDocumentFilter>();

            options.SwaggerDoc("v2", new OpenApiInfo {
                Title = "Warehouse Management API",
                Version = "v2",
                Description = "API for managing warehouses, storage zones, and products.",
                Contact = new OpenApiContact {
                    Name = "Support",
                    Email = "defaultdev0x101@gmail.com"
                }
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                Description = "JWT Authorization header using the Bearer scheme.\r\n\r\n " +
                      "Enter your token in the text input below.\r\n\r\n" +
                      "Example: \"eyJhbGciOi...\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
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
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "Warehouse Management API v2");
                options.RoutePrefix = "docs/swagger";
            });
        }

        app.UseHttpsRedirection();

        // Add Authentication middleware
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<WarehousesDbContext>();

            await dbContext.Database.EnsureCreatedAsync();
        }

        app.Run();

        app.Logger.LogInformation("Application started successfully.");
    }
}