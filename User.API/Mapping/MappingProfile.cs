using AutoMapper;
using User.API.Data.Entities;
using User.API.Models.Dtos;
using User.API.Models.Requests;
using User.API.Models.Responses;

namespace User.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserRequest, Users>();
            CreateMap<Users, CreateUserRequest>();

            CreateMap<Users, UserDto>();
            CreateMap<UserDto, Users>();

            CreateMap<Users, GetUserPrivatePageResponse>();
            CreateMap<GetUserPrivatePageResponse, Users>();
        }
    }
}