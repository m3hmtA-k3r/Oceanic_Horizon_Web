using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Oceanic_Horizon_Travel.DTOs.MemberDtos;
using Oceanic_Horizon_Travel.Services.MemberServices;
using System.Security.Claims;

namespace Oceanic_Horizon_Travel.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MemberController(IMemberServices _memberServices, IMapper _mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var members = await _memberServices.GetAllAsync();
            return View(members);
        }

        public async Task<IActionResult> Update(string id)
        {
            var member = await _memberServices.GetByIdAsync(id);
            var updateMember = _mapper.Map<UpdateMemberDto>(member);

            return View(updateMember);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateMemberDto updateMemberDto)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isSuperAdmin = User.IsInRole("SuperAdmin");

            var target = await _memberServices.GetByIdAsync(updateMemberDto.Id!);

            //Admin, süper yöneticiyi düzenleyemez
            if (target.Roles.Contains("SuperAdmin") && !isSuperAdmin)
            {
                return Forbid();
            }

            //Admin, kimseye yönetici yetkisi veremez
            if (!isSuperAdmin && updateMemberDto.Roles.Contains("Admin"))
            {
                ModelState.AddModelError(string.Empty, "Yönetici yetkisi vermek için süper yönetici olmalısınız.");
                return View(updateMemberDto);
            }

            //Süper yönetici rolü formdan gelmez — mevcut durumu koru
            if (target.Roles.Contains("SuperAdmin"))
            {
                updateMemberDto.Roles.Add("SuperAdmin");
            }

            //Kendi yetkini kaldıramazsın
            if (updateMemberDto.Id == currentUserId && !updateMemberDto.Roles.Contains("Admin") && !isSuperAdmin)
            {
                ModelState.AddModelError(string.Empty, "Kendi yönetici yetkinizi kaldıramazsınız.");
                return View(updateMemberDto);
            }

            //Kendi hesabını pasife alamazsın
            if (updateMemberDto.Id == currentUserId && !updateMemberDto.IsActive)
            {
                ModelState.AddModelError(string.Empty, "Kendi hesabınızı pasife alamazsınız.");
                return View(updateMemberDto);
            }

            await _memberServices.UpdateRolesAndStatusAsync(updateMemberDto);

            TempData["Success"] = "Üye bilgileri güncellendi.";
            return RedirectToAction(nameof(Index));
        }

    }
}
