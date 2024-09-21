using Intl.Realty.Firm.Model.Model;

namespace Intl.Realty.Firm.Repository.IRepository
{
    public interface ITransactionTypeRepository : IRepository<TransactionType>
    {
        Task<TransactionType> GetByNameAsync(string name);
    }
}
