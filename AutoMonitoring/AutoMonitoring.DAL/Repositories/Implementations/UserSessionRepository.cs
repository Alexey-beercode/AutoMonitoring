﻿using AutoMonitoring.DAL.Infrastructure.Database;
using AutoMonitoring.DAL.Repositories.Interfaces;
using AutoMonitoring.Domain.Entities.Implementations;
using Microsoft.EntityFrameworkCore;

namespace AutoMonitoring.DAL.Repositories.Implementations;

public class UserSessionRepository:BaseRepository<UserSession>,IUserSessionRepository
{
    private readonly ApplicationDbContext _dbContext;
    public UserSessionRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserSession> GetActiveSessionByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserSessions
            .FirstOrDefaultAsync(s => s.UserId == userId && s.IsActive && !s.IsDeleted);
    }

    public async Task<IEnumerable<UserSession>> GetAllSessionsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserSessions.Where(us => !us.IsDeleted).ToListAsync(cancellationToken);
    }

    public void Update(UserSession userSession)
    {
        _dbContext.UserSessions.Update(userSession);
    }

    public async Task<UserSession> GetSessionByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserSessions.FirstOrDefaultAsync(us => us.RefreshToken == refreshToken && !us.IsDeleted,
            cancellationToken);
    }

    public void DeleteSessionByUserId(Guid userId)
    {
        var sessionsToDelete = _dbContext.UserSessions
            .Where(us => us.UserId == userId && !us.IsDeleted);

        foreach (var session in sessionsToDelete)
        {
            session.IsDeleted = true;
            _dbContext.UserSessions.Update(session);
        }
    }

    public async Task<UserSession> GetLastSessionByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.UserSessions
            .Where(us => us.UserId == userId)
            .OrderByDescending(us => us.LastActive) 
            .FirstOrDefaultAsync(cancellationToken); 
    }
}