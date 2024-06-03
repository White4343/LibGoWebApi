using System.ComponentModel.DataAnnotations;

namespace Book.API.Models.Requests.GenresRequests
{
    public class CreateGenreRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
