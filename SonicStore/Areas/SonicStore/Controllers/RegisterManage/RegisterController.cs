using Microsoft.AspNetCore.Mvc;
using SonicStore.Areas.SonicStore.Dtos;
using SonicStore.Business.Service.AccountService;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace SonicStore.Areas.SonicStore.Controllers.RegisterManage
{
    [Area("SonicStore")]
    public class RegisterController : Controller
    {
        private readonly IRegisterService _registerService;

        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] CompositeViewModel model, [FromForm] string tinh, [FromForm] string huyen, [FromForm] string xa)
        {
            if (!ModelState.IsValid)
            {
                TempData["StatusMessage"] = "Đăng ký không thành công do dữ liệu không hợp lệ!";
                return RedirectToAction("Success");
            }

            try
            {
                string addressInput = $"{xa}, {huyen}, {tinh}";

                // Kiểm tra user đã tồn tại
                bool userExists = await _registerService.CheckExistingUserAsync(model.UserModel.Email, model.UserModel.Phone);
                if (userExists)
                {
                    TempData["StatusMessage"] = "Số điện thoại hoặc Email đã tồn tại!";
                    return RedirectToAction("Success");
                }

                // Lưu thông tin vào session
                _registerService.StoreUserInfoInSession(model, addressInput);

                // Gửi OTP
                bool emailSent = await _registerService.SendRegistrationOTPAsync(model.UserModel.Email);
                if (!emailSent)
                {
                    TempData["StatusMessage"] = "Không thể gửi OTP. Vui lòng thử lại sau.";
                    return RedirectToAction("Success");
                }

                TempData["StatusMessage"] = "OTP đã được gửi đến email của bạn. Vui lòng kiểm tra email và nhập mã OTP để xác nhận!";
                return RedirectToAction("VerifyOTP");
            }
            catch (Exception ex)
            {
                TempData["StatusMessage"] = "Đăng ký không thành công: " + ex.Message;
                return RedirectToAction("Success");
            }
        }

        [HttpGet("verify-otp")]
        public IActionResult VerifyOTP()
        {
            return View();
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOTP(string otp)
        {
            try
            {
                bool isValidOTP = _registerService.VerifyRegistrationOTPAsync(otp);
                if (isValidOTP)
                {
                    // Lấy thông tin từ session
                    var userInfo = _registerService.GetUserInfoFromSession();

                    if (DateTime.TryParse(userInfo.DobString, null, DateTimeStyles.RoundtripKind, out DateTime dob))
                    {
                        // Tạo tài khoản và người dùng
                        bool accountCreated = await _registerService.RegisterUserAsync(dob);

                        if (accountCreated)
                        {
                            _registerService.ClearSessionData();
                            TempData["SignupSuccess"] = true;
                            TempData["StatusMessage"] = "Đăng ký thành công!";
                            return RedirectToAction("Success");
                        }
                    }
                    else
                    {
                        TempData["StatusMessage"] = "Ngày sinh không hợp lệ.";
                        return RedirectToAction("Success");
                    }
                }

                TempData["StatusMessage"] = "OTP không hợp lệ";
                return RedirectToAction("VerifyOTP");
            }
            catch (Exception ex)
            {
                TempData["StatusMessage"] = "Xác minh OTP không thành công: " + ex.Message;
                return RedirectToAction("Register");
            }
        }

        [HttpGet("success")]
        public IActionResult Success()
        {
            return View();
        }
    }
}
