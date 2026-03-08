using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Service.IService;

namespace Intl.Realty.Firm.Service
{
    public class DocumentTransactionService : IDocumentTransactionService
    {
        public List<DocumentType> GetDocumentTypesFromDocumentTypeAssignment(string transactionTypeName,
                                                                            List<DocumentTypeAssignment> documentTypeAssignments)
        {
            var documentTypes = documentTypeAssignments
                .Where(x => x.TransactionType != null && x.TransactionType.Description == transactionTypeName && x.IsActive && !x.IsDeleted)
                .Select(x => x.DocumentType)
                .Where(dt => dt != null && dt.IsActive && !dt.IsDeleted) // Safety check in case DocumentType is null
                .Distinct()
                .ToList();

            return documentTypes!;
        }

        public List<TransactionType> GetTransactionTypesFromDocumentTypeAssignment(string documentTypeName,
                                                                                   List<DocumentTypeAssignment> documentTypeAssignments)
        {
            var transactionTypes = documentTypeAssignments
                .Where(x => x.DocumentType != null && x.DocumentType.Description == documentTypeName && x.IsActive && !x.IsDeleted)
                .Select(x => x.TransactionType)
                .Where(tt => tt != null && tt.IsActive && !tt.IsDeleted) // Safety check if TransactionType is null
                .Distinct()
                .ToList();

            return transactionTypes!;
        }

    }
}