using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsuranceDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly InsuranceContext _context;
        public ChartsController(InsuranceContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData1")]
        public JsonResult JsonData()
        {
            var categories = _context.Categories.Include(b => b.Types).ToList();
            List<object> catTypes = new List<object>();
            catTypes.Add(new[] { "Категорія", "Послуг за категорією" });
            foreach(var c in categories)
            {
                catTypes.Add(new object[] { c.Category, c.Types.Count() });
            }
            return new JsonResult(catTypes);
        }
        [HttpGet("JsonData2")]
        public JsonResult JsonData2()
        {
            var brokers = _context.Brokers.Include(b=>b.Documents);
            List<object> brokersDocs = new List<object>();
            brokersDocs.Add(new[] { "Брокер", "Підписано договорів" });
            foreach (var c in brokers)
            {
                brokersDocs.Add(new object[] { c.FullName ,c.Documents.Count() });
            }
            return new JsonResult(brokersDocs);
        }
    }
}