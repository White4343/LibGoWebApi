using AutoMapper;
using Book.API.Data.Entities;
using Book.API.Models.Dtos;
using Book.API.Models.Requests.BooksRequests;
using Book.API.Models.Requests.ReadersRequests;
using Book.API.Models.Responses.ReadersResponses;

namespace Book.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateBooksRequest, Books>();
            CreateMap<Books, CreateBooksRequest>();

            CreateMap<UpdateBooksRequest, Books>();
            CreateMap<Books, UpdateBooksRequest>();

            CreateMap<Comments, CommentsDto>();
            CreateMap<CommentsDto, Comments>();

            CreateMap<Books, BooksDto>();
            CreateMap<BooksDto, Books>();

            CreateMap<CreateReadersRequest, Readers>();
            CreateMap<Readers, CreateReadersRequest>();

            CreateMap<UpdateReadersRequest, Readers>();
            CreateMap<Readers, UpdateReadersRequest>();

            CreateMap<Readers,ReadersDto>();
            CreateMap<ReadersDto, Readers>();

            CreateMap<BookGenres, BookGenresDto>();
            CreateMap<BookGenresDto, BookGenres>();
        }
    }
}