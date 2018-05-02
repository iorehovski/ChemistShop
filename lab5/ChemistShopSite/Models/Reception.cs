using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChemistShopSite.Models
{
    public class Reception
    {
        public Reception() { }

        public Reception(int id, string date, int count, double orderCost)
        {
            MedicamentID = id;
            ReceiptDate = date;
            Count = count;
            OrderCost = orderCost;
        }

        public int Id { get; set; }
        public int MedicamentID { get; set; }
        public string ReceiptDate { get; set; }
        public int Count { get; set; }
        public double OrderCost { get; set; }
        public virtual Medicament Medicine { get; set; }
    }
}
