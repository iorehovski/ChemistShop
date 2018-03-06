using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChemistShopSite.Models
{
    public class Reception
    {
        public Reception() { }

        public Reception(string name, string date, int count, double orderCost)
        {
            MedicamentName = name;
            ReceiptDate = date;
            Count = count;
            OrderCost = orderCost;
        }

        public string MedicamentName { get; set; }
        public string ReceiptDate { get; set; }
        public int Count { get; set; }
        public double OrderCost { get; set; }
    }
}
