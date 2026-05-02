using AutoMapper;
using CompanyPortal.Application.Models.Building;
using CompanyPortal.Application.Models.Department;
using CompanyPortal.Domain.Entities;

namespace CompanyPortal.Application.Mapper;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Building, BuildingDetailViewModel>();

        CreateMap<BuildingUpsertViewModel, Building>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.IsActive, opt => opt.Ignore());

        CreateMap<Building, BuildingUpsertViewModel>();

        CreateMap<Building, BuildingListItemViewModel>();

        CreateMap<Department, DepartmentListItemViewModel>()
            .ForMember(x => x.BuildingName, opt => opt.MapFrom(x => x.Building.BuildingName));
    }
}