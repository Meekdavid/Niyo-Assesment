using Microsoft.EntityFrameworkCore;
using SchoolManagement.Helpers.DBContext;
using SchoolManagement.Interfaces;

namespace SchoolManagement.Repositories
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected readonly SchoolDbContext _context;

        public RepositoryBase(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<bool> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
    }

}
