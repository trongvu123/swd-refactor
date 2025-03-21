
using SonicStore.Repository.Repository;
using System;
using System.Threading.Tasks;
using SonicStore.Common.Utils;
namespace SonicStore.Business.Service
{
    public class ForgotPasswordService :  IForgotPasswordService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly EmailService _emailService;
        private readonly EncriptPassword _encriptPassword;
        private readonly Random _random;

        public ForgotPasswordService(
            IUserRepository userRepository,
            IAccountRepository accountRepository,
            EmailService emailService,
            EncriptPassword encriptPassword)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _emailService = emailService;
            _encriptPassword = encriptPassword;
            _random = new Random();
        }

        public async Task<bool> SendOTPAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) return false;

            int otp = _random.Next(10000, 99999);
            // Lưu OTP vào cache hoặc database tạm thời
            // Ở đây giả định có một service lưu trữ OTP
            // OTPStorageService.StoreOTP(email, otp);

            return await _emailService.SendOTPEmail(email, otp);
        }

        public async Task<bool> VerifyOTPAsync(string email, int otp)
        {
            // Kiểm tra OTP từ cache hoặc database
            // Ở đây giả định có một service kiểm tra OTP
            // return OTPStorageService.VerifyOTP(email, otp);
            return true; // Giả lập kết quả xác thực thành công
        }

        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) return false;

            string hashedPassword = _encriptPassword.HashPassword(newPassword);
            return await _accountRepository.UpdatePasswordAsync(user.AccountId, hashedPassword);
        }
    }
}
