using System.ComponentModel.DataAnnotations;

namespace Book.API.Models.Requests.CommentsRequests
{
    public class UpdateCommentsRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int BookId { get; set; }
    }
}
