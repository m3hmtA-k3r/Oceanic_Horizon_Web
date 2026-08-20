using Microsoft.AspNetCore.Mvc;
using Oceanic_Horizon_Travel.DTOs.BookingDtos;
using Oceanic_Horizon_Travel.Entities.SubDocuments;
using Oceanic_Horizon_Travel.Services.BookingServices;
using Oceanic_Horizon_Travel.Services.MemberServices;
using Oceanic_Horizon_Travel.Services.PaymentServices;
using Oceanic_Horizon_Travel.Services.TourServices;


namespace Oceanic_Horizon_Travel.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookingController(IBookingServices _bookingServices, IPaymentServices _paymentServices, ITourServices _tourServices,
    IMemberServices _memberServices) : Controller
    {
        public async Task<IActionResult> Index(string status)
        {
            var bookings = await _bookingServices.GetAllAsync(status);

            ViewBag.ActiveStatus = status;
            ViewBag.PendingCount = await _bookingServices.GetPendingCountAsync();

            return View(bookings);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var booking = await _bookingServices.GetDetailAsync(id);

            if (booking == null)
            {
                TempData["Error"] = "Rezervasyon bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(booking);
        }

        public async Task<IActionResult> Approve(string id)
        {
            await _bookingServices.SetStatusAsync(id, "Onaylandı");
            TempData["Success"] = "Rezervasyon onaylandı.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Cancel(string id)
        {
            await _bookingServices.SetStatusAsync(id, "İptal Edildi");
            TempData["Success"] = "Rezervasyon iptal edildi, koltuklar kontenjana geri eklendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MarkAsPaid(string id)
        {
            var booking = await _bookingServices.GetDetailAsync(id);

            if (booking == null)
            {
                TempData["Error"] = "Rezervasyon bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            if (booking.PaymentStatus == "Ödendi")
            {
                TempData["Error"] = "Bu rezervasyonun ödemesi zaten alınmış.";
                return RedirectToAction(nameof(Detail), new { id });
            }

            var transactionNumber = await _paymentServices.CreateAsync(id, booking.TotalPrice, "Havale");

            TempData["Success"] = $"Ödeme kaydedildi. İşlem no: {transactionNumber}";
            return RedirectToAction(nameof(Detail), new { id });
        }

        

    }
}
