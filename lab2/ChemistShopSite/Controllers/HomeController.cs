using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChemistShopSite.Models;

namespace ChemistShopSite.Controllers
{
    public class HomeController : Controller
    {
        static List<Medicament> medicaments = new List<Medicament>();
        static List<Reception> receptions = new List<Reception>();
        static List<Consumption> consumptions = new List<Consumption>();

        public IActionResult Index()
        {   
            List<String> storages = new List<String>(new string[] { "Gomel", "Minsk", "Grodno" });

            ViewData["storages"] = storages;
            ViewData["medicaments"] = medicaments;
            return View();
        }

        public IActionResult Reception()
        {
            ViewData["receptions"] = receptions;

            return View();
        }
        
        public IActionResult Consumption()
        {
            ViewData["consumptions"] = consumptions;

            return View();
        }

        [HttpPost]
        public IActionResult AddMedicament(Medicament medicament, string storage)
        {
            medicaments.Insert(0, new Medicament(medicament.MedicamentName, medicament.Manufacturer, storage));
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult AddReception(Reception reception)
        {
            receptions.Insert(0, reception);
            return RedirectToAction("Reception");
        }
        
        [HttpPost]
        public IActionResult AddConsumption(Consumption consumption)
        {
            consumptions.Insert(0, consumption);
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
