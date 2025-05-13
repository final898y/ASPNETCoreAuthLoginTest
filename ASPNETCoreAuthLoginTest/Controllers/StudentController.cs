using Microsoft.AspNetCore.Mvc;

namespace ASPNETCoreAuthLoginTest.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
