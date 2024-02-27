namespace Genre.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateGenresRequest, Genres>();
            CreateMap<Genres, CreateGenresRequest>();

            CreateMap<UpdateBookGenresRequest, Genres>();
            CreateMap<Genres, UpdateBookGenresRequest>();

            CreateMap<CreateBookGenresRequest, BookGenres>();
            CreateMap<BookGenres, CreateBookGenresRequest>();

            CreateMap<UpdateBookGenresRequest, BookGenres>();
            CreateMap<BookGenres, UpdateBookGenresRequest>();
        }
    }
}
