using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OOPLab1.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
namespace OOPLab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {

        private PillsContext _context;
        public ChartController(PillsContext context)
        {
            _context = context;
        }

        [HttpGet("JsonClassData")]
        public JsonResult JsonClassData()
        {
            var classes = _context.PillClasses.Include(p => p.Pills).ToList();
            List<object> data = new List<object>();
            data.Add(new[] { "Клас", "Кількість пігулок" });
            foreach (var p in classes)
            {
                data.Add(new object[] { p.Name, p.Pills.Count });
            }

            return new JsonResult(data);
        }

        [HttpGet("JsonPharmasyData")]
        public JsonResult JsonPharmasyData()
        {
            var pharmasies = _context.Pharmasies.Include(p => p.Pills).ToList();
            List<object> data = new List<object>();
            data.Add(new[] { "Аптека", "Кількість ліків" });
            foreach (var p in pharmasies)
            {
                data.Add(new object[] { p.Name, p.Pills.Count });
            }

            return new JsonResult(data);
        }
    }
}
