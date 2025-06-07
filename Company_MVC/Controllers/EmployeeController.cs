using AutoMapper;
using Company.BLL.Interface;
using Company.BLL.Interfaces;
using Company.BLL.Repository;
using Company.DAL.Model;
using Company.DAL.Models;
using Company_MVC.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Company_MVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _employeeRepository; //NULL
        //private IDepartmentRepository _departmentRepository;

        private readonly IMapper _mapper;


        //Ask CLR Create object From IDepartmentRepository
        public EmployeeController(
            //IEmployeeRepository employeeRepository , 
            //IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
           _unitOfWork = unitOfWork;
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _mapper = mapper;
        }


        [HttpGet] //Get : Department/index
        public IActionResult Index( string?  SearchInput)
        {


            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = _unitOfWork.EmployeeRepository.GetAll();
            }
            else
            {
                 employees = _unitOfWork.EmployeeRepository.GetByName(SearchInput);

            }


            //ViewData["Message"] = "An error occurred while saving your data. Please try again or contact technical support.";

            //ViewBag.Message = "An error occurred while saving your data. Please try again or contact technical support.";

            // Dictionary 3- proparty 

            //1.View Data : Transfer Extra Information From Controller (Action) To View
            //2.View Bag  : Transfer Extra Information From Controller (Action) To View
            //3.Temp Data

            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var department = _unitOfWork.DepartmentRepository.GetAll();

            //ViewData["department"] = department;
            ViewData["DepartmentList"] = new SelectList(department, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid) //Server Side Validation 
            {

                //Manual Mapping

                //var employee = new Employee()
                //{

                //    Name = model.Name,
                //    Address = model.Address,
                //    CreateAt = model.CreateAt,
                //    Email = model.Email,
                //    Phone = model.Phone,
                //    IsActive = model.IsActive,
                //    IsDeleted = model.IsDeleted,
                //    HiringDate = model.HiringDate,
                //    Age = model.Age,
                //    Salary = model.Salary,
                //    DepartmentId = model.DepartmentId

                //};
                var employee = _mapper.Map<Employee>(model);
                

               var count = _unitOfWork.EmployeeRepository.Add(employee);
                if (count > 0)
                {
                    TempData["Message"] = "Employee created successfully!";
                    TempData["MessageType"] = "create";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int? Id, string viewName = "Details")
        {
            if (Id is null) return BadRequest("Invalid Id"); // Status code = 400 

            var employee = _unitOfWork.EmployeeRepository.Get(Id.Value);
            if (employee is null)
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = $"Employee with Id: {Id} is not found"
                });

            
            // ✅ تحميل الأقسام لعرضها في Edit
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            ViewData["DepartmentList"] = new SelectList(departments, "Id", "Name", employee.DepartmentId);

            return View(viewName, employee);
        }



        [HttpGet]
        public IActionResult Edit(int? id)
        {

            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Employee employee)
        {
            if (id == employee.Id)
            {
                var count = _unitOfWork.EmployeeRepository.Update(employee);
                if (count > 0)
                {
                    TempData["Message"] = "Employee updated successfully!";
                    TempData["MessageType"] = "edit";

                    return RedirectToAction(nameof(Index));

                }

                
            }

            return View(employee);
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }


        [HttpPost]

        public IActionResult Delete([FromRoute] int id, Employee employee)
        {

            
                if (id == employee.Id)
                {
                    var count = _unitOfWork.EmployeeRepository.Delete(employee);
                    if (count > 0)
                    {
                    TempData["Message"] = "Employee deleted successfully!";
                    TempData["MessageType"] = "delete";

                    return RedirectToAction(nameof(Index));
                    }
                }

            

            return View(employee);
        }

    }
}
