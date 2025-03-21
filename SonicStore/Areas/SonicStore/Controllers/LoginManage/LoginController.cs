using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using SonicStore.Repository.Entity;
using SonicStore.Business.Service.AccountService;

namespace SonicStore.Areas.SonicStore.Controllers.LoginManage
{
    [Area("SonicStore")]
    [Route("[area]/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpGet("login")]
        public IActionResult Index(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Index(Account account, string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                var (success, role, userId) = await _loginService.ValidateUserAsync(account.Username, account.Password);

                if (!success)
                {
                    ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không đúng.");
                    return View(account);
                }

                // Lấy thông tin user từ repository
                var user = await _loginService.GetUserInfoAsync(userId);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Không thể lấy thông tin người dùng.");
                    return View(account);
                }

                // Tạo claims cho authentication
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.Username),
                    new Claim(ClaimTypes.Role, role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);

                // Lưu thông tin user vào session
                var serializerSettings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                var userJson = JsonConvert.SerializeObject(user, serializerSettings);
                HttpContext.Session.SetString("user", userJson);

                // Chuyển hướng dựa trên role
                switch (user.RoleId)
                {
                    case 2:
                        return RedirectToAction("SaleScreen", "Sale");
                    case 3:
                        return RedirectToAction("PromotionScreen", "Promotion");
                    case 4:
                        return RedirectToAction("AdminDashboardScreen", "AdminDashboard", new { Area = "SonicStore" });
                    default:
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return Redirect("/");
                }
            }

            return View(account);
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync();
            return RedirectToAction("HomeScreen", "Home", new { area = "SonicStore" });
        }
    }
}
