using Company.BLL;
using Company.BLL.Interface;
using Company.BLL.Interfaces;
using Company.BLL.Repository;
using Company.BLL.Repositorys;
using Company.DAL.Data.Context;
using Company_MVC.Mapping;
using Microsoft.EntityFrameworkCore;

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
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddDbContext<CompanyDbContext>(option =>
                {
                    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            }); //Allow DI  for CompanyDbContext 

            //builder.Services.AddAutoMapper(typeof(Employeeprofile));
            builder.Services.AddAutoMapper(M => M.AddProfile(new Employeeprofile()));

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
