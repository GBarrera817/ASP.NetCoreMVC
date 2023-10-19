using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreMVC.Models
{
    public class OfficeAssignment
    {
        // Hay una relación de uno a cero o uno entre Instructor y las entidades OfficeAssignment. Una asignación de oficina solo existe en relación con el instructor al que se asigna y, por tanto, su clave principal también es su clave externa para la entidad Instructor. Pero Entity Framework no reconoce automáticamente InstructorID como la clave principal de esta entidad porque su nombre no sigue la convención de nomenclatura de ID o classnameID. Por tanto, se usa el atributo Key para identificarla como la clave:
        [Key]
        public int InstructorID { get; set; }

        [StringLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set; }

        public Instructor Instructor { get; set; }
    }
}