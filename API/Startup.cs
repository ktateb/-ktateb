using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.DataContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.IO;
using AutoMapper;
using Microsoft.Extensions.FileProviders;
using Microsoft.EntityFrameworkCore;
using Services;
using DAL.Repositories;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Services.Profiles;
using DAL.Entities.Countries;
using DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using API.Extensions;
using FluentValidation;
using Model.User.Inputs;
using FluentValidation.AspNetCore;
using DAL.Entities.Categories;
using DAL.Entities.Courses;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserProfile));
            services.AddControllers();
            services.AddDbContext<StoreContext>(x =>
                x.UseSqlite(_configuration.GetConnectionString("DefaultConnection")));

            services.AddFluentValidation(fv =>
                fv.RegisterValidatorsFromAssemblyContaining<UserRegisterValidator>());
            services.AddFluentValidation(fv =>
                            fv.RegisterValidatorsFromAssemblyContaining<UserLoginValidator>());
            services.AddFluentValidation(fv =>
                                        fv.RegisterValidatorsFromAssemblyContaining<ChangePasswordValidator>());

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddScoped(typeof(IGenericRepository<Country>), typeof(GenericRepository<Country>));
            services.AddScoped(typeof(IGenericRepository<Category>), typeof(GenericRepository<Category>)); 

            services.AddScoped(typeof(IGenericRepository<Course>), typeof(GenericRepository<Course>)); 
            services.AddScoped<ICategoryServices, CategoryServices>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddIdentityServices(_configuration);

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Ktateb Project", Version = "1.0" });

                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Auth Bearer Scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                opt.AddSecurityDefinition("Bearer", securitySchema);
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {securitySchema , new [] {"Bearer"}}
                };
                opt.AddSecurityRequirement(securityRequirement);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("../swagger/v1/swagger.json", "Ktateb Project v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}