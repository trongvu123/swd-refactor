using SonicStore.Areas.SonicStore.Dtos;
using System;
using System.Threading.Tasks;

namespace SonicStore.Business.Service
{
    public interface IRegisterService
    {
        Task<bool> CheckExistingUserAsync(string email, string phone);
        Task<bool> SendRegistrationOTPAsync(string email);
        bool VerifyRegistrationOTPAsync(string otp);
        void StoreUserInfoInSession(CompositeViewModel model, string addressInput);
        (string Username, string Password, string FullName, string DobString,
         string Email, string Phone, string Gender, string Address) GetUserInfoFromSession();
        Task<bool> RegisterUserAsync(DateTime dob);
        void ClearSessionData();
    }
}
