using AutoMapper;
using AutoMonitoring.Domain.Entities.Implementations;
using EventMaster.BLL.DTOs.Implementations.Requests.User;
using EventMaster.BLL.DTOs.Responses.User;

namespace AutoMonitoring.BLL.Infrastructure.Mapper;

public class UserProfile:Profile
{
    public UserProfile()
    {
        // Mapping from UserDTO to User
        CreateMap<UserDTO, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        // Mapping from User to UserResponseDTO
        CreateMap<User, UserResponseDTO>();
    }
}