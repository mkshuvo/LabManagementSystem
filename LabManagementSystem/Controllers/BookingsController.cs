using Microsoft.AspNetCore.Mvc;

namespace LabManagementSystem.Controllers
{
    public class BookingsController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}