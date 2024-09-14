using AutoMonitoring.DAL.Infrastructure.Database;
using AutoMonitoring.DAL.Repositories.Interfaces;

namespace AutoMonitoring.DAL.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserSessionRepository _userSessionRepository;

        public UnitOfWork(ApplicationDbContext dbContext,
            IUserRepository userRepository,
            IRoleRepository roleRepository, 
            IUserSessionRepository userSessionRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userSessionRepository = userSessionRepository;
        }
        
        public IUserRepository Users => _userRepository;

        public IRoleRepository Roles => _roleRepository;

        public IUserSessionRepository UserSessions => _userSessionRepository;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken=default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        
    }
}