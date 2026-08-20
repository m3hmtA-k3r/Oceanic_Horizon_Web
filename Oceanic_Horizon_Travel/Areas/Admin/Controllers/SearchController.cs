using Microsoft.AspNetCore.Mvc;
using Oceanic_Horizon_Travel.DTOs.SearchDtos;
using Oceanic_Horizon_Travel.Services.BookingServices;
using Oceanic_Horizon_Travel.Services.MemberServices;
using Oceanic_Horizon_Travel.Services.TourServices;

namespace Oceanic_Horizon_Travel.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchController(
        ITourServices _tourServices,
        IBookingServices _bookingServices,
        IMemberServices _memberServices) : Controller
    {
        public async Task<IActionResult> Index(string q)
        {
            var model = new SearchResultViewModel { Query = q ?? "" };

            if (!string.IsNullOrWhiteSpace(q))
            {
                model.Tours = await _tourServices.SearchAsync(q);
                model.Bookings = await _bookingServices.SearchAsync(q);
                model.Members = await _memberServices.SearchAsync(q);
            }

            return View(model);
        }
    }
}
