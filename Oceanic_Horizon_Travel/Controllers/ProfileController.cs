using Microsoft.AspNetCore.Mvc;
using Oceanic_Horizon_Travel.DTOs.MemberDtos;
using Oceanic_Horizon_Travel.Services.BookingServices;
using Oceanic_Horizon_Travel.Services.MemberServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Oceanic_Horizon_Travel.Controllers
{
    public class ProfileController(IMemberServices _memberServices,IBookingServices _bookingServices) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var memberId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(string.IsNullOrEmpty(memberId))
            {
                return RedirectToAction("Login","Auth");
            }

            var member = await _memberServices.GetByIdAsync(memberId);
            if(member == null)
                        return NotFound();

            var model = new ProfileViewModel
            {
                Member = member,
                Bookings = await _bookingServices.GetByMemberAsync(memberId)
            };

            return View(model);
        }
    }
}
