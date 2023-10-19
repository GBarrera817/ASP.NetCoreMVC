using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EFCoreMVC.Models
{
    public class Student
    {
        public int ID { get; set; }

        [Required, StringLength(50, MinimumLength=2), Display(Name = "Last Name"), RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        public string LastName {  get; set; }

        [Required, StringLength(50, MinimumLength=2), Display(Name = "First Name"), Column("FirstName"), RegularExpression(@"^[A-Z]+[a-zA-Z]*$")]
        public string FirstMidName { get; set; }

        [DataType(DataType.Date), Display(Name = "Enrollment Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate {  get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get => $"{LastName}, {FirstMidName}";
        }

        public ICollection<Enrollment> Enrollments { get; set; }  // Navigation Property; contains all of Enrollments Entities that are related with Student Entity
    }
}
