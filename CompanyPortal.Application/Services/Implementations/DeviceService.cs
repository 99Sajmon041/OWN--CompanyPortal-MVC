using AutoMapper;
using CompanyPortal.Application.Services.Interfaces;
using CompanyPortal.Domain.RepositoryInterfaces;

namespace CompanyPortal.Application.Services.Implementations;

public sealed class DeviceService(IUnitOfWork unitOfWork, IMapper mapper) : IDeviceService
{

}
