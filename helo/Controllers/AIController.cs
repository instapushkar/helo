using helo.Data;
using helo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace helo.Controllers
{
    public class AIController : Controller
    {
        private readonly AIService _aiService;
        private readonly AddDbcontext _context;

        public AIController(AIService aiService, AddDbcontext context)
        {
            _aiService = aiService;
            _context = context;
        }
        [Authorize]
        public async Task<IActionResult> GetRestockingSuggestion()
        {
            var salesData = _context.SalesProducts
                .Select(s => new { s.Product.ProductName, s.QuantitySold })
                .ToList();

            string formattedData = string.Join(", ", salesData.Select(s => $"{s.ProductName}: {s.QuantitySold} sold"));

            var aiResponse = await _aiService.GetRestockingPrediction(formattedData);

            ViewBag.AIResponse = aiResponse;
            return View();
        }

    }
}