using Ali_Store.Data.Context;
using Ali_Store.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Ali_Store.Controllers
{
    public class ProductsController : Controller
    {
        private readonly StoreContext _context;
        protected readonly IHostingEnvironment _hostingEnvironment;
        public ProductsController(StoreContext context,IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Products
        public async Task<IActionResult> Index(int? CatgType)
        {
            ViewBag.iSlaptop = false;
            var U_id = HttpContext.Session.GetInt32("User_id");
            if (U_id == 1)
                ViewBag.IsAdmin = true;
            else
                ViewBag.IsAdmin = false;
            switch (CatgType)
            {
                case 1:
                    {
                        ViewBag.iSlaptop = true;
                        return _context.Products != null ?
                                                    View(await _context.Products.Where(p => p.Type == "Laptob").ToListAsync()) :
                                                    Problem("Entity set 'StoreContext.Products'  is null.");
                    }

                case 2:
                    return _context.Products != null ?
                          View(await _context.Products.Where(p => p.Type == "Mobail").ToListAsync()) :
                          Problem("Entity set 'StoreContext.Products'  is null.");
                case 3:
                    return _context.Products != null ?
                          View(await _context.Products.Where(p => p.Type == "Extentions").ToListAsync()) :
                          Problem("Entity set 'StoreContext.Products'  is null.");
                case 4:
                    return _context.Products != null ?
                          View(await _context.Products.ToListAsync()) :
                          Problem("Entity set 'StoreContext.Products'  is null.");
                default:
                    return _context.Products != null ?
                          View(await _context.Products.ToListAsync()) :
                          Problem("Entity set 'StoreContext.Products'  is null.");
                    break;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Index(string? GoodFor)
        {
            ViewBag.iSlaptop = true;
            var U_id = HttpContext.Session.GetInt32("User_id");
            if (U_id == 1)
                ViewBag.IsAdmin = true;
            else
                ViewBag.IsAdmin = false;
            
                        return _context.Products != null ?
                                View(await _context.Products.Where(p => p.Type == "Laptob")
                                .Where(p => p.GoodFor.Contains(GoodFor)).ToListAsync()) :
                                Problem("Entity set 'StoreContext.Products'  is null.");                                  
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }           
            var product = await _context.Products
                .Include(p => p.Rates)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            var U_id = HttpContext.Session.GetInt32("User_id");
            if (U_id == 1)
                ViewBag.IsAdmin = true;
            else
                ViewBag.IsAdmin = false;
            return View(product);
        }
        public async Task<IActionResult> Buy(int? id)
        {
            var U_id = HttpContext.Session.GetInt32("User_id");
            if (U_id == null)
            {
                return NotFound("User not found.");
            }
            var User = _context.Users.Find(U_id);
            var AUser = _context.Users.Find(1);
            var prduct = _context.Products.Find(id);

            if (User == null)
            {
                return NotFound("User not found.");
            }

            if (AUser == null)
            {
                return NotFound("Admin user not found.");
            }

            if (prduct == null)
            {
                return NotFound("Product not found.");
            }
            
            prduct.IsSall = true;
            User.Amount -= prduct.Price;
            AUser.Amount += prduct.Price;
            var reder = new Order()
            {
                User = User,
                TotalPrice = prduct.Price,
                Date = DateTime.Now
            };            
            _context.Orders.Add(reder);
            _context.Products.Update(prduct);
            _context.Users.Update(User);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");

        }
        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type,Model,Price,Pic,GoodFor")] Product product)
        {
            if (Request.Form.Files.Count != 0)
            {
                var file = Request.Form.Files.First();
                product.Pic = file.FileName;
                using (FileStream fs = new FileStream
                    (Path.Combine(_hostingEnvironment.WebRootPath,"Images",file.FileName), FileMode.OpenOrCreate)                   )
                //Create the file in your file system with the name you want.
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        //Copy the uploaded file data to a memory stream
                        file.CopyTo(ms);
                        //Now write the data in the memory stream to the new file
                        fs.Write(ms.ToArray());
                    }
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Model,Price,Pic,GoodFor")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'StoreContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
