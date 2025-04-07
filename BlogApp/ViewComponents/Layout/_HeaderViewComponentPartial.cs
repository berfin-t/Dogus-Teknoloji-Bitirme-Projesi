using Microsoft.AspNetCore.Mvc;

namespace BlogApp.ViewComponents.Layout
{
    public class _HeaderViewComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }


    }
}
