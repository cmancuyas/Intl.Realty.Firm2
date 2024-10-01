using Intl.Realty.Firm.Model.Model;
using Intl.Realty.Firm.Model.Pagination;
using Intl.Realty.Firm.Model.ViewModel.AllDealsVM;

namespace Intl.Realty.Firm.Model.ViewModel.Home
{
    public class DashboardViewModel
    {
        public int SaleListingCountToday { get; set; }
        public int SaleCoopCountToday { get; set; }
        public int LeaseListingCountToday { get; set; }
        public int LeaseCoopCountToday { get; set; }

        public List<SaleListing> SaleListingListPast1Min { get; set; } = new List<SaleListing>();
        public List<SaleListing> SaleListingListPast5Mins { get; set; } = new List<SaleListing>();
        public List<SaleListing> SaleListingListPastHour { get; set; } = new List<SaleListing>();
        public List<SaleListing> SaleListingListToday { get; set; } = new List<SaleListing>();

        public List<SaleCoop> SaleCoopListPast1Min { get; set; } = new List<SaleCoop>();
        public List<SaleCoop> SaleCoopListPast5Mins { get; set; } = new List<SaleCoop>();
        public List<SaleCoop> SaleCoopListPastHour { get; set; } = new List<SaleCoop>();
        public List<SaleCoop> SaleCoopListToday { get; set; } = new List<SaleCoop>();

        public List<LeaseListing> LeaseListingListPast1Min { get; set; } = new List<LeaseListing>();
        public List<LeaseListing> LeaseListingListPast5Mins { get; set; } = new List<LeaseListing>();
        public List<LeaseListing> LeaseListingListPastHour { get; set; } = new List<LeaseListing>();
        public List<LeaseListing> LeaseListingListToday { get; set; } = new List<LeaseListing>();

        public List<LeaseCoop> LeaseCoopListPast1Min { get; set; } = new List<LeaseCoop>();
        public List<LeaseCoop> LeaseCoopListPast5Mins { get; set; } = new List<LeaseCoop>();
        public List<LeaseCoop> LeaseCoopListPastHour { get; set; } = new List<LeaseCoop>();
        public List<LeaseCoop> LeaseCoopListToday { get; set; } = new List<LeaseCoop>();

        public List<AllDealViewModel>? AllDealsPast1Min { get; set; }
        public List<AllDealViewModel>? AllDealsPast5Mins { get; set; }
        public List<AllDealViewModel>? AllDealsPastHour { get; set; }
        public List<AllDealViewModel>? AllDealsToday { get; set; }

        public PaginatedList<AllDealViewModel>? AllDeals { get; set; } // For Deals data
        public int AllDealsTotalDeals { get; set; } // Total number of deals
        public string CurrentSort { get; set; } // The current sort order
        public string DealTransactionNumberSort { get; set; } // Sort order for DealTransactionNumber
        public string TransactionTypeNameSort { get; set; } // Sort order for TransactionTypeName
        public string PropertyAddressSort { get; set; } // Sort order for PropertyAddress
        public string DealStatusNameSort { get; set; } // Sort order for PropertyAddress
        public string CreatedByFullNameSort { get; set; } // Sort order for PropertyAddress
        public string CreatedAtSort { get; set; } // Sort order for PropertyAddress
        public string FinalClosingDateSort { get; set; } // Sort order for PropertyAddress
        public string CurrentFilter { get; set; } // The current filter (e.g., search string)
        public int? PageSize { get; set; } // Page size for pagination
        public int CurrentPage { get; set; } // Current page number
        public int TotalPages { get; set; } // Total number of pages

    }
}
