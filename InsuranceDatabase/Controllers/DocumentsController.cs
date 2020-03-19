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

namespace InsuranceDatabase.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly InsuranceContext _context;

        public DocumentsController(InsuranceContext context)
        {
            _context = context;
        }

        // GET: Documents
        public async Task<IActionResult> Index(int? brokerId, int? clientId,string? error)
        {
            if (brokerId > 0)
            {
                
                ViewBag.ClientDocId = -1;
                ViewBag.BrokerDocId = brokerId;
                var brokersDocuments = _context.Documents.Where(b => b.BrokerId == brokerId).Include(b => b.Client).Include(b => b.Broker).Include(b => b.Type).ToList();
                return View(brokersDocuments);
            }
            else if (clientId > 0)
            {
                ViewBag.BrokerDocId = -1;
                ViewBag.ClientDocId = clientId;
                var clientsDocuments = _context.Documents.Where(b => b.ClientId == clientId).Include(b => b.Client).Include(b => b.Broker).Include(b => b.Type).ToList();
                return View(clientsDocuments);
            }
            else
            {
                ViewBag.ErrorMes = error;
                ViewBag.BrokerDocId = -1;
                ViewBag.ClientDocId = -1;
                var insuranceContext = _context.Documents.Include(d => d.Broker).Include(d => d.Client).Include(d => d.Type);
                return View(await insuranceContext.ToListAsync());
            }
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id, int? brokerId, int clientId)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.BrokerDocId = brokerId;
            ViewBag.ClientDocId = clientId;
            var documents = await _context.Documents
                .Include(d => d.Broker)
                .Include(d => d.Client)
                .Include(d => d.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (documents == null)
            {
                return NotFound();
            }

            return View(documents);
        }

        // GET: Documents/Create
        public IActionResult Create(int? bbrokerId, int? cclientId)
        {
            ViewBag.BrokerDocId = bbrokerId;
            ViewBag.ClientDocId = cclientId;
            if (bbrokerId > 0) { ViewData["BrokersId"] = new SelectList(_context.Brokers.Where(b => b.Id == bbrokerId), "Id", "FullName");
                ViewData["ClientsId"] = new SelectList(_context.Clients, "Id", "FullName");
            }
            else if (cclientId > 0) { ViewData["ClientsId"] = new SelectList(_context.Clients.Where(b => b.Id == cclientId), "Id", "FullName");
                ViewData["BrokersId"] = new SelectList(_context.Brokers, "Id", "FullName");
            }
            else { ViewData["BrokersId"] = new SelectList(_context.Brokers, "Id", "FullName");
                ViewData["ClientsId"] = new SelectList(_context.Clients, "Id", "FullName");
            }

            ViewData["TypesId"] = new SelectList(_context.Types, "Id", "Type");
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int bbrokerId, int cclientId, [Bind("Id,Number,Date,Sum,TypeId,ClientId,BrokerId")] Documents documents)
        {
            ViewBag.ClientDocId = cclientId;
            ViewBag.BrokerDocId = bbrokerId;
            if (ModelState.IsValid)
            {
                _context.Add(documents);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Documents", new { brokerId = bbrokerId, clientId = cclientId });
            }

            return RedirectToAction("Index", "Documents", new { brokerId = bbrokerId, clientId = cclientId });
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id, int? bDocId, int? clDocId)
        {
            ViewBag.ClientDocId = clDocId;
            ViewBag.BrokerDocId = bDocId;

            if (id == null)
            {
                return NotFound();
            }
            if (bDocId > 0) { ViewData["BrokersId"] = new SelectList(_context.Brokers.Where(b => b.Id == bDocId), "Id", "FullName");
                ViewData["ClientsId"] = new SelectList(_context.Clients, "Id", "FullName");
            }
            else if (clDocId > 0) { ViewData["ClientsId"] = new SelectList(_context.Clients.Where(b => b.Id == clDocId), "Id", "FullName");
                ViewData["BrokersId"] = new SelectList(_context.Brokers, "Id", "FullName");
            }
            else { ViewData["BrokersId"] = new SelectList(_context.Brokers, "Id", "FullName");
                ViewData["ClientsId"] = new SelectList(_context.Clients, "Id", "FullName");
            }
            var documents = await _context.Documents.FindAsync(id);

            if (documents == null)
            {
                return NotFound();
            }
            ViewData["TypesId"] = new SelectList(_context.Types, "Id", "Type", documents.TypeId);
            return View(documents);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string mySum, int id, int bDocId, int clDocId, [Bind("Id,Number,Date,Sum,TypeId,ClientId,BrokerId")] Documents documents)
        {
            ViewBag.ClientDocId = clDocId;
            ViewBag.BrokerDocId = bDocId;
            if (id != documents.Id)
            {
                return NotFound();
            }

            
            if (ModelState.IsValid)
            {

                try
                {
                    _context.Update(documents);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentsExists(documents.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Documents", new { brokerId = bDocId, clientId = clDocId });
            }

            return RedirectToAction("Index", "Documents", new { brokerId = bDocId, clientId = clDocId });
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int? id, int? brokerId, int? clientId)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.ClientDocId = clientId;
            ViewBag.BrokerDocId = brokerId;
            var documents = await _context.Documents
                .Include(d => d.Broker)
                .Include(d => d.Client)
                .Include(d => d.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (documents == null)
            {
                return NotFound();
            }

            return View(documents);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int brokerId, int clientId)
        {
            ViewBag.ClientDocId = clientId;
            ViewBag.BrokerDocId = brokerId;
            var documents = await _context.Documents.FindAsync(id);
            _context.Documents.Remove(documents);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Documents", new { brokerId = brokerId, clientId = clientId });
        }
      


        public IActionResult DateValid(DateTime? Date)
        {
            char[] param = { '.', '/', ':', ' ' };
            string birthDate = Date.ToString();
            int year, day, month;
            try
            {
                year = Convert.ToInt32(birthDate.Split(param)[2]);
                month = Convert.ToInt32(birthDate.Split(param)[1]);
                day = Convert.ToInt32(birthDate.Split(param)[0]);
            }
            catch (Exception e) { return Json(data: "Невірний формат данних"); }
            if (birthDate != null)
            {
                if (year < 2018 || year > DateTime.Today.Year || (year == DateTime.Today.Year
                    && month > DateTime.Today.Month) || (year == DateTime.Today.Year && month == DateTime.Today.Month
                    && day > DateTime.Today.Day))
                {
                    return Json(data: "Невірна дата");
                }
            }
            return Json(data: true);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            string ErrorMes;
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                Categories newcat;
                                var c = (from cat in _context.Categories
                                         where cat.Category.Equals(worksheet.Name)
                                         select cat).ToList();
                                if (c.Count > 0)
                                {
                                    newcat = c[0];
                                }
                                else
                                {
                                    newcat = new Categories();
                                    newcat.Category = worksheet.Name;
                                    _context.Categories.Add(newcat);
                                }

                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Documents doc = new Documents();
                                        doc.Type = new Types();
                                        doc.Broker = new Brokers();
                                        doc.Client = new Clients();
                                        try
                                        {
                                            doc.Number = row.Cell(1).Value.ToString();
                                            if (doc.Number.Length != 10 )
                                            {
                                                throw new Exception("Невірно вказаний номер документа в категорії " + worksheet.Name + " в " + row.RowNumber().ToString() + " рядку");
                                            }
                                            for(int i = 0; i<10; i++)
                                            {
                                                if(doc.Number[i] < '0' || doc.Number[i] > '9') { throw new Exception("Невірно вказаний номер документа в категорії " + worksheet.Name + " в " + row.RowNumber().ToString() + " рядку"); }
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            throw new Exception("Невірно вказаний номер документа в категорії " + worksheet.Name + " в " + row.RowNumber().ToString() + " рядку");
                                        }
                                        
                                        //doc.Type.Type = row.Cell(2).Value.ToString();
                                       
                                        var t = (from ty in _context.Types
                                                 where ty.Type.Equals(row.Cell(2).Value.ToString())
                                                 select ty).ToList();
                                        if (t.Count > 0)
                                        {
                                            doc.Type = t[0];
                                        }
                                        else
                                        {
                                            throw new Exception("Невірий тип договору в категорії " + worksheet.Name + " в " + row.RowNumber().ToString() + " рядку");

                                        }

                                        //doc.Broker.FullName = row.Cell(3).Value.ToString();
                                        char[] param = { ' ' };
                                        string name = row.Cell(3).Value.ToString().Split(param)[0];
                                        string surname = row.Cell(3).Value.ToString().Split(param)[1];
                                        var a = (from br in _context.Brokers
                                                 where br.Name.Equals(name)&& br.Surname.Equals(surname)
                                                 select br).ToList();
                                        if (a.Count > 0)
                                        {
                                            doc.Broker = a[0];
                                        }
                                        else
                                        {
                                            throw new Exception("Невівне ім'я брокера в категорії " + worksheet.Name + " в " + row.RowNumber().ToString() + " рядку" +
                                                "Такого брокера немає в базі");
                                        }


                                        // doc.Client.FullName = row.Cell(4).Value.ToString();
                                        
                                        name = row.Cell(4).Value.ToString().Split(param)[0];
                                        surname = row.Cell(4).Value.ToString().Split(param)[1];
                                        var cli = (from clnt in _context.Clients
                                                   where clnt.Name.Equals(name)&& clnt.Surname.Equals(surname)
                                                   select clnt).ToList();
                                        if (cli.Count > 0)
                                        {
                                            doc.Client = cli[0];
                                        }
                                        else
                                        {
                                            throw new Exception("Невірне ім'я клієнта в категорії " + worksheet.Name+ " в "  + row.RowNumber().ToString() + " рядку." +
                                                " Такого клієнта немає в базі");

                                        }
                                        try
                                        {
                                            doc.Date = Convert.ToDateTime(row.Cell(5).Value);
                                        }
                                        catch (Exception e)
                                        {
                                            throw new Exception("Невірно вказана дата в категорії " + worksheet.Name + " в " + row.RowNumber().ToString() + " рядку");
                                        }
                                        try
                                        {
                                            doc.Sum = Convert.ToDecimal(row.Cell(6).Value);
                                        }
                                        catch (Exception e)
                                        {
                                            throw new Exception("Невірно вказана ціна в категорії " +worksheet.Name + " в " + row.RowNumber().ToString() + " рядку");
                                        }

                                        var d = (from dd in _context.Documents
                                                 where dd.Number.Equals(doc.Number)
                                                 select dd).ToList();
                                        if (d.Count == 0)
                                        {
                                            _context.Documents.Add(doc);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        ViewBag.ErrorMes = e.Message;
                                        return RedirectToAction("Index", "Documents", new { brokerId = -1, clientId = -1, error = e.Message });
                                    }

                                }

                                    
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
            return RedirectToAction(nameof(Index));
    }
        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {

                var categories = _context.Categories.Include(b=>b.Types).ToList();
                 
                
               
                foreach (var d in categories)
                {
                    var worksheet = workbook.Worksheets.Add(d.Category);
                   var docs = _context.Documents.Where(b => b.Type.CategoryId == d.Id).Include("Broker").Include("Client").ToList();
                    worksheet.Cell("A1").Value = "Номер договору";
                    worksheet.Cell("B1").Value = "Тип договору";
                    worksheet.Cell("C1").Value = "Ім'я брокера";
                    worksheet.Cell("D1").Value = "Ім'я клієнта";
                    worksheet.Cell("E1").Value = "Дата складання";
                    worksheet.Cell("F1").Value = "Сума до сплати";
                   
                    worksheet.Row(1).Style.Font.Bold = true;
                
                    for (int i = 0; i < docs.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = docs[i].Number;
                        worksheet.Cell(i + 2, 2).Value = docs[i].Type.Type;
                        worksheet.Cell(i + 2, 3).Value = docs[i].Broker.FullName;
                        worksheet.Cell(i + 2, 4).Value = docs[i].Client.FullName;
                        worksheet.Cell(i + 2, 5).Value = docs[i].Date.ToString();
                        worksheet.Cell(i + 2, 6).Value = docs[i].Sum;

                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"Documents_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

        private bool DocumentsExists(int id)
        {
            return _context.Documents.Any(e => e.Id == id);
        }
    }
}
