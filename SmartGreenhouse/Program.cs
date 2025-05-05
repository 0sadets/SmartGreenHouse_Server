
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartGreenhouse.Interfaces;
using SmartGreenhouse.Models.DTOs;
using SmartGreenhouse.Models.Entities;
using SmartGreenhouse.Repositories;
using SmartGreenhouse.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
namespace SmartGreenhouse
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers()
           .AddJsonOptions(options =>
           {
               options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
           });

            // Add services to the container.
            string connString = builder.Configuration.GetConnectionString("LocalDb")!;
            builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connString));

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            builder.Services.AddIdentity<AppUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            // validators
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return new BadRequestObjectResult(new { success = false, errors });
                };
            });
            builder.Services.AddControllers();
            // services
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IPlantService, PlantService>();
            builder.Services.AddScoped<IGreenhouseService, GreenhouseService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            builder.Services.AddValidatorsFromAssemblyContaining<Program>();
            




            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(Program));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();



            app.MapControllers();

            app.Run();
        }
    }

}
