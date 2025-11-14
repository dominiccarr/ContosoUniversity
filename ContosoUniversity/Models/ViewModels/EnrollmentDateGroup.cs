using System;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models.ViewModels
{
    public class EnrollmentDateGroup
    {
        [DataType(DataType.Date)]
        public DateOnly? EnrollmentDate { get; set; }

        public int StudentCount { get; set; }
    }
}