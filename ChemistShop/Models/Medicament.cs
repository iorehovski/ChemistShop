using System;
using System.Collections.Generic;
using System.Text;

namespace ChemistShop.Models
{
    public class Medicament
    {
        public int MedicamentID { get; set; }
        public string MedicamentName { get; set; }
        public string Manufacturer { get; set; }
        public int Storage { get; set; }
        public virtual ICollection<Reception> Receptions { get; set; }
        public virtual ICollection<Consumption> Consumptions { get; set; }
    }
}
