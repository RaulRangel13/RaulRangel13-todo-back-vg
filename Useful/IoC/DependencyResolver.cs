using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories.Base;
using Domain.Interfaces.Services;
using Domain.Interfaces.Services.Base;
using Domain.Services;
using Domain.Services.Base;
using Identity.Infraestructure;
using Identity.Services;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Useful.IoC
{
    public class DependencyResolver
    {
        public static void DependencyResolve(IServiceCollection services)
        {
            ServicesResolve(services);
            RepositoriesResolve(services);
            IdentityResolve(services);
            SwaggerResolve(services);
        }

        private static void ServicesResolve(IServiceCollection services)
        {
            services.AddScoped<ITodoService, TodoService>();
            services.AddScoped<IBaseService, BaseService>();
        }
        private static void RepositoriesResolve(IServiceCollection services)
        {
            services.AddScoped<ITodoRepository, TodoRepository>();
            services.AddScoped(typeof(IBaseRepository<>),typeof(BaseRepository<>));
        }
        private static void IdentityResolve(IServiceCollection services)
        {
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityDataContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IIdentityService, IdentityService>();
        }
        private static void SwaggerResolve(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            //services.AddSwaggerGen(options =>
            //{
            //    options.SwaggerDoc("v1", new OpenApiInfo
            //    {
            //        Title = "Way2Commerce.Api",
            //        Version = "v1"
            //    });

            //    options.SwaggerDoc("v2", new OpenApiInfo
            //    {
            //        Title = "Way2Commerce.Api",
            //        Version = "v2"
            //    });

            //    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //    {
            //        Description = @"JWT Authorization header using the Bearer scheme. 
            //                    Enter 'Bearer' [space] and then your token in the text input below. 
            //                    Example: 'Bearer 12345abcdef'",
            //        Name = "Authorization",
            //        In = ParameterLocation.Header,
            //        Type = SecuritySchemeType.ApiKey,
            //        Scheme = "Bearer"
            //    });

            //    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            //{
            //    {
            //    new OpenApiSecurityScheme
            //    {
            //        Reference = new OpenApiReference
            //        {
            //            Type = ReferenceType.SecurityScheme,
            //            Id = "Bearer"
            //        },
            //        Scheme = "oauth2",
            //        Name = "Bearer",
            //        In = ParameterLocation.Header,

            //        },
            //        new List<string>()
            //    }
            //});
            //});
        }
    }
}
