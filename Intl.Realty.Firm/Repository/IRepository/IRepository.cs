using Intl.Realty.Firm.Model.Pagination;
using System.Linq.Expressions;

namespace Intl.Realty.Firm.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(
                    Expression<Func<T, bool>>? filter = null,
                    string? includeProperties = null,
                    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!,
                    bool tracked = false,
                    int pageSize = 0,
                    int pageNumber = 1);

        // Merging two tables with a category
        public IQueryable<TResult> MergeWith<TEntity2, TKey, TResult>(
            IQueryable<T> table1,
            IQueryable<TEntity2> table2,
            Expression<Func<T, TKey>> keySelector1,
            Expression<Func<TEntity2, TKey>> keySelector2,
            Expression<Func<T, TEntity2, TResult>> resultSelector)
            where TEntity2 : class
        {
            return table1.Join(table2, keySelector1, keySelector2, resultSelector);
        }

        Task<T> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entity);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entity);
        Task UpdateRangeAsync(IEnumerable<T> entity);
        Task SaveChangesAsync();
        Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class;
        Task UpdateAsync<TEntity>(TEntity entity, params Expression<Func<TEntity, object>>[] updatedProperties) where TEntity : class;
        Task<IQueryable<T>> AsQueryableAsync(Expression<Func<T, bool>>? filter = null,
                                            string? includeProperties = null,
                                            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!,
                                            bool tracked = false,
                                            int pageSize = 0,
                                            int pageNumber = 1);
        IQueryable<T> AsQueryable(Expression<Func<T, bool>>? filter = null,
                                    string? includeProperties = null,
                                    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!,
                                    bool tracked = false,
                                    int pageSize = 0,
                                    int pageNumber = 1);
        Task<int> CountAsync();
        int Count();
    }
}
