using Microsoft.AspNetCore.Mvc;

namespace ASPNETCoreAuthLoginTest.Models
{
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
