using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rocky.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName( "DisplayName" )]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Display Order for categor must be greater than 0")]
        public int DisplayOrder { get; set; }

    }
}
