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
    public class ReceptionController : Controller
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

        public ReceptionController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index() => RedirectToAction("/Reception");

       [ResponseCache(CacheProfileName = "Caching")]
        public IActionResult Reception(string MedicamentName, SortState recSortOrder, int page = 1)
        {
            int pageSize = 6;

            string path = Request.Path.Value.ToLower();
            Reception recFromMemoryCache = (Reception)_memoryCache.Get(path);


            if (HttpContext.Session.Get("recSession") != null)
            {
                Reception recSession = JsonConvert.DeserializeObject<Reception>(HttpContext.Session.GetString("recSession"));
                ViewData["recSession"] = recSession;
            }

            if (HttpContext.Session.Get("recSortOrderSession") != null)
            {
                recSortOrder = JsonConvert.DeserializeObject<SortState>(HttpContext.Session.GetString("recSortOrderSession"));
            }

            //if (HttpContext.Session.Get("ReceptionSession") != null)
            //{
            //    string[] recSession = HttpContext.Session.GetString("ReceptionSession").Split(";");
            //    Reception receptionSession = new Reception(
            //        Convert.ToInt32(recSession[0]),
            //        recSession[1],
            //        Convert.ToInt32(recSession[2]),
            //        Convert.ToDouble(recSession[3])
            //        );
            //    ViewData["receptionSession"] = receptionSession;
            //}

            using (MedicamentsContext db = new MedicamentsContext())
            {
                IQueryable<Reception> receptions = db.Receptions.Include(x => x.Medicine);

                int count = receptions.Count();
                receptions = receptions.Skip((page - 1) * pageSize).Take(pageSize);


                if (!String.IsNullOrEmpty(MedicamentName))
                {
                    receptions = receptions.Where(p => p.Medicine.MedicamentName.Contains(MedicamentName));
                }

                ViewData["NameSort"] = recSortOrder == SortState.NameInc ? SortState.NameDec : SortState.NameInc;
                ViewData["DateSort"] = recSortOrder == SortState.DateInc ? SortState.DateDec : SortState.DateInc;
                ViewData["CountSort"] = recSortOrder == SortState.CountInc ? SortState.CountDec : SortState.CountInc;
                ViewData["CostSort"] = recSortOrder == SortState.CostInc ? SortState.CostDec : SortState.CostInc;

                switch (recSortOrder)
                {
                    case SortState.NameDec:
                        receptions = receptions.OrderByDescending(s => s.Medicine.MedicamentName);
                        break;
                    case SortState.DateInc:
                        receptions = receptions.OrderBy(s => s.ReceiptDate);
                        break;
                    case SortState.DateDec:
                        receptions = receptions.OrderByDescending(s => s.ReceiptDate);
                        break;
                    case SortState.CountInc:
                        receptions = receptions.OrderBy(s => s.Count);
                        break;
                    case SortState.CountDec:
                        receptions = receptions.OrderByDescending(s => s.Count);
                        break;
                    case SortState.CostInc:
                        receptions = receptions.OrderBy(s => s.OrderCost);
                        break;
                    case SortState.CostDec:
                        receptions = receptions.OrderByDescending(s => s.OrderCost);
                        break;
                    default:
                        receptions = receptions.OrderBy(s => s.Medicine.MedicamentName);
                        break;
                }

                PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
                ReceptionViewModel viewModel = new ReceptionViewModel
                {
                    PageViewModel = pageViewModel,
                    Receptions = receptions.ToList()
                };

                //ViewData["receptions"] = receptions.ToList();
                ViewData["medicaments"] = db.Medicaments.ToList();
                ViewData["recFromMemory"] = recFromMemoryCache;
                return View(viewModel);
            }
        }

        [HttpPost]
        public IActionResult AddReception(Reception reception)
        {
            using (MedicamentsContext db = new MedicamentsContext())
            {
                db.Receptions.Add(reception);
                db.SaveChanges();

                //string recToSession = reception.MedicamentID + ";" + reception.ReceiptDate + ";" + reception.Count + ";" + reception.OrderCost;
                //HttpContext.Session.SetString("ReceptionSession", recToSession);
            }

            return RedirectToAction("Reception");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            using (MedicamentsContext db = new MedicamentsContext())
            {
                var rec = await db.Receptions.FindAsync(Convert.ToInt32(id));

                if (rec != null)
                {

                    db.Receptions.Remove(rec);
                    await db.SaveChangesAsync();
                }
            }

            return RedirectToAction("Reception");
        }

        public async Task<IActionResult> Edit(string id)
        {
            Reception model;
            using (MedicamentsContext db = new MedicamentsContext())
            {
                model = await db.Receptions.FindAsync(Convert.ToInt32(id));
                if (model == null)
                {
                    return NotFound();
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Reception model)
        {
            if (ModelState.IsValid)
            {
                using (MedicamentsContext db = new MedicamentsContext())
                {
                    Reception rec = await db.Receptions.FindAsync(Convert.ToInt32(model.Id));
                    if (rec != null)
                    {
                        rec.ReceiptDate = model.ReceiptDate;
                        rec.Count = model.Count;
                        rec.OrderCost = model.OrderCost;

                        db.Receptions.Update(rec);
                        await db.SaveChangesAsync();
                    }
                }
            }
            return RedirectToAction("Reception");
        }
    }
}