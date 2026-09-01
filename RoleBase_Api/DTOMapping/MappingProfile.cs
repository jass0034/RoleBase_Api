using AutoMapper;
using RoleBase_Api.Models;
using RoleBase_Api.Models.DTOs;

namespace RoleBase_Api.DTOMapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User, RegisterDTO>().ReverseMap();
            CreateMap<User, UserDTO>()
               .ForMember(
                   dest => dest.RoleName,
                   opt => opt.MapFrom(src => src.Role.Name)
               )
               .ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
        }
    }
}
