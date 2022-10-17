using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Services;
using Identity.Configurations;
using Identity.Infraestructure;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Useful.IoC;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            DependencyResolver.DependencyResolve(builder.Services);
            builder.Services.AddDbContext<TodoContext>(options => options.UseNpgsql("Server=localhost;Port=5432;Database=TodoList;User Id=postgres;Password=root"));
            builder.Services.AddDbContext<IdentityDataContext>(options => options.UseNpgsql("Server=localhost;Port=5432;Database=TodoList;User Id=postgres;Password=root"));

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var jwtAppSettingOptions = builder.Configuration.GetSection(nameof(JwtOptions));
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtOptions:SecurityKey").Value));

            builder.Services.Configure<JwtOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
                options.AccessTokenExpiration = int.Parse(jwtAppSettingOptions[nameof(JwtOptions.AccessTokenExpiration)] ?? "0");
                options.RefreshTokenExpiration = int.Parse(jwtAppSettingOptions[nameof(JwtOptions.RefreshTokenExpiration)] ?? "0");
            });

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration.GetSection("JwtOptions:Issuer").Value,

                ValidateAudience = true,
                ValidAudience = builder.Configuration.GetSection("JwtOptions:Audience").Value,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
            });

            builder.Services.AddControllers();

            //builder.Services.AddEntityFrameworkNpgsql()
            //    .AddDbContext<TodoContext>(option => option.UseNpgsql("Server=localhost;Port=5432;Database=TodoList;User Id=postgres;Password=root"));

            //builder.Services.AddScoped<ITodoService, TodoService>();
            //builder.Services.AddTransient<ITodoRepository, TodoRepository>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Way2Commerce.Api",
                    Version = "v1"
                });

                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "Way2Commerce.Api",
                    Version = "v2"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. 
                                Enter 'Bearer' [space] and then your token in the text input below. 
                                Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}