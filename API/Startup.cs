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
using DAL.Entities.Reports;
using Model.Report.User.Inputs;
using Model.Report.Comment.Inputs;
using Model.Report.Message.Inputs;
using Model.Report.Course.Inputs;
using DAL.Entities.Countries;
using DAL.Entities.Ratings;
using Model.Rating.Inputs;
using DAL.Entities.Comments;
using DAL.Entities.StudentCourses;
using DAL.Entities.Teachers;
using DAL.Entities.StudentWatches;
using DAL.Entities.StudentFavoriteCourses;
using Services.Services;
using Services.Hubs;

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
            #region AutoMapper
            //ahmad
            services.AddAutoMapper(typeof(UserProfile));
            services.AddAutoMapper(typeof(RoleProfile));
            services.AddAutoMapper(typeof(MessageProfile));
            services.AddAutoMapper(typeof(ReportProfile));
            services.AddAutoMapper(typeof(ReportProfile));
            services.AddAutoMapper(typeof(CountryProfile));
            services.AddAutoMapper(typeof(RatingProfile));
            // sarya 
            services.AddAutoMapper(typeof(CategoryProfile));
            services.AddAutoMapper(typeof(CourseProfile));
            services.AddAutoMapper(typeof(VedioProfile));
            services.AddAutoMapper(typeof(CourseSectionProfile));
            services.AddAutoMapper(typeof(TeacherProfile));
            services.AddAutoMapper(typeof(CommentProfile));
            services.AddAutoMapper(typeof(SubCommentProfile));
            services.AddAutoMapper(typeof(StudentFavoriteCourseProfile));
            services.AddAutoMapper(typeof(WatchedVedioProfile));
            #endregion

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
            services.AddFluentValidation(fv =>
                fv.RegisterValidatorsFromAssemblyContaining<ReportUserInputValidator>());
            services.AddFluentValidation(fv =>
                fv.RegisterValidatorsFromAssemblyContaining<ReportCommentInputValidator>());
            services.AddFluentValidation(fv =>
                fv.RegisterValidatorsFromAssemblyContaining<ReportMessageInputValidator>());
            services.AddFluentValidation(fv =>
                fv.RegisterValidatorsFromAssemblyContaining<ReportCourseInputValidator>());
            services.AddFluentValidation(fv =>
            fv.RegisterValidatorsFromAssemblyContaining<RatingInputValidator>());
            #endregion

            #region Dependency Injection

            //// ahmad Services
            services.AddScoped(typeof(IGenericRepository<Message>), typeof(GenericRepository<Message>));
            services.AddScoped(typeof(IGenericRepository<ReportCourse>), typeof(GenericRepository<ReportCourse>));
            services.AddScoped(typeof(IGenericRepository<ReportComment>), typeof(GenericRepository<ReportComment>));
            services.AddScoped(typeof(IGenericRepository<ReportSubComment>), typeof(GenericRepository<ReportSubComment>));
            services.AddScoped(typeof(IGenericRepository<ReportUser>), typeof(GenericRepository<ReportUser>));
            services.AddScoped(typeof(IGenericRepository<ReportMessage>), typeof(GenericRepository<ReportMessage>));
            services.AddScoped(typeof(IGenericRepository<Country>), typeof(GenericRepository<Country>));
            services.AddScoped(typeof(IGenericRepository<Rating>), typeof(GenericRepository<Rating>));
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ITokenService, TokenService>();
            ///// sarya Services
            services.AddScoped(typeof(IGenericRepository<Teacher>), typeof(GenericRepository<Teacher>));
            services.AddScoped(typeof(IGenericRepository<Category>), typeof(GenericRepository<Category>));
            services.AddScoped(typeof(IGenericRepository<Course>), typeof(GenericRepository<Course>));
            services.AddScoped(typeof(IGenericRepository<CourseSection>), typeof(GenericRepository<CourseSection>));
            services.AddScoped(typeof(IGenericRepository<CourseVedio>), typeof(GenericRepository<CourseVedio>));
            services.AddScoped(typeof(IGenericRepository<StudentCourse>), typeof(GenericRepository<StudentCourse>));
            services.AddScoped(typeof(IGenericRepository<Comment>), typeof(GenericRepository<Comment>));
            services.AddScoped(typeof(IGenericRepository<SubComment>), typeof(GenericRepository<SubComment>));
            services.AddScoped(typeof(IGenericRepository<StudentWatchedVedio>), typeof(GenericRepository<StudentWatchedVedio>));
            services.AddScoped(typeof(IGenericRepository<StudentFavoriteCourse>), typeof(GenericRepository<StudentFavoriteCourse>));
            services.AddScoped(typeof(IGenericRepository<CoursePriceHistory>), typeof(GenericRepository<CoursePriceHistory>));
            services.AddScoped<ICategoryServices, CategoryServices>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ICourseSectionService, CourseSectionService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ISubCommentService, SubCommentService>();
            services.AddScoped<IStudentWatchesService, StudentWatchesService>();
            services.AddScoped<IFavoriteCoursesService, FavoriteCoursesService>();
            services.AddScoped<IVedioService, VedioService>();

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

                opt.AddSecurityDefinition(securitySchema.Reference.Id, securitySchema);
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {securitySchema , System.Array.Empty<string>()}
                };
                opt.AddSecurityRequirement(securityRequirement);
                // opt.OperationFilter<AppendAuthoriziton>();
            });
            #endregion

            services.AddSignalR();
            services.AddCors();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x.WithOrigins("https://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                // .AllowCredentials()
                );

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
                endpoints.MapHub<ChatHub>("/ChatSocket");
            });
        }
    }
}