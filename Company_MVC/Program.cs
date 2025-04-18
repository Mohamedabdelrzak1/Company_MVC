using Company.BLL.Interface;
using Company.BLL.Interfaces;
using Company.BLL.Repository;
using Company.BLL.Repositorys;
using Company.DAL.Data.Context;
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
            builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>();  //Allow DI  for DepartmentRepository 
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();  //Allow DI  for DepartmentRepository 
            builder.Services.AddDbContext<CompanyDbContext>(option =>
                {
                    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            }); //Allow DI  for CompanyDbContext 

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

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
