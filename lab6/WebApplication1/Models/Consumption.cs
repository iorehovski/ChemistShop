using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Consumption
    {
        public Consumption() { }

        public Consumption(int id, string date, int count, double realisationCost)
        {
            MedicamentID = id;
            RealisationDate = date;
            Count = count;
            RealisationCost = RealisationCost;
        }

        public int Id { get; set; }
        public int MedicamentID { get; set; }
        public string RealisationDate { get; set; }
        public int Count { get; set; }
        public double RealisationCost { get; set; }
        public virtual Medicament Medicament { get; set; }
    }
}
