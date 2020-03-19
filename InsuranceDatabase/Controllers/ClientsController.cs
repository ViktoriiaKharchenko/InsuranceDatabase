using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InsuranceDatabase;

namespace InsuranceDatabase.Controllers
{
    public class ClientsController : Controller
    {
        private readonly InsuranceContext _context;

        public ClientsController(InsuranceContext context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clients.ToListAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            ViewBag.ClientName = _context.Clients.Find(id).FullName;
            ViewBag.ClientId = id;
            ViewBag.Day = _context.Clients.Find(id).BirthDate.Day;
            ViewBag.Month = _context.Clients.Find(id).BirthDate.Month;
            ViewBag.Year = _context.Clients.Find(id).BirthDate.Year;
            var clients = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clients == null)
            {
                return NotFound();
            }

            return View(clients);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,BirthDate,Passport,PhoneNum,Email")] Clients clients)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clients);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clients);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var clients = await _context.Clients.FindAsync(id);
            if (clients == null)
            {
                return NotFound();
            }
            return View(clients);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,BirthDate,Passport,PhoneNum,Email")] Clients clients)
        {
            
            if (id != clients.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clients);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientsExists(clients.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(clients);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.ClientName = _context.Clients.Find(id).FullName;
            var clients = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clients == null)
            {
                return NotFound();
            }

            return View(clients);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clients = await _context.Clients.FindAsync(id);
            _context.Clients.Remove(clients);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult PhoneValid(string? PhoneNum)
        {
            if (PhoneNum != null)
            {
                if(PhoneNum.Length != 10) return Json(data: "Номер телефону занадто которкий");
                for (int i = 0; i < PhoneNum.Length; i++)
                {
                    if (PhoneNum[0] != '0' || PhoneNum[i] < '0' || PhoneNum[i] > '9')
                    {
                        return Json(data: "Невірний формат данних");
                    }
                }
            }
            return Json(data: true);

        }
        public IActionResult PassportValid(string? Passport, int?Id)
        {
            if (Passport != null)
            {
                if (Passport.Length != 9) return Json(data: "Номер паспорту занадто которкий");
                for (int i = 0; i < Passport.Length; i++)
                {
                    if ( Passport[i] < '0' || Passport[i] > '9')
                    {
                        return Json(data: "Невірний формат данних");
                    }
                }

                var pas = _context.Clients.Where(b => b.Passport == Passport).Where(b => b.Id != Id);
                if (pas.Count() > 0) { return Json(data: "Людина з таким номером паспорта вже зареєстрована в базі"); }

            }
            return Json(data: true);

        }
        public IActionResult DateValid(DateTime? BirthDate)
        {
            char[] param = { '.', '/', ':', ' ' };
            string birthDate = BirthDate.ToString();
            int year,day, month;
            try 
            { 
                year = Convert.ToInt32(birthDate.Split(param)[2]);
                month = Convert.ToInt32(birthDate.Split(param)[1]);
                day = Convert.ToInt32(birthDate.Split(param)[0]);
            }
            catch (Exception e) { return Json(data: "Невірний формат данних"); }
            if (birthDate != null)
            {
                if (year < DateTime.Today.Year -150 || year > DateTime.Today.Year - 18|| (year ==DateTime.Today.Year 
                    && month > DateTime.Today.Month) ||( year == DateTime.Today.Year && month ==DateTime.Today.Month
                    && day> DateTime.Today.Day))
                {
                    return Json(data: "Невірна дата");
                }
            }
            return Json(data: true);

        }
        public async Task<IActionResult> ClientsDocuments(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ClientId = id;
            ViewBag.ClientId = id;

            return RedirectToAction("Index", "Documents", new { clientId = ClientId });
        }

        private bool ClientsExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
