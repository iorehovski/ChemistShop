using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChemistShopSite.Models
{
    public class Medicament
    {
        public Medicament(){}

        public Medicament(string name, string producer, string storage)
        {
            MedicamentName = name;
            Manufacturer = producer;
            Storage = storage;
        } 

        public string MedicamentName { get; set; }
        public string Manufacturer { get; set; }
        public string Storage { get; set; }
    }
}
