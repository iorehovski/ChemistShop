using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChemistShopSite.Models;

namespace ChemistShopSite.ViewModels
{
    public class PageViewModel
    {
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }

        public PageViewModel(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public bool HasPreviousPage {
            get {
                return (PageNumber > 1);
            }
        }

        public bool HasNextPage {
            get {
                return (PageNumber < TotalPages);
            }
        }
    }

    public class IndexViewModel
    {
        public Medicament medicament { get; set; }
        public IEnumerable<Medicament> Medicaments { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }

    public class ReceptionViewModel
    {
        public IEnumerable<Reception> Receptions { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }

    public class ConsumptionViewModel
    {
        public IEnumerable<Consumption> Consumptions { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
