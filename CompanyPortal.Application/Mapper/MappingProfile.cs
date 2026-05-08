using AutoMapper;
using CompanyPortal.Application.Models.Building;
using CompanyPortal.Application.Models.Department;
using CompanyPortal.Application.Models.Position;
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

        CreateMap<DepartmentUpsertViewModel, Department>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.IsActive, opt => opt.Ignore());

        CreateMap<Department, DepartmentDetailViewModel>()
            .ForMember(x => x.BuildingName, opt => opt.MapFrom(x => x.Building.BuildingName));  

        CreateMap<Position, PositionListItemViewModel>()
            .ForMember(x => x.DepartmentName, opt => opt.MapFrom(x => x.Department.DepartmentName));

        CreateMap<Position, PositionDetailViewModel>()
            .ForMember(x => x.DepartmentName, opt => opt.MapFrom(x => x.Department.DepartmentName));

        CreateMap<Position, PositionUpsertViewModel>();

        CreateMap<PositionUpsertViewModel, Position>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.IsActive, opt => opt.Ignore());
    }
}