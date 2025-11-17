using Doan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Doan.ViewComponents
{
    public class DepartmentsViewComponent : ViewComponent
    {
        private readonly DoanContext _context;
        public DepartmentsViewComponent(DoanContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = _context.TbDepartments.Include(m => m.Category)
                .Where(m => (bool)m.IsActive).Where(m => m.IsNew);
            return await Task.FromResult<IViewComponentResult>
                (View(items.OrderByDescending(m => m.CategoryId).ToList()));
        }
    }
}
