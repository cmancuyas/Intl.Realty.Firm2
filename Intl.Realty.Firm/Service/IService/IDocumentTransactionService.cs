using Intl.Realty.Firm.Model.Model;

namespace Intl.Realty.Firm.Service.IService
{
    public interface IDocumentTransactionService
    {
        List<DocumentType> GetDocumentTypesFromDocumentTypeAssignment(string transactionTypeName,
                                                                    List<DocumentTypeAssignment> documentTypeAssignments);
        List<TransactionType> GetTransactionTypesFromDocumentTypeAssignment(string documentTypeName,
                                                                            List<DocumentTypeAssignment> documentTypeAssignments);
    }
}
