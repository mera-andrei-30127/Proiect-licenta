using AspNetCoreHero.ToastNotification;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplicationForDidacticPurpose.BL.Services;
using WebApplicationForDidacticPurpose.BL.Interfaces;
using WebApplicationForDidacticPurpose.DAL;
using AspNetCoreHero.ToastNotification.Extensions;
using WebApplicationForDidacticPurpose.DAL.Models;
using WebApplicationForDidacticPurpose.MODELS.ViewModelsValidators.Attendee;
using WebApplicationForDidacticPurpose.MODELS.ViewModels.Attendee;
using WebApplicationForDidacticPurpose.Controllers;
using WebApplicationForDidacticPurpose.Automapper;

namespace WebApplicationForDidacticPurpose
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<WebApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("LocalDbConnection")));

            services.AddNotyf(config => { config.DurationInSeconds = 3; config.IsDismissable = true; config.Position = NotyfPosition.BottomRight; });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 1;

            });

            services.AddIdentity<User, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<WebApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(opts =>
            {
                opts.LoginPath = "/Home/Login";
                opts.Cookie.Name = "cookie";
                opts.Cookie.SameSite = SameSiteMode.None;
            });

            services.AddAutoMapper(typeof(MappingProfile)); 

            services.AddNotyf(config => { config.DurationInSeconds = 3; config.IsDismissable = true; config.Position = NotyfPosition.BottomRight; });

            //Services
            services.AddScoped<IAttendeeService, AttendeeService>();
            services.AddScoped<IHomeworkService, HomeworkService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUpdateFileService, UpdateFileService>();
            services.AddScoped<IGitHubService, GitHubService>();
            services.AddScoped<ICompare2StringsService, Compare2StringsService>();
            services.AddScoped<IAttendeeHomeworksService, AttendeeHomeworksService>();

            //Validators
            services.AddTransient<IValidator<AttendeeViewModel>, AttendeeValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}/{id?}");
            });

            app.UseNotyf();
        }

    }
}
