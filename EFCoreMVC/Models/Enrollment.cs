using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreMVC.Models
{
    public enum Grade
    {
        A, B, C, D, F
    }

    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }        // FK to Course 
        public int StudentID { get; set; }       // FK to Student Table

        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }        // Unknown or unassigned course
        public Course Course { get; set; }
        public Student Student { get; set; }    // Navigation Property
    }
}
