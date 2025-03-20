using helo.Data;
using helo.Models.Sales;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace helo.Controllers
{
    public class SalesController : Controller
    {
        private readonly AddDbcontext _context;
        public SalesController(AddDbcontext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var sales = await _context.SalesProducts.Include(s => s.Product).ToListAsync();
            return View(sales);
        }

        [HttpGet]
        public async Task<IActionResult> Buy(int Id)
        {
            var product = await _context.ProductDatas.FindAsync(Id);
            if (product == null)
            {
                return NotFound();
            }

            var model = new SalesProduct
            {
                ProductId = product.Id,
                Product = product,
                TotalPrice = 0 
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Buy(int ProductId, int QuantityToBuy)
        {
            var product = _context.ProductDatas.FirstOrDefault(p => p.Id == ProductId);
            if (product == null)
            {
                return NotFound();
            }

            // Check if requested quantity is available
            if (QuantityToBuy > product.Quantity)
            {
                ModelState.AddModelError("", $"Only {product.Quantity} items are available.");
                return View(new SalesProduct
                {
                    ProductId = product.Id,
                    Product = product,
                    QuantitySold = 0, 
                    TotalPrice = 0
                });
            }
            product.Quantity -= QuantityToBuy;

            var sale = new SalesProduct
            {
                ProductId = product.Id,
                QuantitySold = QuantityToBuy, 
                TotalPrice = product.Price * QuantityToBuy
            };

            _context.SalesProducts.Add(sale);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult GetMostBoughtProduct()
        {
            var mostBoughtProduct = _context.SalesProducts
                .GroupBy(s => s.ProductId)
                .Select(g => new MostBoughtProductViewModel
                {
                    ProductId = g.Key,
                    ProductName = g.First().Product.ProductName,
                    QuantitySold = g.Sum(s => s.QuantitySold)
                })
                .OrderByDescending(p => p.QuantitySold)
                .FirstOrDefault();

            if (mostBoughtProduct == null)
            {
                mostBoughtProduct = new MostBoughtProductViewModel
                {
                    ProductId = 0,
                    ProductName = "No Sales Yet",
                    QuantitySold = 0
                };
            }

            return View(mostBoughtProduct); // ✅ Returns a view instead of JSON
        }

    }
}
