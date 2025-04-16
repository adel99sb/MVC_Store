using Ali_Store.Data.Context;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ali_Store.Components
{
    public class UserAmountViewComponent : ViewComponent
    {
        private readonly StoreContext _context;

        public UserAmountViewComponent(StoreContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = HttpContext.Session.GetInt32("User_id");
            if (userId == null)
            {
                return Content("");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return Content("");
            }

            return View(user.Amount);
        }
    }
} 