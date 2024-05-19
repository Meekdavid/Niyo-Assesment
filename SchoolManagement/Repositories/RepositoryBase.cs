using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Helpers.DBContext;
using SchoolManagement.Helpers.SignalR;
using SchoolManagement.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog;

namespace SchoolManagement.Repositories
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected readonly SchoolDbContext _context;
        private readonly IHubContext<SchoolHub> _hubContext;
        //private readonly ILogger<RepositoryBase<T>> _logger;

        public RepositoryBase(SchoolDbContext context, IHubContext<SchoolHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
           // _logger = logger;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            Log.Information($"Fetching all {typeof(T).Name}s.");
            var entities = await _context.Set<T>().ToListAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"Fetched all {typeof(T).Name}s.", entities);
            return entities;
        }

        public async Task<T> GetByIdAsync(string id)
        {
            Log.Information($"Fetching {typeof(T).Name} with ID {id}.");
            var entity = await _context.Set<T>().FindAsync(id);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"{typeof(T).Name} with ID {id} fetched.", entity);
            return entity;
        }

        public async Task<bool> AddAsync(T entity)
        {
            Log.Information($"Adding {typeof(T).Name}.");
            await _context.Set<T>().AddAsync(entity);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"{typeof(T).Name} added.", entity);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            Log.Information($"Updating {typeof(T).Name}.");
            _context.Set<T>().Update(entity);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"{typeof(T).Name} updated.", entity);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            Log.Information($"Deleting {typeof(T).Name} with ID {id}.");
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"{typeof(T).Name} with ID {id} deleted.", entity);
                    return true;
                }
            }
            return false;
        }
    }
}
