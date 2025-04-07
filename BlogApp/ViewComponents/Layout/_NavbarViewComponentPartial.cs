using Microsoft.AspNetCore.Mvc;

namespace BlogApp.ViewComponents.Layout
{
    public class _NavbarViewComponentPartial: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    
    }
}
