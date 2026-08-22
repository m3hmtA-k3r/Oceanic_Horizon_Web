using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oceanic_Horizon_Travel.DTOs.BookingDtos;
using Oceanic_Horizon_Travel.Entities.SubDocuments;
using Oceanic_Horizon_Travel.Services.BookingServices;
using Oceanic_Horizon_Travel.Services.PaymentServices;
using Oceanic_Horizon_Travel.Services.TourServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Oceanic_Horizon_Travel.Controllers
{
    [Authorize]
    public class BookingController(ITourServices _tourServices,
                                   IBookingServices _bookingServices,
                                   IPaymentServices _paymentServices) : Controller
    {

        public async Task<IActionResult> Create(string tourDateId)
        {
            var model = await BuildViewModelAsync(tourDateId);
            if (model == null )
            {
                return NotFound();
            }


            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Create(BookingCreateViewModel form)
        {
            var memberId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(memberId))
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var bookingNumber = await _bookingServices.CreateAsync(new CreateBookingDto
                {
                    MemberId = memberId,
                    TourId = form.TourId,
                    TourDateId = form.TourDateId,
                    AdultCount = form.AdultCount,
                    ChildCount = form.ChildCount,
                    Guests = form.Guests ?? new List<Guest>()
                });

                return RedirectToAction(nameof(Success), new { bookingNumber });
            }
            catch(InvalidOperationException ex)
            {                
                TempData["Error"] = ex.Message;// Servisten gelen mesajlar kullanıcıya gösterilebilir: 
                // "Bu tarihte yalnızca 2 koltuk kaldı." gibi

                var model = await BuildViewModelAsync(form.TourDateId);
                if (model is null) return NotFound();

                model.AdultCount = form.AdultCount;
                model.ChildCount = form.ChildCount;
                model.Guests = form.Guests ?? new List<Guest>();

                return View(model);
            }
        }

        public IActionResult Success(string bookingNumber)//Onay
        {
            if (string.IsNullOrWhiteSpace(bookingNumber))
                return RedirectToAction("Index", "Home");

            ViewBag.BookingNumber = bookingNumber;
            return View();
        }



        private async Task<BookingCreateViewModel?> BuildViewModelAsync(string tourDateId)
        {
            if (string.IsNullOrWhiteSpace(tourDateId)) return null;

            var tour = await _tourServices.GetByTourDateIdAsync(tourDateId);
            if (tour is null) return null;

            var date = tour.TourDates.FirstOrDefault(d => d.Id == tourDateId);
            if (date is null) return null;

            return new BookingCreateViewModel
            {
                TourId = tour.Id!,
                TourDateId = tourDateId,
                TourTitle = tour.Title.Tr ?? "",
                ThumbnailUrl = tour.ThumbnailUrl,
                StartDate = date.StartDate,
                EndDate = date.EndDate,
                Price = date.Price,
                CurrencyType = tour.CurrencyType,
                AvailableSeats = date.AvailableSeats,
                Day = tour.Day,
                Night = tour.Night
            };
        }
    }
}
