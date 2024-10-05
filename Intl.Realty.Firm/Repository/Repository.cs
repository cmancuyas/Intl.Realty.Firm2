using Intl.Realty.Firm.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Intl.Realty.Firm.DataAccess;
using Intl.Realty.Firm.Model.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Intl.Realty.Firm.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext context)
        {

            this.dbSet = context.Set<T>();
            //_db.Categories == dbSet
            _context = context;
        }
        public async Task AddAsync(T entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
        }
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            _context.AddRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked)
            {
                query = dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }

            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.FirstOrDefaultAsync() ?? query.FirstOrDefault()!;

        }
        public async Task<IEnumerable<T>>
                GetAllAsync(Expression<Func<T, bool>>? filter = null,
                            string? includeProperties = null,
                            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!,
                            bool tracked = false,
                            int pageSize = 0, int pageNumber = 1)
        {
            IQueryable<T> query = dbSet;

            if (tracked)
            {
                query = dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                }
                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return await query.ToListAsync();
        }
        public Task<PaginatedList<T>> GetPagedAsync(
            Expression<Func<T, bool>>? filter = null, 
            string? includeProperties = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            bool tracked = false, 
            int pageSize = 0, 
            int pageIndex = 1)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Include the properties specified
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            // Apply ordering if provided
            if (orderBy != null)
            {
                query = orderBy(query);
            }


            var paginatedList = PaginatedList<T>.CreateAsync(query, pageIndex, pageSize);
            // Return a paginated list
            return paginatedList;
        }

        public async Task UpdateAsync(T entity)
        {
            //_context.Attach(entity);
            //_context.Entry(entity).State = EntityState.Modified;
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _context.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _context.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task<IQueryable<T>> AsQueryableAsync(Expression<Func<T, bool>>? filter = null, string?
                                                    includeProperties = null,
                                                    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!,
                                                    bool tracked = false,
                                                    int pageSize = 0, int pageNumber = 1)
        {
            IQueryable<T> query = dbSet;
            if (tracked)
            {
                query = dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                }
                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }
            if (orderBy != null)
            {
                query = orderBy(query).AsQueryable();
            }
            return Task.FromResult(query);
        }

        public IQueryable<T> AsQueryable(Expression<Func<T, bool>>? filter = null, string?
                                            includeProperties = null,
                                            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!,
                                            bool tracked = false,
                                            int pageSize = 0, int pageNumber = 1)
        {
            IQueryable<T> query = dbSet;

            if (tracked)
            {
                query = dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                }
                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }
            if (orderBy != null)
            {
                query = orderBy(query).AsQueryable();
            }
            return query;
        }

        public Task<int> CountAsync()
        {
            var count = dbSet.CountAsync();
            return count; ;
        }

        public int Count()
        {
            return dbSet.Count();
        }


    }
}
