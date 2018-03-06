using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ChemistShop.Models;
using System.Collections;

namespace ChemistShop
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (MedicamentsContext db = new MedicamentsContext())
            {
                Console.WriteLine("== Инициализация ==");
                PharmacyInitializator.Initialize(db);
                Select(db);

                Console.WriteLine("== Выборка основных таблиц ==");
                Console.ReadKey();
                SelectStandart(db);

                Console.WriteLine("== Вставка в таблицы: Medicaments & Reception ==");

                Console.ReadKey();
                Insert(db);

                Console.WriteLine("== Выборка основных таблиц ==");

                Console.ReadKey();
                SelectStandart(db);

                Console.WriteLine("== Удаление из таблиц: Medicaments & Reception ==");

                Console.ReadKey();
                Delete(db);

                Console.WriteLine("== Выборка основных таблиц ==");

                SelectStandart(db);

                Console.WriteLine("== Обновление ==");

                Update(db);
                SelectStandart(db);

                Console.ReadKey();
            }
        }

        static void Print(string sqltext, IEnumerable items)
        {
            Console.WriteLine(sqltext);
            Console.WriteLine("Записи: ");
            foreach (var item in items)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine();
        }

        static void SelectStandart(MedicamentsContext db)
        {
            // Определение LINQ запроса 1
            var queryLINQ1 = from f in db.Medicaments
                             select new
                             {
                                 Medicament_ID = f.MedicamentID,
                                 Medicament_Name = f.MedicamentName,
                                 Manufacturer_Name = f.Manufacturer,
                                 Storage_Num = f.Storage
                             };
            string comment = "1. Запрос на выборку всех доступных медикаментов в аптеке : \n";
            Print(comment, queryLINQ1.ToList());

            // Определение LINQ запроса 2
            var queryLINQ2 = from f in db.Receptions
                             join t in db.Medicaments
                             on f.MedicamentID equals t.MedicamentID
                             orderby f.ReceiptDate
                             select new
                             {
                                 id = f.id,
                                 MedicamentName = t.MedicamentName,
                                 Receipt_Date = f.ReceiptDate,
                                 Provider = f.Provider,
                                 OrderCost = f.OrderCost
                             };
            comment = "2. Запрос на выборку поступлений лекарств в аптеке(первые 10): \n";
            Print(comment, queryLINQ2.Take(10).ToList());

            // Определение LINQ запроса 3
            var queryLINQ3 = from f in db.Consumptions
                             join t in db.Medicaments
                             on f.MedicamentID equals t.MedicamentID
                             orderby f.RealisationDate
                             select new
                             {
                                 id = f.id,
                                 MedicamentName = t.MedicamentName,
                                 RealisationDate = f.RealisationDate,
                                 RealisationCost = f.RealisationCost
                             };
            comment = "3. Запрос на выборку реализаций лекарств в аптеке(первые 10): \n";
            Print(comment, queryLINQ3.Take(10).ToList());
        }

        static void Select(MedicamentsContext db)
        {
            // Определение LINQ запроса 1
            var queryLINQ1 = from f in db.Medicaments
                             where f.Manufacturer == "Angelini(Италия)"
                             select new
                             {
                                 Medicament_ID = f.MedicamentID,
                                 Medicament_Name = f.MedicamentName,
                                 Manufacturer_Name = f.Manufacturer,
                                 Storage_Num = f.Storage
                             };
            string comment = "1. Запрос на выборку лекарств в аптеке, по производителю Angelini(Италия): \n";
            Print(comment, queryLINQ1   .ToList());

            // Определение LINQ запроса 2
            var queryLINQ2 = from f in db.Receptions
                             join t in db.Medicaments
                             on f.MedicamentID equals t.MedicamentID
                             where (f.ReceiptDate.ToShortDateString() == "03.01.2018")
                             select new
                             {
                                 id = f.id,
                                 MedicamentName = t.MedicamentName,
                                 Receipt_Date = f.ReceiptDate,
                                 Provider = f.Provider,
                                 OrderCost = f.OrderCost
                             };
            comment = "2. Запрос на выборку лекарств в аптеке, на дату 03.01.2018: \n";
            Print(comment, queryLINQ2.ToList());

            // Определение LINQ запроса 3
            var queryLINQ3 = from f in db.Receptions
                             join t in db.Medicaments
                             on f.MedicamentID equals t.MedicamentID
                             where (f.ReceiptDate >= new DateTime(2018, 01, 03) && 
                                    f.ReceiptDate <= new DateTime(2018, 02, 03))
                             select new
                             {
                                 id = f.id,
                                 MedicamentName = t.MedicamentName,
                                 Receipt_Date = f.ReceiptDate,
                                 Provider = f.Provider,
                                 OrderCost = f.OrderCost
                             };
            comment = "3. Запрос на выборку лекарств в аптеке, на даты между 03.01.2018 и 03.02.2018: \n";
            Print(comment, queryLINQ3.ToList());

            // Определение LINQ запроса 4
            var queryLINQ4 = from f in db.Consumptions
                             join t in db.Medicaments
                             on f.MedicamentID equals t.MedicamentID
                             group f.RealisationCost by f.RealisationDate into gr
                             select new
                             {
                                Date = gr.Key.ToShortDateString(),
                                RealizationCost = gr.Sum()
                             };
            comment = "4. Запрос на выборку реализации лекарств в аптеке, на заданные даты: \n";
            Print(comment, queryLINQ4.ToList());

            // Определение LINQ запроса 5
            var queryLINQ5 = from f in db.Consumptions
                             join t in db.Medicaments
                             on f.MedicamentID equals t.MedicamentID
                             group f.Count by f.RealisationDate into gr
                             select new
                             {
                                 Date = gr.Key.ToShortDateString(),
                                 RealizationCost = gr.Sum()
                             };
            comment = "5. Запрос на выборку количества лекарств, реализованных в аптеке, на заданные даты: \n";
            Print(comment, queryLINQ5.ToList());

            // Определение LINQ запроса 6
            var queryLINQ6 = from f in db.Medicaments
                             where f.MedicamentName == "Омез"
                             select new
                             {
                                 Medicament_ID = f.MedicamentID,
                                 Medicament_Name = f.MedicamentName,
                                 Manufacturer_Name = f.Manufacturer,
                                 Storage_Num = f.Storage
                             };
            comment = "6. Запрос на выборку медикаментов, с названием 'Омез': \n";
            Print(comment, queryLINQ6.ToList());

        }

        static void Insert(MedicamentsContext db)
        {
            Medicament medicament = new Medicament
            {
                MedicamentName = "Фервекс",
                Manufacturer = "Angelini(Италия)",
                Storage = 2
            };
            db.Medicaments.Add(medicament);       

            db.SaveChanges();

            Reception reception = new Reception
            {
                MedicamentID = medicament.MedicamentID,
                ReceiptDate = new DateTime(2018, 01, 02),
                Count = 15,
                Provider = "Главфарм",
                OrderCost = 150
            };
            db.Receptions.Add(reception);

            db.SaveChanges();
        }

        static void Delete(MedicamentsContext db)
        {
            var medicaments = db.Medicaments
                .Where(t => t.MedicamentName == "Фервекс");
            var receptions = db.Receptions
                .Where(t => (t.Medicine.MedicamentName == "Фервекс"));

            db.Receptions.RemoveRange(receptions);
            db.SaveChanges();

            db.Medicaments.RemoveRange(medicaments);
            db.SaveChanges();

        }

        static void Update(MedicamentsContext db)
        {
            var medicaments = db.Medicaments.Where(t => t.MedicamentName == "Омез");
            if (medicaments != null)
            {
                foreach(var i in medicaments)
                {
                    i.Storage = 1;  
                };
            }

            db.SaveChanges();
        }
    }
}
