using Ali_Store.Data.Context;
using Ali_Store.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Ali_Store.Extensions;

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
        public async Task<IActionResult> Index(string? CatgType)
        {
            ViewBag.iSlaptop = false;
            var U_id = HttpContext.Session.GetInt32("User_id");
            // if(U_id == null) {
            //     return RedirectToAction("Login", "");
            // }
            ViewBag.IsAdmin = U_id == 1;

            IQueryable<Product> productsQuery = _context.Products.Include(p => p.Rates);

            if (CatgType != null && CatgType != "All")
            {
                if(CatgType == "Laptob") 
                {
                    ViewBag.iSlaptop = true;
                }
                    productsQuery = productsQuery.Where(p => p.Type == CatgType);
            }

            var products = await productsQuery.ToListAsync();

            foreach (var product in products)
            {
                product.AverageRating = product.Rates.Any() ? product.Rates.Average(r => r.RateTo5) : 0;
            }

            return View(products);
        }

        // GET: Products/Details/[id]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }           
            var product = await _context.Products
                .Include(p => p.Rates).ThenInclude(r => r.User).Include(p => p.Offers)
                    
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            var U_id = HttpContext.Session.GetInt32("User_id");
            if(U_id == null) {
                return RedirectToAction("Login", "");
            }
            if (U_id == 1)
                ViewBag.IsAdmin = true;
            else
                ViewBag.IsAdmin = false;
            return View(product);
        }
        public async Task<IActionResult> Buy(int? id)
        {
            var U_id = HttpContext.Session.GetInt32("User_id");
            if(U_id == null) {
                return RedirectToAction("Login", "");
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
            var OPrice = prduct.Price;
            prduct.IsSall = true;
            if(prduct.NewPrice != null)
            {
                OPrice = (float)prduct.NewPrice;
            }
            User.Amount -= OPrice;
            AUser.Amount += OPrice;
            
            var reder = new Order()
            {
                User = User,
                TotalPrice = OPrice,
                Date = DateTime.Now
            };
            
            var Order = _context.Orders.Add(reder);
            await _context.SaveChangesAsync();

            var OrderItem = new OrderItem()
            {
                OrderId = Order.Entity.Id,
                ProductId = prduct.Id
            };
            _context.OrderItems.Add(OrderItem);
            _context.Products.Update(prduct);
            _context.Users.Update(User);
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = "Product bought successfully!";
            TempData["ToastType"] = "success";
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
                TempData["ToastMessage"] = "Product created successfully!";
                TempData["ToastType"] = "success";
                return RedirectToAction(nameof(Index));
            }
            TempData["ToastMessage"] = "Product creation failed!";
            TempData["ToastType"] = "error";
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Model,Price,Pic,GoodFor,NewPrice")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle image file upload
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files.FirstOrDefault(f => f.Name == "ImageFile");
                        if (file != null && file.Length > 0)
                        {
                            // Generate a unique filename to avoid overwrites
                            string fileName = Path.GetFileName(file.FileName);
                            
                            // Save the file to the images folder
                            string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", fileName);
                            using (FileStream fs = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fs);
                            }
                            
                            // Update the product's Pic property with the new filename
                            product.Pic = fileName;
                        }
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                        throw;
                }
                TempData["ToastMessage"] = "Product updated successfully!"; 
                TempData["ToastType"] = "success";
                return RedirectToAction("index", "Products");
            }
            TempData["ToastMessage"] = "Product update failed!";
            TempData["ToastType"] = "error";
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
            TempData["ToastMessage"] = "Product deleted successfully!";
            TempData["ToastType"] = "success";
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRating(int productId, int rateTo5, string comment)
        {
            var userId = HttpContext.Session.GetInt32("User_id");
            if (userId == null)
            {
                return Unauthorized("User not logged in.");
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            var rate = new Rate
            {
                UserId = userId.Value,
                ProductId = productId,
                RateTo5 = rateTo5,
                Comment = comment
            };

            _context.Rates.Add(rate);
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = "Rating added successfully!";
            TempData["ToastType"] = "success";

            return RedirectToAction("Details", new { id = productId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOffer(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }
            product.NewPrice = newPrice;


                // Create a new offer
                var newOffer = new Offer
                {
                    ProductId = product.Id,
                    Price = newPrice
                };
            _context.Offers.Add(newOffer);

            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = "Offer added successfully!";
            TempData["ToastType"] = "success";
            return RedirectToAction("Details", new { id = productId });
        }

        public async Task<IActionResult> EditOffer(int id)
        {
            var offer = await _context.Offers
                .Include(o => o.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
                
            if (offer == null)
            {
                return NotFound("Offer not found.");
            }

            return View(offer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOffer(int id, decimal newPrice)
        {
            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
            {
                return NotFound("Offer not found.");
            }

            offer.Price = newPrice;
            
            // Update the product's NewPrice property as well
            var product = await _context.Products.FindAsync(offer.ProductId);
            if (product != null)
            {
                product.NewPrice = newPrice;
                _context.Products.Update(product);
            }

            _context.Offers.Update(offer);
            await _context.SaveChangesAsync();
            
            TempData["ToastMessage"] = "Offer updated successfully!";
            TempData["ToastType"] = "success";
            return RedirectToAction("Details", new { id = offer.ProductId });
        }

        public async Task<IActionResult> Orders()
        {
            var U_id = HttpContext.Session.GetInt32("User_id");
            if(U_id == null) {
                return RedirectToAction("Login", "");
            }
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();

            if(U_id != 1) {
                    orders = await _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .Where(o => o.User.Id == U_id)
                    .ToListAsync();
            }
            return View(orders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var U_id = HttpContext.Session.GetInt32("User_id");
            if(U_id == null || U_id != 1) {
                return RedirectToAction("Login", "");
            }

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            TempData["ToastMessage"] = "Order deleted successfully!";
            TempData["ToastType"] = "success";
            return RedirectToAction("Orders");
        }

        public async Task<IActionResult> Cart()
        {
            var userId = HttpContext.Session.GetInt32("User_id");
            if (userId == null)
            {
                return RedirectToAction("Login", "");
            }
            
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
                
            if (cart == null)
            {
                cart = new Cart { UserId = userId.Value };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }
            
            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var userId = HttpContext.Session.GetInt32("User_id");
            if (userId == null)
            {
                return RedirectToAction("Login", "");
            }
            
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);
                
            if (cart == null)
            {
                cart = new Cart { UserId = userId.Value };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
                _context.CartItems.Update(cartItem);
            }
            else
            {
                cartItem = new CartItem 
                { 
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = "Product added to cart!";
            TempData["ToastType"] = "success";
            return RedirectToAction("index", "Products");
        }

        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = HttpContext.Session.GetInt32("User_id");
            if (userId == null)
            {
                return RedirectToAction("Login", "");
            }
            
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);
                
            if (cart != null)
            {
                var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
                if (cartItem != null)
                {
                    _context.CartItems.Remove(cartItem);
                    await _context.SaveChangesAsync();
                }
            }

            TempData["ToastMessage"] = "Product removed from cart!";
            TempData["ToastType"] = "success";
            return RedirectToAction("Cart");
        }

        public async Task<IActionResult> PurchaseCart()
        {
            var userId = HttpContext.Session.GetInt32("User_id");
            if (userId == null)
            {
                return RedirectToAction("Login", "");
            }

            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
                
            if (cart == null || !cart.Items.Any())
            {
                TempData["ToastMessage"] = "Cart is empty!";
                TempData["ToastType"] = "error";
                return RedirectToAction("Cart");
            }
            
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.Amount < cart.TotalPrice)
            {
                TempData["ToastMessage"] = "Insufficient balance!";
                TempData["ToastType"] = "error";
                return RedirectToAction("Cart");
            }

            // Get the admin user for payment
            var adminUser = await _context.Users.FindAsync(1);
            if (adminUser == null)
            {
                TempData["ToastMessage"] = "System error: Admin account not found";
                TempData["ToastType"] = "error";
                return RedirectToAction("Cart");
            }

            int totalItems = cart.Items.Sum(i => i.Quantity);
            
            var order = new Order
            {
                User = user,
                Date = DateTime.Now,
                TotalPrice = cart.TotalPrice,
                NumberOfItems = totalItems
            };
            
            user.Amount -= order.TotalPrice;
            adminUser.Amount += order.TotalPrice;
            
            _context.Users.Update(user);
            _context.Users.Update(adminUser);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in cart.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                product.IsSall = true;
                
                // Create an OrderItem for each quantity
                for (int i = 0; i < item.Quantity; i++)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId
                    };
                    _context.OrderItems.Add(orderItem);
                }
                
                _context.Products.Update(product);
            }

            // Clear the cart
            _context.CartItems.RemoveRange(cart.Items);
            await _context.SaveChangesAsync();

            TempData["ToastMessage"] = "Purchase successful!";
            TempData["ToastType"] = "success";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToAction("Index");
            }

            ViewBag.SearchTerm = searchTerm;
            var U_id = HttpContext.Session.GetInt32("User_id");
            ViewBag.IsAdmin = U_id == 1;

            var lowercaseSearchTerm = searchTerm.ToLower();
            
            var products = await _context.Products
                .Include(p => p.Rates)
                .Where(p => p.Name.ToLower().Contains(lowercaseSearchTerm) || 
                           p.Model.ToLower().Contains(lowercaseSearchTerm) || 
                           p.Type.ToLower().Contains(lowercaseSearchTerm) || 
                           p.GoodFor.ToLower().Contains(lowercaseSearchTerm))
                .ToListAsync();

            foreach (var product in products)
            {
                product.AverageRating = product.Rates.Any() ? product.Rates.Average(r => r.RateTo5) : 0;
            }

            TempData["ToastMessage"] = $"Found {products.Count} results for '{searchTerm}'";
            TempData["ToastType"] = "info";
            
            return View("Index", products);
        }

        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            if (quantity < 1)
            {
                TempData["ToastMessage"] = "Quantity must be at least 1!";
                TempData["ToastType"] = "error";
                return RedirectToAction("Cart");
            }
            
            var userId = HttpContext.Session.GetInt32("User_id");
            if (userId == null)
            {
                return RedirectToAction("Login", "");
            }
            
            var cartItem = await _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.Cart.UserId == userId);
                
            if (cartItem == null)
            {
                return NotFound("Cart item not found.");
            }
            
            cartItem.Quantity = quantity;
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
            
            TempData["ToastMessage"] = "Quantity updated!";
            TempData["ToastType"] = "success";
            return RedirectToAction("Cart");
        }
    }
}
