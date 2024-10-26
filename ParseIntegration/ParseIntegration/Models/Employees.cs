using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ParseIntegration.Models
{
    public class Employees
    {
        [Key]
        [Name("Payroll_Number")]
        public string Payroll_Number { get; set; }
        [Name("Forenames")]
        public string Forenames { get; set; }
        [Name("Surname")]
        public string Surname { get; set; }
        [Name("Date_of_Birth")]
        public DateTime Date_of_Birth { get; set; }
        [Name("Telephone")]
        public string Telephone { get; set; }
        [Name("Mobile")]
        public string Mobile { get; set; }
        [Name("Address")]
        public string Address { get; set; }
        [Name("Address_2")]
        public string Address_2 { get; set; }
        [Name("Postcode")]
        public string Postcode { get; set; }
        [Name("Email_Home")]
        public string Email_Home { get; set; }
        [Name("Start_Date")]
        public DateTime Start_Date { get; set; }
    }
}
