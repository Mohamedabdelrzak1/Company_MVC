using Company.BLL;
using Company.BLL.Interface;
using Company.BLL.Interfaces;
using Company.BLL.Repository;
using Company.BLL.Repositorys;
using Company.DAL.Data.Context;
using Company.DAL.Models;
using Company_MVC.Helpers;
using Company_MVC.Mapping;
using MailKit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SendEmailsWithDotNet8.Services;
using SendEmailsWithDotNet8.Settings;

namespace Company_MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>();  //Allow DI  for DepartmentRepository 
            //builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();  //Allow DI  for DepartmentRepository 
            builder.Services.AddScoped<IMailingService, MailingService>();  //Allow DI  for IMailService 
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddDbContext<CompanyDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            }); //Allow DI  for CompanyDbContext 

            //builder.Services.AddAutoMapper(typeof(Employeeprofile));
            builder.Services.AddAutoMapper(M => M.AddProfile(new Employeeprofile()));

            // ??????? ??????
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<CompanyDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";

            });

          //  builder.Services.AddAuthentication(options =>
          //  {
          //      options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
          //      options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
          //  })

          //.AddGoogle(options =>
          //{
          //    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
          //    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
          //});

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            // Life Time 

            //builder.Services.AddScoped();     // Create Object Life Time Per Request - UnReachable Object 
            //builder.Services.AddTransient();  // Create Object Life Time Per Operation
            //builder.Services.AddSingleton();  // Create Object Life Timer Per App 



            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}


