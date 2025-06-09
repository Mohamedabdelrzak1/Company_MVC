using Company.BLL.Interface;
using Company.BLL.Interfaces;
using Company.BLL.Repository;
using Company.DAL.Model;
using Company_MVC.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Threading.Tasks;

namespace Company_MVC.Controllers
{

   
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;


        //Ask CLR Create object From UnitOfWork
        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet] //Get : Department/index
        public async Task<IActionResult> Index()
        {
         
          var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();

            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto model)
        { 
            if (ModelState.IsValid) //Server Side Validation 
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt

                };
                var count = await _unitOfWork.DepartmentRepository.AddAsync(department);
                if(count>0)
                {
                    TempData["Message"] = "Employee created successfully!";
                    TempData["MessageType"] = "create";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details (int? Id , string  viewName = "Details")
        {
            if (Id is null) return BadRequest("Invaled Id"); //Satues code = 400 

            var department = await _unitOfWork.DepartmentRepository.GetAsync(Id.Value);

            if (department is null) return NotFound(new { SatuesCode=404 ,Message=$"Department With Id :{Id} is not found"});

            return View(viewName, department);
        }
        


        [HttpGet]
        public Task<IActionResult> Edit(int? id)
        {

            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, Department department)
        {

            
                if (id == department.Id)
                {
                    var count = await _unitOfWork.DepartmentRepository.Update(department);
                    if (count > 0)
                    {
                    TempData["Message"] = "Employee updated successfully!";
                    TempData["MessageType"] = "edit";

                    return RedirectToAction(nameof(Index));
                    }
                }

            

            return View(department);
        }


        [HttpGet]
        public Task<IActionResult> Delete(int? id)
        {
            return Details(id, "Delete");
        }


        [HttpPost]
     
        public async Task<IActionResult> Delete([FromRoute] int id, Department department)
        {

           
                if (id == department.Id)
                {
                    var count = await _unitOfWork.DepartmentRepository.Delete(department);
                    if (count > 0)
                    {
                    TempData["Message"] = "Employee deleted successfully!";
                    TempData["MessageType"] = "delete";
                    return RedirectToAction(nameof(Index));
                    }
                }

           

            return View(department);
        }

    }

}
