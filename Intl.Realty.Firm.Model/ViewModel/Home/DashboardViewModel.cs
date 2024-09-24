using Intl.Realty.Firm.Model.Model;

namespace Intl.Realty.Firm.Model.ViewModel.Home
{
    public class DashboardViewModel
    {
        public int SaleListingCountToday { get; set; }
        public int SaleCoopCountToday { get; set; }
        public int LeaseListingCountToday { get; set; }
        public int LeaseCoopCountToday { get; set; }

        public List<SaleListing> SaleListingListAll { get; set; } = new List<SaleListing>();
        public List<SaleListing> SaleListingListPastHour{ get; set; } = new List<SaleListing>();
        public List<SaleListing> SaleListingListToday { get; set; } = new List<SaleListing>();
        public List<SaleListing> SaleListingListMonth { get; set; } = new List<SaleListing>();
        public List<SaleCoop> SaleCoopListAll { get; set; } = new List<SaleCoop>();
        public List<SaleCoop> SaleCoopListPastHour { get; set; } = new List<SaleCoop>();
        public List<SaleCoop> SaleCoopListToday { get; set; } = new List<SaleCoop>();
        public List<SaleCoop> SaleCoopListMonth{ get; set; } = new List<SaleCoop>();
        public List<LeaseListing> LeaseListingListAll { get; set; } = new List<LeaseListing>();
        public List<LeaseListing> LeaseListingListPastHour { get; set; } = new List<LeaseListing>();
        public List<LeaseListing> LeaseListingListToday { get; set; } = new List<LeaseListing>();
        public List<LeaseListing> LeaseListingListMonth { get; set; } = new List<LeaseListing>();
        public List<LeaseCoop> LeaseCoopListAll{ get; set; } = new List<LeaseCoop>();
        public List<LeaseCoop> LeaseCoopListPastHour{ get; set; } = new List<LeaseCoop>();
        public List<LeaseCoop> LeaseCoopListToday { get; set; } = new List<LeaseCoop>();
        public List<LeaseCoop> LeaseCoopListMonth { get; set; } = new List<LeaseCoop>();
    }
}
