using Microsoft.EntityFrameworkCore;

namespace BlazorPOS.Server.Services
{
    public class SecurityAuditLog
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Action { get; set; }
        public string IpAddress { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsSuccessful { get; set; }
    }

    public interface ISecurityAuditService
    {
        Task LogLoginAttempt(string userId, string ipAddress, bool isSuccessful);
        Task LogPasswordReset(string userId, string ipAddress);
        Task<List<SecurityAuditLog>> GetRecentSecurityLogs(string userId);
    }

    public class SecurityAuditService : ISecurityAuditService
    {
        private readonly ApplicationDbContext _context;

        public SecurityAuditService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogLoginAttempt(string userId, string ipAddress, bool isSuccessful)
        {
            var log = new SecurityAuditLog
            {
                UserId = userId,
                Action = "Login",
                IpAddress = ipAddress,
                Timestamp = DateTime.UtcNow,
                IsSuccessful = isSuccessful
            };

            _context.SecurityAuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task LogPasswordReset(string userId, string ipAddress)
        {
            var log = new SecurityAuditLog
            {
                UserId = userId,
                Action = "PasswordReset",
                IpAddress = ipAddress,
                Timestamp = DateTime.UtcNow,
                IsSuccessful = true
            };

            _context.SecurityAuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SecurityAuditLog>> GetRecentSecurityLogs(string userId)
        {
            return await _context.SecurityAuditLogs
                .Where(log => log.UserId == userId)
                .OrderByDescending(log => log.Timestamp)
                .Take(10)
                .ToListAsync();
        }
    }
}