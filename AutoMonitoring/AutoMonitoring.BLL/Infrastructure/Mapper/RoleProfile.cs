using AutoMapper;
using AutoMonitoring.Domain.Entities.Implementations;
using EventMaster.BLL.DTOs.Responses.Role;

namespace AutoMonitoring.BLL.Infrastructure.Mapper;

public class RoleProfile:Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleDTO>();
    }
}