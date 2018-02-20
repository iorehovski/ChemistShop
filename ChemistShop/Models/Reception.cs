using System;
using System.Collections.Generic;
using System.Text;

namespace ChemistShop.Models
{
    public class Reception
    {
        public int id { get; set; }
        public int MedicamentID { get; set; }
        public DateTime ReceiptDate { get; set; }
        public int Count { get; set; }
        public string Provider { get; set; }
        public int OrderCost { get; set; }
        public virtual Medicament Medicine { get; set; }
    }
}
