namespace Chapter.API.Models.Responses
{
    public class GetChaptersTitlesByBookIdResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsFree { get; set; }
    }
}
