using System.Threading.Tasks;

namespace SonicStore.Business.Service
{
    public interface IForgotPasswordService 
    {
        Task<bool> SendOTPAsync(string email);
        Task<bool> VerifyOTPAsync(string email, int otp);
        Task<bool> ResetPasswordAsync(string email, string newPassword);
    }
}
