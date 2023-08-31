using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SendReceiptsDemo.Data.Repositories;
using SendReceiptsDemo.Models;
using System.Security.Claims;

namespace SendReceiptsDemo.Controllers
{
    public class AccountController : Controller
    {
        private IAdminRep _adminRep;
        public AccountController(IAdminRep adminRep)
        {
            _adminRep = adminRep;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            var admin = _adminRep.GetAdminForLogin(loginVM.UserName.ToLower(), loginVM.Password);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, admin.AdminId.ToString()),
                new Claim(ClaimTypes.Name, admin.UserName),
                new Claim("AdminType",admin.AdminType)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties
            {
                IsPersistent = loginVM.RememberMe
            };

            HttpContext.SignInAsync(principal, properties);

            return Redirect("/admin");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Account/Login");
        }

        public IActionResult CheckPassword(string Password, string UserName) // an Action that remoted for check the field value validation (no need to post page)
        {
            if (_adminRep.CheckPassword(UserName.ToLower(), Password))
            {
                return Json(true); // send true value
            }
            else return Json("اطلاعات وارد شده نادرست است"); //send error text
        }
    }
}
