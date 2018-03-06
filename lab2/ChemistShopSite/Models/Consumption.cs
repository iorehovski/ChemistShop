using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChemistShopSite.Models
{
    public class Consumption
    {
        public Consumption() { }

        public Consumption(string name, string date, int count, double realisationCost)
        {
            MedicamentName = name;
            RealisationDate = date;
            Count = count;
            RealisationCost = RealisationCost;
        }

        public string MedicamentName { get; set; }
        public string RealisationDate { get; set; }
        public int Count { get; set; }
        public double RealisationCost { get; set; }
    }
}
