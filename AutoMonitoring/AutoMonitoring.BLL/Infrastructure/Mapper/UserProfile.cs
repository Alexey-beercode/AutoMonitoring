using AutoMapper;
using AutoMonitoring.BLL.DTOs.Implementations.Requests.User;
using AutoMonitoring.BLL.DTOs.Implementations.Responses.User;
using AutoMonitoring.Domain.Entities.Implementations;

namespace AutoMonitoring.BLL.Infrastructure.Mapper;

public class UserProfile:Profile
{
    public UserProfile()
    {
        CreateMap<LoginDTO, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
        
        CreateMap<User, UserResponseDTO>();
    }
}