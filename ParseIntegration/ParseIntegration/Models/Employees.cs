using System.ComponentModel.DataAnnotations;

namespace ParseIntegration.Models
{
    public class Employees
    {
        [Key]  
        public string Payroll_Number { get; set; }
        public string Forenames { get; set; }
        public string Surname { get; set; }
        public DateTime Date_of_Birth { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Address_2 { get; set; }
        public string Postcode { get; set; }
        public string Email_Home { get; set; }
        public DateTime Start_Date { get; set; }
    }
}
