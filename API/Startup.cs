using DAL.DataContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Services;
using DAL.Repositories;
using Services.Profiles;
using API.Extensions;
using Model.User.Inputs;
using FluentValidation.AspNetCore;
using DAL.Entities.Categories;
using DAL.Entities.Courses;
using DAL.Entities.Messages;
using Model.Message.Inputs;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserProfile));
            services.AddControllers();
            services.AddDbContext<StoreContext>(x =>
                x.UseSqlite(_configuration.GetConnectionString("DefaultConnection")));

            #region FluentValidation
            services.AddFluentValidation(fv =>
                fv.RegisterValidatorsFromAssemblyContaining<UserRegisterValidator>());
            services.AddFluentValidation(fv =>
                fv.RegisterValidatorsFromAssemblyContaining<UserLoginValidator>());
            services.AddFluentValidation(fv =>
                fv.RegisterValidatorsFromAssemblyContaining<ChangePasswordValidator>());
            services.AddFluentValidation(fv =>
                fv.RegisterValidatorsFromAssemblyContaining<UpdateMessageInputValidator>());
            services.AddFluentValidation(fv =>
                fv.RegisterValidatorsFromAssemblyContaining<MessageInputValidator>());
            services.AddFluentValidation(fv =>
                fv.RegisterValidatorsFromAssemblyContaining<UserUpdateValidator>());
            #endregion

            #region Dependency Injection
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddScoped(typeof(IGenericRepository<Category>), typeof(GenericRepository<Category>));
            services.AddScoped(typeof(IGenericRepository<Message>), typeof(GenericRepository<Message>));
            services.AddScoped(typeof(IGenericRepository<Course>), typeof(GenericRepository<Course>));
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<ICategoryServices, CategoryServices>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddIdentityServices(_configuration);
            #endregion

            #region Swagger
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
            #endregion

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

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