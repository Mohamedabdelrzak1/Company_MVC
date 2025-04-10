using Company.BLL.Interface;
using Company.BLL.Repository;
using Company.DAL.Model;
using Company_MVC.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace Company_MVC.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository; //NULL

        //Ask CLR Create object From IDepartmentRepository
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }


        [HttpGet] //Get : Department/index
        public IActionResult Index()
        {
         
          var departments =  _departmentRepository.GetAll();

            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        { 
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentDto model)
        { 
            if (ModelState.IsValid) //Server Side Validation 
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt

                };
                var count = _departmentRepository.Add(department);
                if(count>0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details (int? id , string  viewName = "Details")
        {
            if (id is null) return BadRequest("Invaled Id"); //Satues code = 400 

            var department =  _departmentRepository.Get(id.Value);

            if (department is null) return NotFound(new { SatuesCode=404 ,Message=$"Department With Id :{id} is not found"});

            return View(viewName, department);
        }
        


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null) return BadRequest("Invaled Id"); //Satues code = 400 

            var department = _departmentRepository.Get(id.Value);

            if (department is null) return NotFound(new { SatuesCode = 404, Message = $"Department With Id :{id} is not found" });

            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Department department)
        {

            if (ModelState.IsValid) //Server Side Validation 
            {
                if (id == department.Id)
                {
                    var count = _departmentRepository.Update(department);
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

            }

            return View(department);
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest("Invaled Id"); //Satues code = 400 

            var department = _departmentRepository.Get(id.Value);

            if (department is null) return NotFound(new { SatuesCode = 404, Message = $"Department With Id :{id} is not found" });

            return Details(id, "Delete");
        }


        [HttpPost]
     
        public IActionResult Delete([FromRoute] int id, Department department)
        {

            if (ModelState.IsValid) //Server Side Validation 
            {
                if (id == department.Id)
                {
                    var count = _departmentRepository.Delete(department);
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

            }

            return View(department);
        }

    }

}
