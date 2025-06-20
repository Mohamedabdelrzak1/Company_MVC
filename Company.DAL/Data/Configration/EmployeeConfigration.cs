using Company.DAL.Model;
using Company.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Data.Configration
{
    public class EmployeeConfigration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(E => E.Salary).HasColumnType("decimal(18,2)");

            builder.HasOne(E => E.Department)         // موظف له قسم واحد
                   .WithMany(D => D.Employees)        // القسم يحتوي على العديد من الموظفين
                   .HasForeignKey(E => E.DepartmentId) // المفتاح الخارجي في Employee هو DepartmentId
                   .OnDelete(DeleteBehavior.SetNull); // عند حذف القسم، خلي DepartmentId = null في الموظفين

        }
    }
}
