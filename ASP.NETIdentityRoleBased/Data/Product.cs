using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NETIdentityRoleBased.Data
{
    [Table("Product")]
    public class Product
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
