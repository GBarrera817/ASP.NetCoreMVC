using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreMVC.Models
{
    public class Instructor : Person
    {

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Hire Date")]
        public DateTime HireDate {  get; set; }

        // Navigation properties
        public ICollection<CourseAssignment> CourseAssignments {  get; set; }
        public OfficeAssignment OfficeAssignment { get; set; }
    }
}
