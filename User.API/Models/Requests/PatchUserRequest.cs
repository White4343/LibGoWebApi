using System.ComponentModel.DataAnnotations;

namespace User.API.Models.Requests
{
    public record PatchableFields
    {
        public string Email { get; init; }
        public string Nickname { get; init; }
        public string Description { get; init; }
        public string PhotoUrl { get; init; }
    }
    
    public class PatchUserRequest
    {
        [Required]
        public int Id { get; set; }

        public string FieldToPatch { get; set; }

        public string ValueToPatch { get; set; }
    }
}