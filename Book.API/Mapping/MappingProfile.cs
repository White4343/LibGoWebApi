using AutoMapper;
using Book.API.Data.Entities;
using Book.API.Models.Dtos;
using Book.API.Models.Requests;

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
        }
    }
}