using AutoMapper;
using Company.BLL.Interface;
using Company.BLL.Interfaces;
using Company.BLL.Repository;
using Company.DAL.Model;
using Company.DAL.Models;
using Company_MVC.Dtos;
using Company_MVC.Helpers;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Company_MVC.Controllers
{

    [Authorize]
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
        public async Task<IActionResult> Index(string? SearchInput)
        {


            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);

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
        public async Task<IActionResult> Create()
        {
            var department = await _unitOfWork.DepartmentRepository.GetAllAsync();

            //ViewData["department"] = department;
            ViewData["DepartmentList"] = new SelectList(department, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid) //Server Side Validation 
            {

                if (model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }

                var employee = _mapper.Map<Employee>(model);


                var count = await _unitOfWork.EmployeeRepository.AddAsync(employee);
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
        public async Task<IActionResult> Details(int? Id)
        {


            if (Id is null) return BadRequest("Invalid Id"); // Status code = 400 

            var employee = await _unitOfWork.EmployeeRepository.GetAsync(Id.Value);
            if (employee is null)
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = $"Employee with Id: {Id} is not found"
                });
            return View(employee);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest("Invalid Id");

            var employee = await  _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (employee == null)
                return NotFound($"Employee with Id {id} not found");

            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["DepartmentList"] = new SelectList(departments, "Id", "Name", employee.DepartmentId);

            // صححنا هنا عشان نستخدم DTO للعرض
            var dto = _mapper.Map<CreateEmployeeDto>(employee);

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, CreateEmployeeDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id);
            if (employee == null)
                return NotFound();

            if (model.Image != null)
            {
                if (!string.IsNullOrEmpty(employee.ImageName))
                {
                    DocumentSettings.DeleteFile(employee.ImageName, "images");
                }
                model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
            }

            // تحديث بيانات الموظف من الـ DTO
            _mapper.Map(model, employee);

            var count = await _unitOfWork.EmployeeRepository.Update(employee);

            if (count > 0)
            {
                TempData["Message"] = "Employee updated successfully!";
                TempData["MessageType"] = "edit";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest("Invalid Id");

            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (employee == null)
                return NotFound($"Employee with Id {id} not found");

            return View(employee);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                ModelState.AddModelError("", "Id mismatch.");
                return View(employee);
            }

            var count = await _unitOfWork.EmployeeRepository.Delete(employee);
            if (count > 0)
            {

                if (employee.ImageName is not null)
                {
                    DocumentSettings.DeleteFile(employee.ImageName, "images");
                }

                TempData["Message"] = "Employee deleted successfully!";
                TempData["MessageType"] = "delete";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Failed to delete employee.";
            return View(employee);
        }
    }
}
