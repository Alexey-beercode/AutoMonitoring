﻿using AutoMonitoring.Domain.Entities.Interfaces;

namespace AutoMonitoring.Domain.Entities.Implementations;

public class UserRole:BaseEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}