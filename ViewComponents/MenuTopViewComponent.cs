using Doan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Doan.ViewComponents
{
    public class MenuTopViewComponent : ViewComponent
    {
        private readonly DoanContext _context;
        public MenuTopViewComponent(DoanContext context)
        { _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = _context.TbMenus.Where(m => (bool)m.IsActive).
                OrderBy(m => m.Position).ToList();
            return await Task.FromResult<IViewComponentResult>(View(items));
        }
    }
}
