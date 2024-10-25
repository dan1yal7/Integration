using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using ParseIntegration.Infrastructure;
using ParseIntegration.Models;
using System.Globalization;
using System;

namespace ParseIntegration.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context; 

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Upload()
        {
            var employees = _context.Employees.OrderBy(e => e.Surname).ToList();
            return View("Upload", employees);
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "Please select a CSV file.";
                return View("Upload", _context.Employees.ToList());
            }

            var employees = new List<Employees>();

            using (var stream = file.OpenReadStream())
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                employees = csv.GetRecords<Employees>().ToList();
            }
                
            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();

            ViewBag.Message = $"{employees.Count} employees were successfully imported.";
            return View("Upload", _context.Employees.OrderBy(e => e.Surname).ToList());
        }
    }
}
