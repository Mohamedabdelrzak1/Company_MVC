using Company.BLL.Interfaces;
using Company.DAL.Data.Context;
using Company.DAL.Model;
using Company.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repositorys
{
    public class GenericRepository<T> : IGenericRepository<T> where T :BaseEntity
    {
        private readonly CompanyDbContext _context; //NULL

        //Ask CLR Create object From CompanyDbContext

        public GenericRepository(CompanyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Employee)) //حل مؤقت 
            {
                return  (IEnumerable<T>) await _context.Employees.Include(E => E.Department).ToListAsync();
            }

                return await _context.Set<T>().ToListAsync();
        }
        public async Task<T?> GetAsync(int Id)
        {
            if (typeof(T) == typeof(Employee))
            {
                return await _context.Employees.Include(E => E.Department).FirstOrDefaultAsync(E => E.Id == Id) as T;
            }

            return await _context.Set<T>().FindAsync(Id);
        }
        public async Task<int> AddAsync(T model)
        {
            await _context.Set<T>().AddAsync(model);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> Update(T model)
        {
            _context.Set<T>().Update(model);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(T model)
        {
            _context.Set<T>().Remove(model);
            return await _context.SaveChangesAsync();
        }

       

       

      
    }
}
