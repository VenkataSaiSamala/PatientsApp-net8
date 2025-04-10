using API.Dtos;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(d => d.Age, o => o.MapFrom(x => x.DateOfBirth.CalculateAge()))
                .ForMember(p => p.PhotoUrl, o =>
                    o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMail)!.Url));
            CreateMap<Photo, PhotoDto>();
        }
    }
}
