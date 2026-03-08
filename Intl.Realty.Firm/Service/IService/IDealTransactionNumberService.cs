namespace Intl.Realty.Firm.Service.IService
{
    public interface IDealTransactionNumberService
    {
        /// <summary>
        /// Safely gets the next transaction number for the specified year.
        /// </summary>
        /// <param name="year">The year for which to get the next transaction number.</param>
        /// <returns>The new incremented transaction number.</returns>
        Task<int> GetNextTransactionNumberAsync(int year);
    }
}
