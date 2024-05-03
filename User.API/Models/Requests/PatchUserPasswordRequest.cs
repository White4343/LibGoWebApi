using System.ComponentModel.DataAnnotations;

namespace User.API.Models.Requests
{
    public class PatchUserPasswordRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string NewPasswordRepeat { get; set; }
    }
}