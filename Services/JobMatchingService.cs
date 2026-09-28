using Microsoft.AspNetCore.Mvc;

namespace CareerPilot_AI.Services
{
    public class JobMatchingService : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
