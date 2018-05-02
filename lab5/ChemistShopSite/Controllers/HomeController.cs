using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ChemistShopSite.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using ChemistShopSite.Filters;
using ChemistShopSite.ViewModels;

namespace ChemistShopSite.Controllers
{
    [LogFilter]
    [SessionFilter]
    public class HomeController : Controller
    {
        public enum SortState
        {
            NameInc,
            NameDec,
            ProducerInc,
            ProducerDec,   
            StorageInc,
            StorageDec 
        }

        IMemoryCache _memoryCache;
        List<String> storages = new List<String>(new string[] { "Минск-1", "Гомель-1", "Гродно-1", "Минск-2", "Гомель-2", "Гродно-2" });

        public HomeController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [ResponseCache(CacheProfileName = "Caching")]
        public IActionResult Index(string MedicamentName, SortState medSortOrder = SortState.NameInc, int page = 1)
        {
            int pageSize = 6;

            string path = Request.Path.Value.ToLower();
            Medicament medicamentFromMemoryCache = (Medicament)_memoryCache.Get(path);

            ViewData["storages"] = storages;

            if (HttpContext.Session.Get("medSession") != null)
            {
                Medicament medSession = JsonConvert.DeserializeObject<Medicament>(HttpContext.Session.GetString("medSession"));
                ViewData["medSession"] = medSession;
            }

            if (HttpContext.Session.Get("medSortOrderSession") != null)
            {
                medSortOrder = JsonConvert.DeserializeObject<SortState>(HttpContext.Session.GetString("medSortOrderSession"));
            }

            using (MedicamentsContext db = new MedicamentsContext())
            {
                //PharmacyInitializer.Initialize(db);

                IQueryable<Medicament> medicaments = db.Medicaments;

                int count = medicaments.Count();
                medicaments = medicaments.Skip((page - 1) * pageSize).Take(pageSize);


                if (!String.IsNullOrEmpty(MedicamentName))
                {
                    medicaments = medicaments.Where(p => p.MedicamentName.Contains(MedicamentName));
                }

                ViewData["NameSort"] = medSortOrder == SortState.NameInc ? SortState.NameDec : SortState.NameInc;
                ViewData["ProducerSort"] = medSortOrder == SortState.ProducerInc ? SortState.ProducerDec : SortState.ProducerInc;
                ViewData["StorageSort"] = medSortOrder == SortState.StorageInc ? SortState.StorageDec : SortState.StorageInc;

                switch (medSortOrder)
                {
                    case SortState.NameDec:
                        medicaments = medicaments.OrderByDescending(s => s.MedicamentName);
                        break;
                    case SortState.ProducerInc:
                        medicaments = medicaments.OrderBy(s => s.Manufacturer);
                        break;
                    case SortState.ProducerDec:
                        medicaments = medicaments.OrderByDescending(s => s.Manufacturer);
                        break;
                    case SortState.StorageInc:
                        medicaments = medicaments.OrderBy(s => s.Storage);
                        break;
                    case SortState.StorageDec:
                        medicaments = medicaments.OrderByDescending(s => s.Storage);
                        break;
                    default:
                        medicaments = medicaments.OrderBy(s => s.MedicamentName);
                        break;
                }

                PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
                IndexViewModel viewModel = new IndexViewModel
                {
                    PageViewModel = pageViewModel,
                    Medicaments = medicaments.ToList()
                };

                //ViewData["medicaments"] = medicaments.ToList();

                ViewData["medFromMemory"] = medicamentFromMemoryCache;

                return View(viewModel);
            }

        }

        [HttpPost]
        public IActionResult AddMedicament(Medicament medicament)
        {
            //CookieOptions cookie = new CookieOptions();
            //cookie.Expires = DateTime.Now.AddMinutes(1);

            //if(Request.Cookies["FormMedicament"] == null)
            //{
            //    string value = JsonConvert.SerializeObject(medicament);
            //    //string value = medicament.MedicamentName + ';' + medicament.Manufacturer + ';' + medicament.Storage;
            //    Response.Cookies.Append("FormMedicament", value);
            //}

            using (MedicamentsContext db = new MedicamentsContext())
            {
                db.Medicaments.Add(medicament);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            using (MedicamentsContext db = new MedicamentsContext())
            {
                var med = await db.Medicaments.FindAsync(Convert.ToInt32(id));

                var consumptions = db.Consumptions.Where(c => c.MedicamentID == med.Id);
                var receptions = db.Receptions.Where(c => c.MedicamentID == med.Id);

                if (med != null)
                {
                    foreach(Reception i in receptions)
                    {
                        db.Receptions.Remove(i);
                    }
                    foreach (Consumption i in consumptions)
                    {
                        db.Consumptions.Remove(i);
                    }

                    db.Medicaments.Remove(med);
                    await db.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            Medicament model;
            using (MedicamentsContext db = new MedicamentsContext())
            {
                model = await db.Medicaments.FindAsync(Convert.ToInt32(id));
                if (model == null)
                {
                    return NotFound();
                }
            }
            ViewData["storages"] = storages;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Medicament model)
        {
            if (ModelState.IsValid)
            {
                using (MedicamentsContext db = new MedicamentsContext())
                {
                    Medicament med = await db.Medicaments.FindAsync(Convert.ToInt32(model.Id));
                    if(med != null)
                    {
                        med.MedicamentName = model.MedicamentName;
                        med.Manufacturer = model.Manufacturer;
                        med.Storage = model.Storage;

                        db.Medicaments.Update(med);
                        await db.SaveChangesAsync();
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public String GetCurrentDate()
        {
            return DateTime.Today.ToShortDateString();
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
