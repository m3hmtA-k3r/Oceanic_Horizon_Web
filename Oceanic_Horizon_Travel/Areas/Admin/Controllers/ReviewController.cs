using Microsoft.AspNetCore.Mvc;
using Oceanic_Horizon_Travel.Services.ReviewServices;

namespace Oceanic_Horizon_Travel.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReviewController(IReviewServices _reviewServices) : Controller
    {
        // status: null | pending | approved
        public async Task<IActionResult> Index(string? status)
        {
            var reviews = await _reviewServices.GetAllAsync(status);

            ViewBag.CurrentStatus = status ?? "all";
            ViewBag.PendingCount = await _reviewServices.GetPendingCountAsync();

            return View(reviews);
        }

        public async Task<IActionResult> Approve(string id)
        {
            await _reviewServices.SetApprovalAsync(id, true);

            TempData["Success"] = "Yorum onaylandı ve yayınlandı.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Unapprove(string id)
        {
            await _reviewServices.SetApprovalAsync(id, false);

            TempData["Success"] = "Yorum yayından kaldırıldı.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _reviewServices.DeleteAsync(id);

            TempData["Success"] = "Yorum silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
