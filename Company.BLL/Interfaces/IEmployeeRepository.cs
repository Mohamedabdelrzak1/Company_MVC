﻿using Company.DAL.Model;
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

         Task<List<Employee>> GetByNameAsync(string name);
        
    }
}
