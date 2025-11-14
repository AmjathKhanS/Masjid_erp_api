using AutoMapper;
using PersonalDetailsAPI.Models.DTOs;
using PersonalDetailsAPI.Models.Entities;

namespace PersonalDetailsAPI.Data;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // PersonalDetail mappings
        CreateMap<PersonalDetail, PersonalDetailDto>();
        CreateMap<CreatePersonalDetailDto, PersonalDetail>();
        CreateMap<UpdatePersonalDetailDto, PersonalDetail>();

        // WifeDetail mappings
        CreateMap<WifeDetail, WifeDetailDto>().ReverseMap();

        // ChildDetail mappings
        CreateMap<ChildDetail, ChildDetailDto>().ReverseMap();
    }
}
