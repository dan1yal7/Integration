﻿using CsvHelper.Configuration;

namespace ParseIntegration.Models
{
    public class EmployeesMap : ClassMap<Employees>
    { 
        public EmployeesMap()
        {
            Map(m => m.Payroll_Number).Name("Personnel_Records.Payroll_Number");
            Map(m => m.Forenames).Name("Personnel_Records.Forenames");
            Map(m => m.Surname).Name("Personnel_Records.Surname");
            // Specify multiple date formats to handle both `dd/MM/yyyy` and `d/M/yyyy`
            Map(m => m.Date_of_Birth)
                .Name("Personnel_Records.Date_of_Birth")
                .TypeConverterOption.Format("dd/MM/yyyy", "d/M/yyyy");

            Map(m => m.Telephone).Name("Personnel_Records.Telephone");
            Map(m => m.Mobile).Name("Personnel_Records.Mobile");
            Map(m => m.Address).Name("Personnel_Records.Address");
            Map(m => m.Address_2).Name("Personnel_Records.Address_2");
            Map(m => m.Postcode).Name("Personnel_Records.Postcode");
            Map(m => m.Email_Home).Name("Personnel_Records.EMail_Home");
            Map(m => m.Start_Date)
                .Name("Personnel_Records.Start_Date")
                .TypeConverterOption.Format("dd/MM/yyyy", "d/M/yyyy");
        }
    }
}
