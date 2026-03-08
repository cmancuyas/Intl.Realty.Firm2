using Intl.Realty.Firm.Repository.IRepository;
using Intl.Realty.Firm.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace Intl.Realty.Firm.Service
{
    public class DealTransactionNumberService : IDealTransactionNumberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int MaxRetries = 3;

        public DealTransactionNumberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> GetNextTransactionNumberAsync(int year)
        {
            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                var numberRange = await _unitOfWork.DealTransactionNumberRange
                    .GetAsync(x => !x.IsDeleted && x.Year == year);

                if (numberRange == null)
                    throw new InvalidOperationException($"No DealTransactionNumberRange found for year {year}.");

                numberRange.CurrentNumber += 1;

                try
                {
                    await _unitOfWork.DealTransactionNumberRange.UpdateAsync(numberRange);
                    await _unitOfWork.SaveChangesAsync();
                    return numberRange.CurrentNumber;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (attempt == MaxRetries)
                        throw new InvalidOperationException("Could not generate a unique transaction number due to high concurrency. Please try again.");
                    await Task.Delay(50);
                }
            }
            throw new InvalidOperationException("Failed to generate a unique transaction number.");
        }
    }

}
