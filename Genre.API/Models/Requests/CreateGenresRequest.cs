using System.ComponentModel.DataAnnotations;

namespace Genre.API.Models.Requests
{
    public class CreateGenresRequest
    {
        [Required]
        [StringLength(30, ErrorMessage = "{0} length must be between {2} and {1}."), MinLength(3)]
        public string Name { get; set; }
    }
}
