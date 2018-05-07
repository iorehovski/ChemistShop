using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        MedicamentContext db;
        public HomeController(MedicamentContext context)
        {
            this.db = context;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<Medicament> Get()
        {
            var items = db.Medicaments
                //.Include(i => i.Receptions)
                //.Include(i => i.Consumptions)
                .ToList();
            return items;
        }

        // GET api/<controller>/id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = db.Medicaments
                 //.Include(i => i.Receptions)
                 //.Include(i => i.Consumptions)
                 .FirstOrDefault(i => i.Id == id);
            if (item == null)
                return NotFound();
            return new ObjectResult(item);
        }

        // POST api/<controller>
        [HttpPost]
        public IActionResult Post([FromBody]Medicament medicament)
        {
            if(medicament == null)
            {
                ModelState.AddModelError("", "Не указаны данные");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Medicaments.Add(medicament);
            db.SaveChanges();
            return Ok(medicament);
        }

        // PUT api/<controller>/id
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Medicament medicament)
        {
            if( medicament == null)
            {
                return BadRequest(ModelState);
            }
            if (!db.Medicaments.Any(i => i.Id == id))
            {
                return NotFound();
            }
            db.Update(medicament);
            db.SaveChanges();
            return Ok(medicament);
        }

        // DELETE api/<controller>/id
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Medicament medicament = db.Medicaments.FirstOrDefault(i => i.Id == id);
            if (medicament == null)
            {
                return NotFound();
            }
            db.Medicaments.Remove(medicament);
            db.SaveChanges();
            return Ok(medicament);

        }
    }
}