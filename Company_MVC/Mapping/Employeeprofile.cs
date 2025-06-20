using AutoMapper;
using Company.DAL.Models;
using Company_MVC.Dtos;

namespace Company_MVC.Mapping
{
    public class Employeeprofile: Profile
    {
        public Employeeprofile()
        {
            CreateMap<CreateEmployeeDto, Employee>().ReverseMap(); 
            CreateMap<Employee, CreateEmployeeDto>();
        }
    }
}
