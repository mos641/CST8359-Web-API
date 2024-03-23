using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace Lab6.Models
{
    public class Student
    {
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SwaggerSchema(ReadOnly = true)]
        [Required]
        public Guid ID { get; set; }

        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 1)]
        [Required]
        public String FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 1)]
        [Required]
        public String LastName { get; set; }

        [Display(Name = "Program")]
        [StringLength(5, MinimumLength = 1)]
        public String Program { get; set; }
    }
}
