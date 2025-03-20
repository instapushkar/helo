using helo.Data;
using helo.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace helo.Controllers
{
    public class ProductsController : Controller
    {

        private readonly AddDbcontext _context;
        public ProductsController(AddDbcontext context)
        {
            _context = context;
        }
        [Authorize]
        public IActionResult Index()
        {
            var products = _context.ProductDatas.ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductData product)
        {
            if (ModelState.IsValid)
            {
                await _context.ProductDatas.AddAsync(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.ProductDatas.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductData product)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = await _context.ProductDatas.FindAsync(product.Id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                existingProduct.ProductName = product.ProductName;
                existingProduct.Quantity = product.Quantity;
                existingProduct.Price = product.Price;

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(product);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.ProductDatas.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.ProductDatas.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.ProductDatas.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index"); 
        }


    }
}
