using AutoMapper;
using AutoMonitoring.BLL.DTOs.Implementations.Responses.Role;
using AutoMonitoring.Domain.Entities.Implementations;

namespace AutoMonitoring.BLL.Infrastructure.Mapper;

public class RoleProfile:Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleDTO>();
    }
}