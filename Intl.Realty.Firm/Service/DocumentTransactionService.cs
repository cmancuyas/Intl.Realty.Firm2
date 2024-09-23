using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Service.IService;

namespace Intl.Realty.Firm.Service
{
    public class DocumentTransactionService : IDocumentTransactionService
    {
        public List<DocumentType> GetDocumentTypesFromDocumentTypeAssignment(string transactionTypeName,
                                                                            List<DocumentTypeAssignment> documentTypeAssignments,
                                                                            List<DocumentType> documentTypes)
        {
            List<DocumentType> documentTypeList = new List<DocumentType>();

            var documentTypeAssignmentList = documentTypeAssignments.Where(x => x.TransactionType!.Description == transactionTypeName).ToList();

            var filteredDocumentTypes = documentTypeAssignmentList?
                                    .GroupBy(x => x.DocumentType)
                                    .Select(grp => new DocumentType
                                    {
                                        Id = grp.Key!.Id,
                                        Code = grp.Key.Code,
                                        Description = grp.Key.Description,
                                        IsActive = grp.Key.IsActive,
                                        IsRequired = grp.Key.IsRequired,
                                        IsDeleted = grp.Key.IsDeleted,
                                        CreatedAt = grp.Key.CreatedAt,
                                        CreatedBy = grp.Key.CreatedBy,
                                        UpdatedAt = grp.Key.UpdatedAt,
                                        UpdatedBy = grp.Key.UpdatedBy,
                                        DeletedAt = grp.Key.DeletedAt,
                                        DeletedBy = grp.Key.DeletedBy,
                                    }).ToList();
            if (filteredDocumentTypes != null)
            {
                var documentTypeIEnum = documentTypes.Where(x => filteredDocumentTypes.Any(docTypes => x.Id == docTypes.Id)).ToList();
                documentTypeList = documentTypeIEnum.ToList();
            }
            return documentTypeList;
        }

        public List<TransactionType> GetTransactionTypesFromDocumentTypeAssignment(string documentTypeName,
                                                                                    List<DocumentTypeAssignment> documentTypeAssignments,
                                                                                    List<TransactionType> transactionTypes)
        {
            List<TransactionType> transactionTypeList = new List<TransactionType>();

            var documentTypeAssignmentList = documentTypeAssignments.Where(x => x.DocumentType!.Description == documentTypeName).ToList();

            var filteredTransactionTypes = documentTypeAssignmentList?
                                    .GroupBy(x => x.TransactionType)
                                    .Select(grp => new TransactionType
                                    {
                                        Id = grp.Key!.Id,
                                        Code = grp.Key.Code,
                                        Description = grp.Key.Description,
                                        IsActive = grp.Key.IsActive,
                                        IsDeleted = grp.Key.IsDeleted,
                                        CreatedAt = grp.Key.CreatedAt,
                                        CreatedBy = grp.Key.CreatedBy,
                                        UpdatedAt = grp.Key.UpdatedAt,
                                        UpdatedBy = grp.Key.UpdatedBy,
                                        DeletedAt = grp.Key.DeletedAt,
                                        DeletedBy = grp.Key.DeletedBy,
                                    }).ToList();
            if (filteredTransactionTypes != null)
            {
                var transactionTypeIEnum = transactionTypes.Where(x => filteredTransactionTypes.Any(transactionTypes => x.Id == transactionTypes.Id)).ToList();
                transactionTypeList = transactionTypeIEnum.ToList();
            }
            return transactionTypeList;
        }
    }
}
