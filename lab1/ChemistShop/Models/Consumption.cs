using System;
using System.Collections.Generic;
using System.Text;

namespace ChemistShop.Models
{
    public class Consumption
    {
        public int id { get; set; }
        public int MedicamentID { get; set; }
        public DateTime RealisationDate { get; set; }
        public int Count { get; set; }
        public int RealisationCost { get; set; }
        public virtual Medicament Medicine { get; set; }
    }
}
