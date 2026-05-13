using AutoMapper;
using CompanyPortal.Application.Models.Building;
using CompanyPortal.Application.Models.Department;
using CompanyPortal.Application.Models.Employee;
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

        CreateMap<EmployeeCreateViewModel, Employee>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.CreatedAt, opt => opt.Ignore())
            .ForMember(x => x.PersonalNumber, opt => opt.Ignore())
            .ForMember(x => x.IsActive, opt => opt.Ignore());

        CreateMap<Employee, EmployeeUpdateViewModel>();

        CreateMap<EmployeeUpdateViewModel, Employee>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.CreatedAt, opt => opt.Ignore())
            .ForMember(x => x.PersonalNumber, opt => opt.Ignore())
            .ForMember(x => x.IsActive, opt => opt.Ignore())
            .ForMember(x => x.ApplicationUserId, opt => opt.Ignore())
            .ForMember(x => x.PositionId, opt => opt.Ignore());

        CreateMap<Employee, EmployeePositionUpdateViewModel>()
            .ForMember(x => x.EmployeeId, opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.Positions, opt => opt.Ignore());

        CreateMap<EmployeePositionUpdateViewModel, Employee>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.PersonalNumber, opt => opt.Ignore())
            .ForMember(x => x.FirstName, opt => opt.Ignore())
            .ForMember(x => x.LastName, opt => opt.Ignore())
            .ForMember(x => x.ApplicationUserId, opt => opt.Ignore())
            .ForMember(x => x.IsActive, opt => opt.Ignore())
            .ForMember(x => x.CreatedAt, opt => opt.Ignore());

        CreateMap<Employee, EmployeeViewModel>()
            .ForMember(x => x.PositionName, opt => opt.MapFrom(x => x.Position.PositionName))
            .ForMember(x => x.Id, opt => opt.Ignore());
    }
}