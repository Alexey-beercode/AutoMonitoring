﻿using AutoMonitoring.Domain.Entities.Interfaces;

namespace AutoMonitoring.Domain.Entities.Implementations;

public class User:BaseEntity
{
    public string Login { get; set; }
    public string Password { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime BlockedUntil { get; set; }
}