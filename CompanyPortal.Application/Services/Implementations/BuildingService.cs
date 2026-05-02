using AutoMapper;
using CompanyPortal.Application.Models.Building;
using CompanyPortal.Application.Services.Interfaces;
using CompanyPortal.Domain.Entities;
using CompanyPortal.Domain.RepositoryInterfaces;
using System.Reflection;

namespace CompanyPortal.Application.Services.Implementations;

public sealed class BuildingService(IUnitOfWork unitOfWork, IMapper mapper) : IBuildingService
{
    // List - dynamicsPager
    public async Task<string> ChangeStatusAsync(int id, CancellationToken ct)
    {
        var building = await unitOfWork.BuildingRepository.GetByIdAsync(id, ct);
        if (building is null)
        {
            throw new Exception("Budova nebyla nalezena.");
        }

        string term;

        if (building.IsActive)
        {
            if (building.Departments.Any(x => x.IsActive))
            {
                building.IsActive = false;
                term = "Deaktivace";
            }
            else
            {
                throw new Exception("Budovu nelze deaktivovat, má stále aktivní oddělení.");
            }
        }
        else
        {
            building.IsActive = true;
            term = "Aktivace";
        }

        await unitOfWork.SaveChangesAsync(ct);

        return term;
    }

    public async Task<BuildingDetailViewModel> GetDetailAsync(int id, CancellationToken ct)
    {
        var building = await unitOfWork.BuildingRepository.GetByIdAsync(id, ct);
        if (building is null)
        {
            throw new Exception("Budova nebyla nalezena.");
        }

        var buildingVm = mapper.Map<BuildingDetailViewModel>(building);

        return buildingVm;
    }

    public async Task CreateAsync(BuildingUpsertViewModel model, CancellationToken ct)
    {
        bool existsWithSameParametres = await unitOfWork.BuildingRepository.ExistsAnyWithSameParametersAsync(model.BuildingName, model.Code, ct);
        if (existsWithSameParametres)
        {
            throw new Exception("Název nebo kód budovy již existuje u jiné budovy, musí být unikátní.");
        }

        var entity = mapper.Map<Building>(model);
        entity.IsActive = true;

        await unitOfWork.BuildingRepository.AddAsync(entity, ct);

        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(BuildingUpsertViewModel model, CancellationToken ct)
    {
        var building = await unitOfWork.BuildingRepository.GetWithoutDependenciesAsync(model.Id, ct);
        if (building is null)
        {
            throw new Exception("Budova nebyla nalezena.");
        }

        bool existsWithSameParametres = await unitOfWork.BuildingRepository.ExistsAnyWithSameParametersAsync(model.BuildingName, model.Code, building.Id, ct);
        if (existsWithSameParametres)
        {
            throw new Exception("Název nebo kód budovy již existuje u jiné budovy, musí být unikátní.");
        }

        mapper.Map(model, building);

        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<BuildingUpsertViewModel> GetForUpdateAsync(int id, CancellationToken ct)
    {
        var building = await unitOfWork.BuildingRepository.GetWithoutDependenciesAsync(id, ct);
        if (building is null)
        {
            throw new Exception("Budova nebyla nalezena.");
        }

        var buildingVm = mapper.Map<BuildingUpsertViewModel>(building);

        return buildingVm;
    }
}
