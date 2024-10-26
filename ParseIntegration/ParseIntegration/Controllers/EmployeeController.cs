﻿using CsvHelper;
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
            return await TryExecute(() => Upload(file));
        }


        /// <summary>
        /// Processes the uploaded CSV file, reads employee data, and saves it to the database.
        /// </summary>
        /// <param name="file">The uploaded CSV file containing employee data.</param>
        /// <returns>An IActionResult that displays the "Upload" view with a list of employees, and a message indicating the import status.</returns>
        private async Task<IActionResult> Upload(IFormFile file)
        {
           if (file == null || file.Length == 0)
           {
              ViewBag.Message = "Please select a CSV file.";
              return View("Upload", _context.Employees.ToList());
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

        /// <summary>
        /// Executes a specified asynchronous function and catches any exceptions, displaying the error message in the view.
        /// </summary>
        /// <param name="func">The function to execute.</param>
        /// <returns>An IActionResult indicating the result of the function or an error message.</returns>
        private async Task<IActionResult> TryExecute(Func<Task<IActionResult>> func)
         {
            try
            {
                return await func();
            }

            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return default;
            }
         }
        #endregion 
    }
}
