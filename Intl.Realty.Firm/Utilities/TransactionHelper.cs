namespace Intl.Realty.Firm.Utilities
{
    public static class TransactionHelper
    {
        public static string BuildDealTransactionNumber(int transactionTypeId, int nextNumber)
        {
            string type = transactionTypeId.ToString().PadLeft(2, '0');
            string date = DateTime.Now.ToString("yyyyMMdd");
            string seq = nextNumber.ToString().PadLeft(5, '0');
            return $"{date}-{type}-{seq}";
        }
    }
}
