using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParseIntegration.Controllers;
using ParseIntegration.Infrastructure;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using ParseIntegration.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace ParseIntegraionUnitTest.Controllers
{
    public class EmployeeControllerTest
    {
        [Fact]
        public void UploadTheEmployeeList()
        {

            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var mockContext = new ApplicationDbContext(options))
            {
                mockContext.Employees.AddRange(GetTestEmployees());
                mockContext.SaveChanges();

                var controller = new EmployeeController(mockContext);
                // Act 
                var result = controller.Upload();

                //Assert  
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Employees>>(viewResult.Model);
                Assert.Equal(GetTestEmployees().Count, model.Count());
            }
        }
        private List<Employees> GetTestEmployees()
        {
            var employees = new List<Employees>()
            {
                new Employees
                {
                  Payroll_Number = "EMP001",
                  Forenames = "John",
                  Surname = "Doe",
                  Date_of_Birth = new DateTime(1980, 1, 1),
                  Telephone = "1234567890",
                  Mobile = "0987654321",
                  Address = "123 Main St",
                  Address_2 = "Apt 4",
                  Postcode = "12345",
                  Email_Home = "john.doe@example.com",
                  Start_Date = new DateTime(2020, 1, 15)
                },

                new Employees
                {
                  Payroll_Number = "EMP002",
                  Forenames = "Jane",
                  Surname = "Smith",
                  Date_of_Birth = new DateTime(1990, 5, 10),
                  Telephone = "5555555555",
                  Mobile = "4444444444",
                  Address = "456 Elm St",
                  Address_2 = "Suite B",
                  Postcode = "54321",
                  Email_Home = "jane.smith@example.com",
                  Start_Date = new DateTime(2021, 6, 1)
                }

            };
            return employees;
        }
    }

}
