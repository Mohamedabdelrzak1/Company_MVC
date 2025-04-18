using Company.BLL.Interfaces;
using Company.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Interface
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        //IEnumerable<Department> GetAll();

        //Department? Get(int Id);

        //int Add(Department model);
        //int Update(Department model);
        //int Delete(Department model);

    }
}
