using AutoMapper;
using Chapter.API.Data.Entities;
using Chapter.API.Models.Responses;

namespace Chapter.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GetChaptersTitlesByBookIdResponse, Chapters>();
            CreateMap<Chapters, GetChaptersTitlesByBookIdResponse>();
        }
    }
}