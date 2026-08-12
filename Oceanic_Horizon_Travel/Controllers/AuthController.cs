using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Oceanic_Horizon_Travel.DTOs.MemberDtos;
using Oceanic_Horizon_Travel.Services.MemberServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Oceanic_Horizon_Travel.Controllers
{
    public class AuthController : Controller
    {
        private readonly IMemberServices _memberServise;

        public AuthController(IMemberServices memberServise)
        {
            _memberServise = memberServise;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterMemberDto registerMemberDto)
        {
            // FluentValidation Kurallarını Bunda devreye girecek
            if (!ModelState.IsValid)
                return View(registerMemberDto);

            var isMailExist = await _memberServise.IsEmailExistAsync(registerMemberDto.Email);

            if (isMailExist)
            {
                ModelState.AddModelError(string.Empty, "Bu E-Posta daha önce kullanıldı");
                return View(registerMemberDto);
            }

            await _memberServise.RegisterAsync(registerMemberDto);

            return RedirectToAction("Login");

        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginMemberDto loginMemberDto)
        {

            var member = await _memberServise.LoginAsync(loginMemberDto);

            if(member == null)
            {
                ModelState.AddModelError(string.Empty, "Bu E-Posta veya Şifre Hatalı");
                return View(loginMemberDto);
            }

            // Cookiede hangi bilgiler olacak

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, member.Id),
                new Claim(ClaimTypes.Name, member.FirstName + " " + member.LastName),
                new Claim(ClaimTypes.Email, member.Email)
            };

            // Rolüde Cereze yazalım
            foreach(var role in member.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

    }
}





