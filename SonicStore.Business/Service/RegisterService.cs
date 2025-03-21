using SonicStore.Areas.SonicStore.Dtos;
using SonicStore.Repository.Entity;
using SonicStore.Repository.Repository;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SonicStore.Common.Utils;

namespace SonicStore.Business.Service
{
    public class RegisterService : IRegisterService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserAddressRepository _userAddressRepository;
        private readonly EmailService _emailService;
        private readonly EncriptPassword _encriptPassword;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RegisterService(
            IUserRepository userRepository,
            IAccountRepository accountRepository,
            IUserAddressRepository userAddressRepository,
            EmailService emailService,
            EncriptPassword encriptPassword,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _userAddressRepository = userAddressRepository;
            _emailService = emailService;
            _encriptPassword = encriptPassword;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CheckExistingUserAsync(string email, string phone)
        {
            var existingUser = await _userRepository.GetUserByEmailOrPhoneAsync(email, phone);
            return existingUser != null;
        }

        public async Task<bool> SendRegistrationOTPAsync(string email)
        {
            Random random = new Random();
            int OTP = random.Next(10000, 99999);
            _httpContextAccessor.HttpContext.Session.SetInt32("OTP", OTP);

            return await _emailService.SendOTPEmail(email, OTP);
        }

        public bool VerifyRegistrationOTPAsync(string otp)
        {
            var storedOtp = _httpContextAccessor.HttpContext.Session.GetInt32("OTP");
            return int.TryParse(otp, out int parsedOtp) && parsedOtp == storedOtp;
        }

        public void StoreUserInfoInSession(CompositeViewModel model, string addressInput)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString("Username", model.AccountModel.Username);
            session.SetString("Password", _encriptPassword.HashPassword(model.AccountModel.Password));
            session.SetString("FullName", model.UserModel.FullName);
            session.SetString("Dob", model.UserModel.Dob.ToString("o"));
            session.SetString("Email", model.UserModel.Email);
            session.SetString("Phone", model.UserModel.Phone);
            session.SetString("Gender", model.UserModel.Gender);
            session.SetString("User_Address", addressInput);
        }

        public (string Username, string Password, string FullName, string DobString,
                string Email, string Phone, string Gender, string Address) GetUserInfoFromSession()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            return (
                Username: session.GetString("Username"),
                Password: session.GetString("Password"),
                FullName: session.GetString("FullName"),
                DobString: session.GetString("Dob"),
                Email: session.GetString("Email"),
                Phone: session.GetString("Phone"),
                Gender: session.GetString("Gender"),
                Address: session.GetString("User_Address")
            );
        }

        public async Task<bool> RegisterUserAsync(DateTime dob)
        {
            var userInfo = GetUserInfoFromSession();

            // Tạo tài khoản
            var account = new Account
            {
                Username = userInfo.Username,
                Password = userInfo.Password,
                RegisterDate = DateOnly.FromDateTime(DateTime.Now),
                Status = "on",
                ByAdmin = false
            };

            bool accountCreated = await _accountRepository.AddAccountAsync(account);
            if (!accountCreated) return false;

            // Tạo người dùng
            var user = new User
            {
                FullName = userInfo.FullName,
                Dob = dob,
                Email = userInfo.Email,
                Phone = userInfo.Phone,
                Gender = userInfo.Gender,
                UpdateDate = DateTime.Now,
                UpdateBy = 1,
                AccountId = account.Id,
                RoleId = 1
            };

            bool userCreated = await _userRepository.AddUserAsync(user);
            if (!userCreated) return false;

            // Tạo địa chỉ người dùng
            var userAddress = new UserAddress
            {
                User_Address = userInfo.Address,
                Status = true,
                UserId = user.Id
            };

            return await _userAddressRepository.AddUserAddressAsync(userAddress);
        }

        public void ClearSessionData()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }
    }
}
