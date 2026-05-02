using AutoMapper;
using CompanyPortal.Application.Models.Building;
using CompanyPortal.Application.Models.Department;
using CompanyPortal.Domain.Entities;

namespace CompanyPortal.Application.Mapper;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Building mapping
        CreateMap<Building, BuildingDetailViewModel>();

        CreateMap<Department, DepartmentListItemViewModel>()
            .ForMember(x => x.BuildingName, opt => opt.MapFrom(x => x.Building.BuildingName));

        CreateMap<BuildingUpsertViewModel, Building>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.IsActive, opt => opt.Ignore());
    }
}