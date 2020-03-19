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
    public class BrokersCategoriesController : Controller
    {
        private readonly InsuranceContext _context;

        public BrokersCategoriesController(InsuranceContext context)
        {
            _context = context;
        }

        // GET: BrokersCategories
        public async Task<IActionResult> Index(int? id)
        {
            ViewBag.CategoryId = id;
            ViewBag.CategoryName = _context.Categories.Find(id).Category;
            var BrokersInCategories = _context.BrokersCategories.Where(e => e.CategoryId == id).Include(e => e.Broker);
            return View(BrokersInCategories.ToList());
        }

        // GET: BrokersCategories/Details/5
        public async Task<IActionResult> Details(int? id, int? categoryId)
        {
            int BrokerId = _context.BrokersCategories.Find(id).BrokerId;
            ViewBag.BrokerName = _context.Brokers.Find(BrokerId).FullName;
            ViewBag.CategoryId = categoryId;
            if (id == null)
            {
                return NotFound();
            }
            var brokerId = _context.BrokersCategories.Find(id).BrokerId; 
            var brokersCategories = await _context.BrokersCategories
                .Include(b => b.Broker)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brokersCategories == null)
            {
                return NotFound();
            }
            ViewBag.Day = _context.Brokers.Find(brokerId).BirthDate.Day;
            ViewBag.Month = _context.Brokers.Find(brokerId).BirthDate.Month;
            ViewBag.Year = _context.Brokers.Find(brokerId).BirthDate.Year;
            ViewBag.Count = _context.BrokersCategories.Where(b => b.BrokerId == brokerId).Include(b => b.Category).Count();
            ViewBag.CategoriesList = _context.BrokersCategories.Where(b => b.BrokerId == brokerId).Include(b=> b.Category).ToList();
            return View(brokersCategories);
        }

        // GET: BrokersCategories/Create
        public IActionResult Create( int? categoryId)
        {
           // ViewBag.BrokerId = brokerId;
            ViewBag.CategoryId = categoryId;
            //var brokersCategories = await _context.BrokersCategories.FindAsync(id);
            ViewData["BrokersId"] = new SelectList(_context.Brokers, "Id", "FullName");
            ViewData["CategoriesId"] = new SelectList(_context.Categories.Where(b => b.Id == categoryId), "Id", "Category");
            return View();
        }

        // POST: BrokersCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int brokerId, int categoryId, [Bind("Id,BrokerId,CategoryId")] BrokersCategories brokersCategories)
        {
            
                _context.Add(brokersCategories);
               
                await _context.SaveChangesAsync();


            
           return  RedirectToAction("Index", "BrokersCategories", new { id = categoryId });
        }

        // GET: BrokersCategories/Edit/5
        public async Task<IActionResult> Edit(int? id, int? categoryId)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var brokersCategories = await _context.BrokersCategories.FindAsync(id);
            if (brokersCategories == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = categoryId;
           
            ViewData["BrokersId"] = new SelectList(_context.Brokers, "Id", "FullName", brokersCategories.BrokerId);
            ViewBag.BrokerId = brokersCategories.BrokerId;
            ViewData["CategoriesId"] = new SelectList(_context.Categories.Where(b=> b.Id == categoryId), "Id", "Category", brokersCategories.CategoryId);
            return View(brokersCategories);
        }

        // POST: BrokersCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int categoryId, [Bind("Id,BrokerId,CategoryId")] BrokersCategories brokersCategories)
        {
            
            if (id != brokersCategories.Id)
            {
                return NotFound();
            }
            ViewBag.CategoryId = categoryId;
            ViewBag.BrokerId = brokersCategories.BrokerId;
           
                try
                {
                   
                    _context.Update(brokersCategories);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    if (!BrokersCategoriesExists(brokersCategories.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "BrokersCategories", new { id = categoryId });

                // return RedirectToAction(nameof(Index));
            
           
            return RedirectToAction("Index", "BrokersCategories", new { id = categoryId });
        }

        // GET: BrokersCategories/Delete/5
        public async Task<IActionResult> Delete(int? categoryId , int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int brokerId = _context.BrokersCategories.Find(id).BrokerId;
            ViewBag.BrokerName = _context.Brokers.Find(brokerId).FullName;
            var brokersCategories = await _context.BrokersCategories
                .Include(b => b.Broker)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            //int categoryId = brokersCategories.CategoryId;
            ViewBag.CategoryId = categoryId;
            if (brokersCategories == null)
            {
                return NotFound();
            }

            return View(brokersCategories);
        }

        // POST: BrokersCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int categoryId, int id)
        {

            ViewBag.CategoryId = categoryId;
            var brokersCategories = await _context.BrokersCategories.FindAsync(id);
            _context.BrokersCategories.Remove(brokersCategories);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "BrokersCategories", new { id = categoryId });
        }

        private bool BrokersCategoriesExists(int id)
        {
            return _context.BrokersCategories.Any(e => e.Id == id);
        }
    }
}
