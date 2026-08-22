using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Oceanic_Horizon_Travel.DTOs.TourDtos;
using Oceanic_Horizon_Travel.Models;
using Oceanic_Horizon_Travel.Services.DestinationServices;
using Oceanic_Horizon_Travel.Services.MemberServices;
using Oceanic_Horizon_Travel.Services.ReviewServices;
using Oceanic_Horizon_Travel.Services.TourServices;

namespace Oceanic_Horizon_Travel.Controllers
{
    public class HomeController(
        ITourServices _tourServices,
        IDestinationServices _destinationServices,
        IReviewServices _reviewServices,
        IMemberServices _memberServices) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var (allTours, tourCount) = await _tourServices.GetFilteredAsync(new TourFilterDto());
            var destinations = await _destinationServices.GetAllAsync();
            var members = await _memberServices.GetAllAsync();
            var reviews = await _reviewServices.GetAllAsync("approved");

            var model = new HomeViewModel
            {
                FeaturedTours = await _tourServices.GetFeaturedAsync(6),
                Destinations = destinations.Where(x => x.IsActive).Take(4).ToList(),
                Reviews = reviews.Take(3).ToList(),

                TotalTours = tourCount,
                TotalDestinations = destinations.Count(x => x.IsActive),
                TotalMembers = members.Count
            };

            return View(model);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
