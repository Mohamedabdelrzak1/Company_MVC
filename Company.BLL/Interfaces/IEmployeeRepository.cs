using Company.DAL.Model;
using Company.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Interfaces
{
     public interface IEmployeeRepository : IGenericRepository<Employee>
     {
        //IEnumerable<Employee> GetAll();
        //Employee? GetByName(string name);
        //Employee? Get(int Id);

        //int Add(Employee model);
        //int Update(Employee model);
        //int Delete(Employee model);

    }
}
