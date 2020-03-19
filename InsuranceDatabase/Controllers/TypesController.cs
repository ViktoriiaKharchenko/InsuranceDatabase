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
    public class TypesController : Controller
    {
        private readonly InsuranceContext _context;

        public TypesController(InsuranceContext context)
        {
            _context = context;
        }

        // GET: Types
        public async Task<IActionResult> Index(int? id, string? category)
        {
            if (id == null) return RedirectToAction("Categories", "Index");

            ViewBag.CategoryId = id;
            ViewBag.CategoryName = category;
            var typesByCategory = _context.Types.Where(b => b.CategoryId == id).Include(b => b.Category);

            return View(await typesByCategory.ToListAsync());
        }

        // GET: Types/Details/5
        public async Task<IActionResult> Details(int? categoryId, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = categoryId;
            ViewBag.TypeName = _context.Types.Find(id).Type;
            var types = await _context.Types
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (types == null)
            {
                return NotFound();
            }

            return View(types);
        }

        // GET: Types/Create
        public IActionResult Create(int categoryId)
        {
         
            ViewBag.CategoryId = categoryId;
            ViewBag.CategoryName = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault().Category;
            return View();
        }

        // POST: Types/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int categoryId, [Bind("Id,Type,CategoryId,Info")] Types types)
        {
            types.CategoryId = categoryId;
            if (ModelState.IsValid)
            {
                _context.Add(types);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Types", new { id = categoryId, category = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault().Category });  
            }
           
            return RedirectToAction("Index", "Types", new { id = categoryId, category = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault().Category });
        }

        // GET: Types/Edit/5
        public async Task<IActionResult> Edit(int? categoryId,int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var types = await _context.Types.FindAsync(id);
            if (types == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = categoryId;
            return View(types);
        }

        // POST: Types/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int categoryId,int id, [Bind("Id,Type,CategoryId,Info")] Types types)
        {
            if (id != types.Id)
            {
                return NotFound();
            }
            types.CategoryId = categoryId;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(types);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypesExists(types.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Types", new { id = categoryId, category = _context.Categories.Where(d => d.Id == categoryId).FirstOrDefault().Category });
            }
            
            // ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Category", types.CategoryId);
            //return View(types);
            return RedirectToAction("Index", "Types", new { id = categoryId, category = _context.Categories.Where(d => d.Id == categoryId).FirstOrDefault().Category });

        }

        // GET: Types/Delete/5
        public async Task<IActionResult> Delete(int? categoryId , int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = categoryId;
            ViewBag.TypeName = _context.Types.Find(id).Type;
            var types = await _context.Types
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (types == null)
            {
                return NotFound();
            }

            return View(types);
        }

        // POST: Types/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int categoryId, int id)
        {
            ViewBag.CategoryId = categoryId;
            var types = await _context.Types.FindAsync(id);
            _context.Types.Remove(types);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Types", new { id = categoryId, category = _context.Categories.Where(d => d.Id == categoryId).FirstOrDefault().Category });
        }
        public IActionResult TypeValid(string? Type, int? Id)
        {
            var type = _context.Types.Where(b => b.Type == Type).Where(b => b.Id != Id);
            if (type.Count() > 0)
            {
                return Json(data: "Така послуга вже є в базі");
            }
            return Json(data: true);
        }
        private bool TypesExists(int id)
        {
            return _context.Types.Any(e => e.Id == id);
        }
        // GET: Types/Brokers/5
        public async Task<IActionResult> Brokers(int? categoryId)
        {
            if (categoryId == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = categoryId;
          
           
            return RedirectToAction("Index", "BrokersCategories", new { id = categoryId});
        }
    }
}
