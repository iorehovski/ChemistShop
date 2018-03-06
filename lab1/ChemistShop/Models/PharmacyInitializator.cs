using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ChemistShop.Models
{
    public class PharmacyInitializator
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
            int countMedicamentDict = medicamentsDict.GetLength(0);
            int countМanufacturersDict = manufacturersDict.GetLength(0);
            int storages = 3;

            for (int i = 0; i < medicamentsCount; i++)
            {
                string medicamentTmp = medicamentsDict[randObj.Next(countMedicamentDict)];
                string manufacturersTmp = manufacturersDict[randObj.Next(countМanufacturersDict)];

                db.Medicaments.Add(new Medicament {
                    MedicamentName  = medicamentTmp,
                    Manufacturer = manufacturersTmp,
                    Storage  = randObj.Next(storages)
                });
            }

            //сохранение изменений в базу данных, связанную с объектом контекста
            db.SaveChanges();

            string[] providersDict = { "Главфарм", "Alliance Healthcare", "Givana-Pharm", "Медторг", "Bracco" };
            int countProvidersDict = providersDict.GetLength(0);

            int receptionsCount = 30;

            for (int i = 0; i < receptionsCount; i++)
            {
                int medicamentID = randObj.Next(1, medicamentsCount - 1);

                DateTime today = DateTime.Now.Date;
                DateTime receiptDate = today.AddDays(randObj.Next(50)-50);
                int count = randObj.Next(30) + 30;
                string providerTmp = providersDict[randObj.Next(countProvidersDict)];
                int orderCost = count * 10;     //price

                DateTime realisationDate = today.AddDays(randObj.Next(50));
                count = randObj.Next(30);
                int realisationCost = count * 10;

                db.Receptions.Add(new Reception {
                    MedicamentID = medicamentID,
                    Count = count,
                    ReceiptDate = receiptDate,
                    Provider = providerTmp,
                    OrderCost = orderCost
                });
            }

            db.SaveChanges();

            int consumptionCount = 30;

            for (int i = 0; i < consumptionCount; i++)
            {
                int medicamentID = randObj.Next(1, medicamentsCount -1);

                DateTime today = DateTime.Now.Date;

                DateTime realisationDate = today.AddDays(randObj.Next(50));
                int count = randObj.Next(30);
                int realisationCost = count * 10;

                db.Consumptions.Add(new Consumption
                {
                    MedicamentID = medicamentID,
                    RealisationDate = realisationDate,
                    Count = count,
                    RealisationCost = realisationCost
                });
            }

            db.SaveChanges();
        }
    }
}
