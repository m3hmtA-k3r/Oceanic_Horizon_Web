using Microsoft.AspNetCore.Mvc;
using Oceanic_Horizon_Travel.Services.DashboardServices;

namespace Oceanic_Horizon_Travel.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController(IDashboardServices _dashboardServices) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var model = await _dashboardServices.GetDashboardAsync();
            return View(model);
        }
    }
}
