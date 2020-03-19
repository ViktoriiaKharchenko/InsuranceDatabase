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
    public class BrokersController : Controller
    {
        private readonly InsuranceContext _context;

        public BrokersController(InsuranceContext context)
        {
            _context = context;
        }

        // GET: Brokers
        public async Task<IActionResult> Index( )
        {
            
            return View(await _context.Brokers.ToListAsync());
        }

        // GET: Brokers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.BrokerName = _context.Brokers.Find(id).FullName;
            var brokers = await _context.Brokers
                .FirstOrDefaultAsync(m => m.Id == id);
           
            if (brokers == null)
            {
                return NotFound();
            }

            var brokerId = id;
            ViewBag.BrokerId = id;
            ViewBag.Day = _context.Brokers.Find(id).BirthDate.Day;
            ViewBag.Month = _context.Brokers.Find(id).BirthDate.Month;
            ViewBag.Year = _context.Brokers.Find(id).BirthDate.Year;
            ViewBag.Count = _context.BrokersCategories.Where(b => b.BrokerId == brokerId).Include(b => b.Category).Count();
            ViewBag.CategoriesList = _context.BrokersCategories.Where(b => b.BrokerId == brokerId).Include(b => b.Category).ToList();
            return View(brokers);
        }

        // GET: Brokers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brokers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,BirthDate,PhoneNum,Passport,Email,Password")] Brokers brokers)
        {
            
            if (ModelState.IsValid)
            {
                _context.Add(brokers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brokers);
        }

        // GET: Brokers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
           
            if (id == null)
            {
                return NotFound();
            }

            var brokers = await _context.Brokers.FindAsync(id);
            if (brokers == null)
            {
                return NotFound();
            }
            return View(brokers);
        }

        // POST: Brokers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,BirthDate,PhoneNum,Passport,Email,Password")] Brokers brokers)
        {
            
            if (id != brokers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brokers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrokersExists(brokers.Id))
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
            return View(brokers);
        }

        // GET: Brokers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.BrokerName = _context.Brokers.Find(id).FullName;
            var brokers = await _context.Brokers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brokers == null)
            {
                return NotFound();
            }

            return View(brokers);
        }

        // POST: Brokers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brokers = await _context.Brokers.FindAsync(id);
            _context.Brokers.Remove(brokers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> BrokersDocuments(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            var BrokerId = id;
            ViewBag.BrokerId = id;

            return RedirectToAction("Index", "Documents", new { brokerId = BrokerId } );
        }

        public IActionResult PhoneValid(string? PhoneNum)
        {
            if (PhoneNum != null)
            {
                if (PhoneNum.Length != 10) return Json(data: "Номер телефону занадто которкий");
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
            public IActionResult PassportValid(string? Passport, int? Id)
            {
                if (Passport != null)
                {
                    if (Passport.Length != 9) return Json(data: "Номер паспорту занадто которкий");
                    for (int i = 0; i < Passport.Length; i++)
                    {
                        if (Passport[i] < '0' || Passport[i] > '9')
                        {
                            return Json(data: "Невірний формат данних");
                        }
                    }

                    var pas = _context.Brokers.Where(b => b.Passport == Passport).Where(b => b.Id != Id);
                    if (pas.Count() > 0) { return Json(data: "Людина з таким номером паспорта вже зареєстрована в базі"); }
                
            }
               
                return Json(data: true);

            }
        public IActionResult NameValid(string? Name)
        {
            if(Name != null)
            {
                if(Name.Length < 2) { return  Json (data: "Невірний формат данних"); }
                for(int i =0; i < Name.Length; i++)
                {
                    if (Name[i] < 'А' || Name[i] > 'ї') { return Json(data: "Невірний формат данних"); }
                }
            }
            return Json(data: true);
        }
        public IActionResult SurnameValid(string? Surname)
        {
            if (Surname != null)
            {
                if (Surname.Length < 2) {return Json (data: "Невірний формат данних"); }
                for (int i = 0; i < Surname.Length; i++)
                {
                    if (Surname[i] < 'А' || Surname[i] > 'ї') { return Json(data: "Невірний формат данних"); }
                }
            }
            return Json(data: true);
        }

        public IActionResult DateValid(DateTime? BirthDate)
        {
            char[] param = { '.', '/', ':',' '};
            string birthDate = BirthDate.ToString();
            int year;
            try {  year = Convert.ToInt32(birthDate.Split(param)[2]); }
            catch (Exception e) { return Json(data: "Невірний формат данних"); }
            if (birthDate != null)
            {
                if (year < DateTime.Today.Year -65 || year > DateTime.Today.Year - 18)
                {
                    return Json(data: "Невірна дата");
                }
            }
             return Json(data: true);

        }

        private bool BrokersExists(int id)
        {
            return _context.Brokers.Any(e => e.Id == id);
        }
    }
}
