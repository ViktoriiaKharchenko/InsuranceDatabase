using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InsuranceDatabase;
using Microsoft.AspNetCore.Http;
using System.IO;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;

namespace InsuranceDatabase.Controllers
{
    
    public class CategoriesController : Controller
    {
        private readonly InsuranceContext _context;

        public CategoriesController(InsuranceContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.CategoryName = _context.Categories.Find(id).Category;
            if (id == null)
            {
                return NotFound();
            }
           
            var categories = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categories == null)
            {
                return NotFound();
            }

            //return View(categories);
            return RedirectToAction("Index", "Types", new { id = categories.Id, category = categories.Category });
        }

        // GET: Categories/Create
        [Authorize(Policy = "RequireBrokerRole")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Category")] Categories categories)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categories);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categories);
        }

        // GET: Categories/Edit/5
        [Authorize(Policy = "RequireBrokerRole")]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.CategoryName = _context.Categories.Find(id).Category;
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories.FindAsync(id);
            if (categories == null)
            {
                return NotFound();
            }
            return View(categories);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("Id,Category")] Categories categories)
        {
            if (id != categories.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categories);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriesExists(categories.Id))
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
            return View(categories);
        }

        // GET: Categories/Delete/5
        [Authorize(Policy = "RequireBrokerRole")]
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.CategoryName = _context.Categories.Find(id).Category;
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categories == null)
            {
                return NotFound();
            }

            return View(categories);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categories = await _context.Categories.FindAsync(id);
            var types =  _context.Types.Where(b => b.CategoryId == id).ToList();
            var docs = _context.Documents.Where(b => b.Type.CategoryId == id).ToList();
            foreach(var doc in docs)
            {
                _context.Documents.Remove(doc);
            }
            foreach (var type in types)
            {
                _context.Types.Remove(type);
            }
            _context.Categories.Remove(categories);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult CatValid(string? Category, int? Id)
        {
            if (Category.Length < 2) return Json(data: "Назва категорії занадто коротка");
            var cat = _context.Categories.Where(b => b.Category == Category).Where(b => b.Id != Id);
            if (cat.Count() > 0) { return Json(data: "Така категорія вже є в базі");
            }
            return Json(data: true);
        }
    private bool CategoriesExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
