using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChemistShopSite.Models
{
    public class Medicament
    {
        public Medicament(){
            Receptions = new List<Reception>();
            Consumptions = new List<Consumption>();
        }

        public Medicament(string name, string producer, string storage)
        {
            MedicamentName = name;
            Manufacturer = producer;
            Storage = storage;
        }

        public override string ToString()
        {
            return MedicamentName + ";" + Manufacturer + ";" + Storage;
        }

        public int Id { get; set; }
        public string MedicamentName { get; set; }
        public string Manufacturer { get; set; }
        public string Storage { get; set; }
        public virtual ICollection<Reception> Receptions { get; set; }
        public virtual ICollection<Consumption> Consumptions { get; set; }
    }
}
