using Book.API.Data.Entities;
using Book.API.Models.Dtos;

namespace Book.API.Models.Responses.ReadersResponses
{
    public class GetReadersBookByUserId
    {
        public int UserId { get; set; }
        
        public IEnumerable<ReadersDto> Readers { get; set; }
        
        public IEnumerable<BooksDto> Books { get; set; }
    }
}
