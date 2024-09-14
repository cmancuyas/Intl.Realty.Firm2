namespace Intl.Realty.Firm.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IDepartmentRepository Department { get; }
        IEmploymentStatusRepository EmploymentStatus { get; }
        void SaveAsync();
        void Save();
    }
}
