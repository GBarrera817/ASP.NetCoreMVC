using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EFCoreMVC.Models
{
    public class Student : Person
    {
        [DataType(DataType.Date)]
        [Display(Name = "Enrollment Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate {  get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }  // Navigation Property; contains all of Enrollments Entities that are related with Student Entity
    }
}
