using Company.BLL.Interface;
using Company.BLL.Repositorys;
using Company.DAL.Data.Context;
using Company.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repository
{
   public class DepartmentRepository : GenericRepository<Department> , IDepartmentRepository
    {
        public DepartmentRepository(CompanyDbContext context) :  base( context) //Ask CLR Create object From CompanyDbContext
        {
            
        }
    }
}
