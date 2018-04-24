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

namespace ChemistShopSite.Controllers
{
    [LogFilter]
    [SessionFilter]
    public class ConsumptionController : Controller
    {
        public enum SortState
        {
            NameInc,
            NameDec,
            DateInc,
            DateDec,
            CountInc,
            CountDec,
            CostInc,
            CostDec
        }
        IMemoryCache _memoryCache;

        public ConsumptionController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        [ResponseCache(CacheProfileName = "Caching")]
        public IActionResult Consumption(string MedicamentName, SortState conSortOrder)
        {
            string path = Request.Path.Value.ToLower();
            Consumption conFromMemory = (Consumption)_memoryCache.Get(path);

            if (HttpContext.Session.Get("conSession") != null)
            {
                Consumption conSession = JsonConvert.DeserializeObject<Consumption>(HttpContext.Session.GetString("conSession"));
                ViewData["conSession"] = conSession;
            }

            if (HttpContext.Session.Get("conSortOrderSession") != null)
            {
                conSortOrder = JsonConvert.DeserializeObject<SortState>(HttpContext.Session.GetString("conSortOrderSession"));
            }

            using (MedicamentsContext db = new MedicamentsContext())
            {
                IQueryable<Consumption> consumptions = db.Consumptions.Include(x => x.Medicament);

                if (!String.IsNullOrEmpty(MedicamentName))
                {
                    consumptions = consumptions.Where(p => p.Medicament.MedicamentName.Contains(MedicamentName));
                }

                ViewData["NameSort"] = conSortOrder == SortState.NameInc ? SortState.NameDec : SortState.NameInc;
                ViewData["DateSort"] = conSortOrder == SortState.DateInc ? SortState.DateDec : SortState.DateInc;
                ViewData["CountSort"] = conSortOrder == SortState.CountInc ? SortState.CountDec : SortState.CountInc;
                ViewData["CostSort"] = conSortOrder == SortState.CostInc ? SortState.CostDec : SortState.CostInc;


                switch (conSortOrder)
                {
                    case SortState.NameDec:
                        consumptions = consumptions.OrderByDescending(s => s.Medicament.MedicamentName);
                        break;
                    case SortState.DateInc:
                        consumptions = consumptions.OrderBy(s => s.RealisationDate);
                        break;
                    case SortState.DateDec:
                        consumptions = consumptions.OrderByDescending(s => s.RealisationDate);
                        break;
                    case SortState.CountInc:
                        consumptions = consumptions.OrderBy(s => s.Count);
                        break;
                    case SortState.CountDec:
                        consumptions = consumptions.OrderByDescending(s => s.Count);
                        break;
                    case SortState.CostInc:
                        consumptions = consumptions.OrderBy(s => s.RealisationCost);
                        break;
                    case SortState.CostDec:
                        consumptions = consumptions.OrderByDescending(s => s.RealisationCost);
                        break;
                    default:
                        consumptions = consumptions.OrderBy(s => s.Medicament.MedicamentName);
                        break;
                }

                ViewData["consumptions"] = consumptions.ToList();
                ViewData["medicaments"] = db.Medicaments.ToList();
                ViewData["conFromMemory"] = conFromMemory;
                return View();
            }
        }

        [HttpPost]
        public IActionResult AddConsumption(Consumption consumption)
        {
            using (MedicamentsContext db = new MedicamentsContext())
            {
                db.Consumptions.Add(consumption);
                db.SaveChanges();
            }
            return RedirectToAction("Consumption");
        }

    }
}