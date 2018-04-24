﻿using System;
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

        public HomeController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [ResponseCache(CacheProfileName = "Caching")]
        public IActionResult Index(string MedicamentName, SortState medSortOrder = SortState.NameInc)
        {
            string path = Request.Path.Value.ToLower();
            Medicament medicamentFromMemoryCache = (Medicament)_memoryCache.Get(path);

            List<String> storages = new List<String>(new string[] { "Минск-1", "Гомель-1", "Гродно-1", "Минск-2", "Гомель-2", "Гродно-2" });

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

                if(!String.IsNullOrEmpty(MedicamentName))
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

                ViewData["medicaments"] = medicaments.ToList();

                ViewData["medFromMemory"] = medicamentFromMemoryCache;

                return View();
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
