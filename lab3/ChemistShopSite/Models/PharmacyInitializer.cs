using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChemistShopSite.Models
{
    public class PharmacyInitializer
    {
        public static void Initialize(MedicamentsContext db)
        {
            db.Database.EnsureCreated();

            if (db.Medicaments.Any())
            {
                return;
            }

            int medicamentsCount = 10;

            Random randObj = new Random(1);

            string[] medicamentsDict = { "Фервекс", "Омез", "Азитромицин", "Ибупрофен", "Асперин" };
            string[] manufacturersDict = { "Angelini(Италия)", "Bayer AG(Германия)", "BELUPO(Хорватия)", "Celgene(США)", "KRKA(Словения)", "Sopharma AD(Болгария)" };
            string[] storages = { "Минск-1", "Гомель-1", "Гродно-1", "Минск-2", "Гомель-2", "Гродно-2" };
            int countMedicamentDict = medicamentsDict.GetLength(0);
            int countМanufacturersDict = manufacturersDict.GetLength(0);

            for (int i = 0; i < medicamentsCount; i++)
            {
                string medicamentTmp = medicamentsDict[randObj.Next(countMedicamentDict)];
                string manufacturersTmp = manufacturersDict[randObj.Next(countМanufacturersDict)];

                db.Medicaments.Add(new Medicament
                {
                    MedicamentName = medicamentTmp,
                    Manufacturer = manufacturersTmp,
                    Storage = storages[randObj.Next(storages.Length)]
                });
            }

            //сохранение изменений в базу данных, связанную с объектом контекста
            db.SaveChanges();

        
            int receptionsCount = 30;

            for (int i = 0; i < receptionsCount; i++)
            {
                int medicamentID = randObj.Next(1, medicamentsCount - 1);

                DateTime today = DateTime.Now.Date;
                DateTime receiptDate = today.AddDays(randObj.Next(50) - 50);
                int count = randObj.Next(30) + 30;
                int orderCost = count * 10;     //price

                DateTime realisationDate = today.AddDays(randObj.Next(50));
                count = randObj.Next(30);
                int realisationCost = count * 10;

                db.Receptions.Add(new Reception
                {
                    MedicamentID = medicamentID,
                    Count = count,
                    ReceiptDate = receiptDate.ToLongDateString(),
                    OrderCost = orderCost
                });
            }

            db.SaveChanges();

            int consumptionCount = 30;

            for (int i = 0; i < consumptionCount; i++)
            {
                int medicamentID = randObj.Next(1, medicamentsCount - 1);

                DateTime today = DateTime.Now.Date;

                DateTime realisationDate = today.AddDays(randObj.Next(50));
                int count = randObj.Next(30);
                int realisationCost = count * 10;

                db.Consumptions.Add(new Consumption
                {
                    MedicamentID = medicamentID,
                    RealisationDate = realisationDate.ToLongDateString(),
                    Count = count,
                    RealisationCost = realisationCost
                });
            }

            db.SaveChanges();
        }
    }
}
