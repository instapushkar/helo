using helo.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using helo.Data;
using Microsoft.AspNetCore.Authorization;

namespace helo.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AddDbcontext _context;
        public EmployeeController(AddDbcontext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()  //create the view of it
        {
            var employees = await _context.Employees.ToListAsync();
            return View(employees);
        }


        [HttpGet]
        public IActionResult Add() //create the view of it
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel model)
        {
            //if (ModelState.IsValid)
            //{
            var employee = new Employee()
            {
                id = Guid.NewGuid(),
                Name = model.Name,
                DateofBirth = model.DateofBirth,
                Email = model.Email,
                Department = model.Department
            };
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
            //}
            //return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.id == id);
            if (employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    id = employee.id,
                    Name = employee.Name,
                    DateofBirth = employee.DateofBirth,
                    Email = employee.Email,
                    Department = employee.Department
                };
               
                return await Task.Run(() => View("View", viewModel));
            }

            return RedirectToAction("Index");
        }
       
        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await _context.Employees.FindAsync(model.id);
            if (employee != null)
            {
                employee.Name = model.Name;
                employee.DateofBirth = model.DateofBirth;
                employee.Email = model.Email;
                employee.Department = model.Department;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await _context.Employees.FindAsync(model.id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
