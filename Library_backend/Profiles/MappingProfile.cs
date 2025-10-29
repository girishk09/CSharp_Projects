using AutoMapper;
using Library_backend.DTO;
using Library_backend.Models;
using Library_backend.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Library_backend.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map AuthorDTO to Author and vice versa
            CreateMap<AuthorDTO, Author>().ReverseMap();
            CreateMap<AuthorDTO, Author>().ForMember(dest => dest.AuthorId, opt => opt.Ignore());

            // Map RegisterModel to IdentityUser
            CreateMap<RegisterModel, IdentityUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            // Map LoginModel to IdentityUser
            CreateMap<LoginModel, IdentityUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.username));
        }
    }
}
