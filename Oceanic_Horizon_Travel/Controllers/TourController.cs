using Microsoft.AspNetCore.Mvc;
using Oceanic_Horizon_Travel.DTOs.TourDtos;
using Oceanic_Horizon_Travel.Services.CategoryServices;
using Oceanic_Horizon_Travel.Services.DestinationServices;
using Oceanic_Horizon_Travel.Services.QuestionServices;
using Oceanic_Horizon_Travel.Services.ReviewServices;
using Oceanic_Horizon_Travel.Services.TourServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Oceanic_Horizon_Travel.DTOs.QuestionDtos;
using Oceanic_Horizon_Travel.DTOs.ReviewDtos;

namespace Oceanic_Horizon_Travel.Controllers
{
    public class TourController(ITourServices _tourServices, IDestinationServices _destinationServices,ICategoryServices _CategoryServices,
                                IReviewServices _reviewService, IQuestionServices _questionServices) : Controller
    {
        public async Task<IActionResult> Index(TourFilterDto filter)
        {
            var (item, totalCount) = await _tourServices.GetFilteredAsync(filter);

            var model = new TourListViewModel
            {
                Tours = item,
                TotalCount = totalCount,
                Filter = filter,
                Destinations = await _destinationServices.GetAllAsync(),
                Categories = await _CategoryServices.GetAllCategoryAsync()
            };

            return View(model);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var tour = await _tourServices.GetBySeoUrlAsync(id); // id aslında SeoUrl — adres /Tour/Detail/kıbrıs-sokaklarinda biciminde

            if (tour == null)
                return NotFound();

            var model = new TourDetailViewModel
            {
                Tour = tour,
                Reviews = await _reviewService.GetApprovedByTourAsync(tour.Id!),
                RatingDistribution = await _reviewService.GetRatingDistributionAsync(tour.Id!),
                Questions = await _questionServices.GetApprovedByTourAsync(tour.Id!)
            };

            if (!string.IsNullOrEmpty(tour.DestinationId))
            {
                var destination = await _destinationServices.GetByIdAsync(tour.DestinationId);
                if (destination is not null)
                    model.DestinationName = $"{destination.City.Tr} — {destination.Country.Tr}";
            }

            if (!string.IsNullOrEmpty(tour.CategoryId))
            {
                var category = await _CategoryServices.GetByIdAsync(tour.CategoryId);
                if (category != null)
                    model.CategoryName = category.Name.Tr ?? "";
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReview(string seoUrl, string tourId, int rating, string comment)
        {
            var memberId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(memberId))
                return RedirectToAction("Login", "Auth");

            if (rating < 1 || rating > 5 || string.IsNullOrWhiteSpace(comment))
            {
                TempData["FormError"] = "Puan ve yorum alanları zorunlu.";
                return RedirectToAction(nameof(Detail), new { id = seoUrl });
            }

            await _reviewService.CreateAsync(new CreateReviewDto
            {
                MemberId = memberId,
                Type = "Tour",
                EntityId = tourId,
                Rating = rating,
                Comment = comment.Trim()
            });

            TempData["FormSuccess"] = "Değerlendirmen alındı. Onaylandıktan sonra yayınlanacak.";
            return RedirectToAction(nameof(Detail), new { id = seoUrl });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddQuestion(string seoUrl, string tourId, string text)
        {
            var memberId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(memberId))
                return RedirectToAction("Login", "Auth");

            if (string.IsNullOrWhiteSpace(text))
            {
                TempData["FormError"] = "Soru alanı boş bırakılamaz.";
                return RedirectToAction(nameof(Detail), new { id = seoUrl });
            }

            await _questionServices.CreateAsync(new CreateQuestionDto
            {
                TourId = tourId,
                MemberId = memberId,
                Text = text.Trim()
            });

            TempData["FormSuccess"] = "Sorun iletildi. Cevaplandığında bu sayfada görünecek.";
            return RedirectToAction(nameof(Detail), new { id = seoUrl });
        }


    }
}
