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

namespace ChemistShopSite.Controllers
{
    public class HomeController : Controller
    {
        IMemoryCache _memoryCache;

        public HomeController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [ResponseCache(CacheProfileName = "Caching")]
        public IActionResult Index()
        {
            string path = Request.Path.Value.ToLower();
            Medicament medicamentFromMemoryCache = (Medicament)_memoryCache.Get(path);

            List<String> storages = new List<String>(new string[] { "Минск-1", "Гомель-1", "Гродно-1", "Минск-2", "Гомель-2", "Гродно-2" });

            ViewData["storages"] = storages;

            if (Request.Cookies["FormMedicament"] != null)
            {
                Medicament medFromCookie = JsonConvert.DeserializeObject<Medicament>(Request.Cookies["FormMedicament"].ToString());
                ViewData["medFromCookie"] = medFromCookie;
            }

            using (MedicamentsContext db = new MedicamentsContext())
            {
                //PharmacyInitializer.Initialize(db);

                ViewData["medicaments"] = db.Medicaments.ToList();
                ViewData["medFromMemory"] = medicamentFromMemoryCache;
                return View();
            }

        }

        [ResponseCache(CacheProfileName = "Caching")]
        public IActionResult Reception()
        {
            string path = Request.Path.Value.ToLower();
            Reception recFromMemoryCache = (Reception)_memoryCache.Get(path);

            if(HttpContext.Session.Get("ReceptionSession") != null)
            {
                string[] recSession = HttpContext.Session.GetString("ReceptionSession").Split(";");
                Reception receptionSession = new Reception(
                    Convert.ToInt32( recSession[0]),
                    recSession[1],
                    Convert.ToInt32(recSession[2]),
                    Convert.ToDouble(recSession[3])
                    );
                ViewData["receptionSession"] = receptionSession;
            }


            using (MedicamentsContext db = new MedicamentsContext())
            {
                ViewData["receptions"] = db.Receptions.ToList();
                ViewData["medicaments"] = db.Medicaments.ToList();
                ViewData["recFromMemory"] = recFromMemoryCache;
                return View();
            }
        }

        [ResponseCache(CacheProfileName = "Caching")]
        public IActionResult Consumption()
        {
            string path = Request.Path.Value.ToLower();
            Consumption conFromMemory = (Consumption)_memoryCache.Get(path);

            using (MedicamentsContext db = new MedicamentsContext())
            {
                ViewData["consumptions"] = db.Consumptions.ToList();
                ViewData["medicaments"] = db.Medicaments.ToList();
                ViewData["conFromMemory"] = conFromMemory;
                return View();
            }
        }

        [HttpPost]
        public IActionResult AddMedicament(Medicament medicament)
        {
            CookieOptions cookie = new CookieOptions();
            cookie.Expires = DateTime.Now.AddMinutes(1);

            if(Request.Cookies["FormMedicament"] == null)
            {
                string value = JsonConvert.SerializeObject(medicament);
                //string value = medicament.MedicamentName + ';' + medicament.Manufacturer + ';' + medicament.Storage;
                Response.Cookies.Append("FormMedicament", value);
            }

            using (MedicamentsContext db = new MedicamentsContext())
            {
                db.Medicaments.Add(medicament);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult AddReception(Reception reception)
        {
            using (MedicamentsContext db = new MedicamentsContext())
            {
                db.Receptions.Add(reception);
                db.SaveChanges();

                string recToSession = reception.MedicamentID + ";" + reception.ReceiptDate + ";"  + reception.Count + ";"+ reception.OrderCost;
                HttpContext.Session.SetString("ReceptionSession", recToSession);
            }

            return RedirectToAction("Reception");
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
