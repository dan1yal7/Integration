using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using ParseIntegration.Infrastructure;
using ParseIntegration.Models;
using System.Globalization;
using System;
using CsvHelper.Configuration;
using System.Text;

namespace ParseIntegration.Controllers
{
    public class EmployeeController : Controller
    {
        #region Fields 
        private readonly ApplicationDbContext _context;
        #endregion

        #region Ctor
        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public IActionResult Upload()
        {
            var employees = _context.Employees.OrderBy(e => e.Surname).ToList();
            return View("Upload", employees);
        }

        /// <summary>
        /// Imports employee data from a CSV file.
        /// </summary>
        /// <param name="file">The CSV file containing employee data.</param>
        /// <returns>An IActionResult indicating the outcome of the import operation.</returns>
        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file?.Length == 0)
            {
                return BadRequest("File not provided.");
            }
            var employees = new List<Employees>();
            using (var stream = file.OpenReadStream())
            using (var reader = new StreamReader(stream))
            using (var csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                DetectDelimiter = true,
                Encoding = Encoding.UTF8,
                HeaderValidated = null,
                MissingFieldFound = null
            }))
            {
                csvReader.Context.RegisterClassMap<EmployeesMap>();
                employees = csvReader.GetRecords<Employees>().ToList();
            }
            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();
            ViewBag.Message = $"{employees.Count} employees were successfully imported.";
            return View("Upload", _context.Employees.OrderBy(e => e.Surname).ToList());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(Employees updatedEmployee)
        {
            if (ModelState.IsValid)
            {
                var employee = await _context.Employees.FindAsync(updatedEmployee.Payroll_Number);
                if (employee != null)
                {
                    // Update properties
                    employee.Forenames = updatedEmployee.Forenames;
                    employee.Surname = updatedEmployee.Surname;
                    employee.Date_of_Birth = updatedEmployee.Date_of_Birth;
                    employee.Telephone = updatedEmployee.Telephone;
                    employee.Mobile = updatedEmployee.Mobile;
                    employee.Address = updatedEmployee.Address;
                    employee.Address_2 = updatedEmployee.Address_2;
                    employee.Postcode = updatedEmployee.Postcode;
                    employee.Email_Home = updatedEmployee.Email_Home;
                    employee.Start_Date = updatedEmployee.Start_Date;

                    _context.Employees.Update(employee);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Employee updated successfully!";
                }
                else
                {
                    TempData["Message"] = "Employee not found.";
                }
            }
            else
            {
                TempData["Message"] = "Invalid data.";
            }
            return RedirectToAction("Upload");
        }

        #endregion 
    }
}