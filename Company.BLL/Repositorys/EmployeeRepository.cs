using Company.BLL.Interfaces;
using Company.DAL.Data.Context;
using Company.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Company.BLL.Repositorys
{
    public class EmployeeRepository : GenericRepository<Employee> , IEmployeeRepository
    {
        public EmployeeRepository(CompanyDbContext context) :base( context)  //Ask CLR Create object From CompanyDbContext
        {
            
        }

    }
}
