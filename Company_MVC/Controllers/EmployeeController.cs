using Company.BLL.Interface;
using Company.BLL.Interfaces;
using Company.DAL.Model;
using Company.DAL.Models;
using Company_MVC.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Company_MVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository; //NULL

        //Ask CLR Create object From IDepartmentRepository
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }


        [HttpGet] //Get : Department/index
        public IActionResult Index()
        {

            var employees = _employeeRepository.GetAll();

            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid) //Server Side Validation 
            {
                var employee = new Employee()
                {
                    
                    Name = model.Name,
                    Address = model.Address,
                    CreateAt = model.CreateAt,
                    Email = model.Email,
                    Phone = model.Phone,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    HiringDate = model.HiringDate,
                    Age = model.Age,
                    Salary = model.Salary

                };
                var count = _employeeRepository.Add(employee);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int? Id, string viewName = "Details")
        {
            if (Id is null) return BadRequest("Invaled Id"); //Satues code = 400 

            var employee = _employeeRepository.Get(Id.Value);

            if (employee is null) return NotFound(new { SatuesCode = 404, Message = $"Department With Id :{Id} is not found" });

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

            if (ModelState.IsValid) //Server Side Validation 
            {
                if (id == employee.Id)
                {
                    var count = _employeeRepository.Update(employee);
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
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

            if (ModelState.IsValid) //Server Side Validation 
            {
                if (id == employee.Id)
                {
                    var count = _employeeRepository.Delete(employee);
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

            }

            return View(employee);
        }

    }
}
