using Doan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Doan.ViewComponents
{
    public class AboutViewComponent : ViewComponent
    {
        private readonly DoanContext _context;
        public AboutViewComponent(DoanContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = _context.TbAbouts.Include(m => m.Category)
                .Where(m => (bool)m.IsActive).Where(m => m.IsNew);
            return await Task.FromResult<IViewComponentResult>
                (View(items.OrderByDescending(m => m.CategoryId).ToList()));
        }
    }
}
