﻿using AutoMonitoring.BLL.Factories.Interfaces;
using AutoMonitoring.Domain.Entities.Implementations;

namespace AutoMonitoring.BLL.Factories.Implementations;

public class UserSessionFactory:IUserSessionFactory
{
    public UserSession Create(Guid userId, string deviceName, string refreshToken,DateTime refreshTokenExpireTime)
    {
        return new UserSession
        {
            UserId = userId,
            DeviceName = deviceName,
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = refreshTokenExpireTime,
            LastActive = DateTime.UtcNow,
            IsActive = true
        };
    }
}