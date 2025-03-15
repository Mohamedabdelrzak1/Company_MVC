using Company.BLL.Interface;
using Company.DAL.Data.Context;
using Company.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repository
{
    class DepartmentRepository : IDepartmentRepository
    {
        private readonly CompanyDbContext _context;

        public DepartmentRepository(CompanyDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Department> GetAll()
        {
            return _context.Departments.ToList();
        }

        public Department? Get(int Id)
        {
            return _context.Departments.Find(Id);
        }


        public int Add(Department model)
        {

            _context.Departments.Add(model);
            return _context.SaveChanges();
               
        }
        public int Update(Department model)
        {
            _context.Departments.Update(model);
            return _context.SaveChanges();

        }

        public int Delete(Department model)
        {
            _context.Departments.Remove(model);
            return _context.SaveChanges();

        }




    }
}
