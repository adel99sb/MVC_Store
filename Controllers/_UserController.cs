using Ali_Store.Data.Context;
using Ali_Store.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ali_Store.Controllers
{
    public class _UserController : Controller
    {
        private readonly StoreContext _context ;

        public _UserController(StoreContext context)
        {
            _context = context;
        }

        // GET: _User
        public async Task<IActionResult> Index()
        {
            return _context.Users != null ?
                        View(await _context.Users.ToListAsync()) :
                        Problem("Entity set 'StoreContext.Users'  is null.");
        }

        // GET: _User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var _User = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (_User == null)
            {
                return NotFound();
            }

            return View(_User);
        }

        // GET: _User/Create
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        // POST: _User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,EMail,Password")] _User _User)
        {
            if (ModelState.IsValid)
            {
                _context.Add(_User);
                await _context.SaveChangesAsync();
                TempData["ToastMessage"] = "User created successfully!";
                TempData["ToastType"] = "success";
                return RedirectToAction("Login");
            }
            TempData["ToastMessage"] = "User creation failed!";
            TempData["ToastType"] = "error";
            return View(_User);
        }
        [HttpPost]
        public async Task<IActionResult> Login([Bind("Id,EMail,Password")] _User _User)
        {
            int id = 0;
            var Res = _context.Users.ToList();
            foreach (var item in Res)
            {
                if (_User.EMail == item.EMail && _User.Password == item.Password)
                {
                    id = item.Id;
                    break;
                }
            }
            if (id != 0)
            {
                HttpContext.Session.SetInt32("User_id", id);
                return RedirectToAction("index", "Products");
            }
            return View();
        }

        // GET: _User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var _User = await _context.Users.FindAsync(id);
            if (_User == null)
            {
                return NotFound();
            }
            return View(_User);
        }

        // POST: _User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,E_Mail,Password,Amount")] _User _User)
        {
            if (id != _User.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(_User);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_UserExists(_User.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["ToastMessage"] = "User updated successfully!";
                TempData["ToastType"] = "success";
                return RedirectToAction(nameof(Index));
            }
            TempData["ToastMessage"] = "User update failed!";
            TempData["ToastType"] = "error";
            return View(_User);
        }

        // GET: _User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var _User = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (_User == null)
            {
                return NotFound();
            }

            return View(_User);
        }

        // POST: _User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'StoreContext.Users'  is null.");
            }
            var _User = await _context.Users.FindAsync(id);
            if (_User != null)
            {
                _context.Users.Remove(_User);
            }

            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = "User deleted successfully!";
            TempData["ToastType"] = "success";
            return RedirectToAction(nameof(Index));
        }

        private bool _UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
