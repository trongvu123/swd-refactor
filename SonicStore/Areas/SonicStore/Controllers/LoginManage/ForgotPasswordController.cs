using Microsoft.AspNetCore.Mvc;
using SonicStore.Business.Service;
using System;
using System.Threading.Tasks;

namespace SonicStore.Areas.SonicStore.Controllers.LoginManage
{
    [Area("SonicStore")]
    [Route("[area]/[controller]")]
    public class ForgotPasswordController : Controller
    {
        private readonly IForgotPasswordService _forgotPasswordService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ForgotPasswordController(
            IForgotPasswordService forgotPasswordService,
            IHttpContextAccessor httpContextAccessor)
        {
            _forgotPasswordService = forgotPasswordService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email)
        {
            bool result = await _forgotPasswordService.SendOTPAsync(email);

            if (!result)
            {
                // Không hiển thị thông báo email không tồn tại vì lý do bảo mật
                TempData["StatusMessage"] = "Nếu email tồn tại, OTP sẽ được gửi đến email của bạn.";
                return RedirectToAction("Index");
            }

            HttpContext.Session.SetString("Email", email);
            TempData["StatusMessage"] = "OTP đã được gửi đến email của bạn. Vui lòng kiểm tra email và nhập mã OTP để xác nhận!";
            return RedirectToAction("VerifyOTP");
        }

        [HttpGet("verify-otp")]
        public IActionResult VerifyOTP()
        {
            return View();
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOTP(string otp)
        {
            string email = HttpContext.Session.GetString("Email");

            if (string.IsNullOrEmpty(email))
            {
                TempData["StatusMessage"] = "Phiên làm việc đã hết hạn. Vui lòng thử lại.";
                return RedirectToAction("Index");
            }

            bool isValid = await _forgotPasswordService.VerifyOTPAsync(email, int.Parse(otp));

            if (isValid)
            {
                TempData["StatusMessage"] = "OTP hợp lệ. Vui lòng nhập mật khẩu mới.";
                return RedirectToAction("ResetPassword");
            }

            TempData["StatusMessage"] = "OTP không hợp lệ.";
            return RedirectToAction("VerifyOTP");
        }

        [HttpGet("reset-password")]
        public IActionResult ResetPassword()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Email")))
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string newPassword, string confirmPassword)
        {
            string email = HttpContext.Session.GetString("Email");

            if (string.IsNullOrEmpty(email))
            {
                TempData["StatusMessage"] = "Phiên làm việc đã hết hạn. Vui lòng thử lại.";
                return RedirectToAction("Index");
            }

            if (newPassword != confirmPassword)
            {
                TempData["StatusMessage"] = "Mật khẩu không khớp. Vui lòng thử lại.";
                return RedirectToAction("ResetPassword");
            }

            bool result = await _forgotPasswordService.ResetPasswordAsync(email, newPassword);

            if (!result)
            {
                TempData["StatusMessage"] = "Đã xảy ra lỗi. Vui lòng thử lại.";
                return RedirectToAction("ResetPassword");
            }

            HttpContext.Session.Clear();
            TempData["StatusMessage"] = "Đặt lại mật khẩu thành công.";
            return RedirectToAction("Success");
        }

        [HttpGet("success")]
        public IActionResult Success()
        {
            return View();
        }
    }
}
