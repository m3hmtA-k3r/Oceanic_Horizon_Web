using Microsoft.AspNetCore.Mvc;
using Oceanic_Horizon_Travel.Services.PaymentServices;

namespace Oceanic_Horizon_Travel.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PaymentController(IPaymentServices _paymentServices) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var payments = await _paymentServices.GetAllAsync();

            ViewBag.TotalRevenue = await _paymentServices.GetTotalRevenueAsync();

            return View(payments);
        }
    }
}
