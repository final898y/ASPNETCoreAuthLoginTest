using Microsoft.AspNetCore.Mvc;

namespace ASPNETCoreAuthLoginTest.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
