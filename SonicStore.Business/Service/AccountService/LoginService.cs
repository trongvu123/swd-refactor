using SonicStore.Repository.Entity;
using System.Threading.Tasks;
using SonicStore.Common.Utils;
using SonicStore.Repository.Repository.AccountRepo;
using SonicStore.Repository.Repository.UserRepo;
namespace SonicStore.Business.Service.AccountService
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly EncriptPassword _encriptPassword;

        public LoginService(
            IUserRepository userRepository,
            IAccountRepository accountRepository,
            EncriptPassword encriptPassword)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _encriptPassword = encriptPassword;
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            var account = await _accountRepository.GetAccountByUsernameAsync(username);
            if (account == null || account.Status != "on") return false;

            return _encriptPassword.VerifyPassword(password, account.Password);
        }

        public async Task<(bool success, string role, int userId)> ValidateUserAsync(string username, string password)
        {
            var account = await _accountRepository.GetAccountByUsernameAsync(username);
            if (account == null || account.Status != "on")
                return (false, string.Empty, 0);

            if (!_encriptPassword.VerifyPassword(password, account.Password))
                return (false, string.Empty, 0);

            var user = await _userRepository.GetUserByAccountIdAsync(account.Id);
            if (user == null) return (false, string.Empty, 0);

            string role = GetRoleName(user.RoleId);
            return (true, role, user.Id);
        }

        private string GetRoleName(int roleId)
        {
            return roleId switch
            {
                1 => "customer",
                2 => "saler",
                3 => "marketing",
                4 => "admin",
                _ => string.Empty
            };
        }
        public async Task<User> GetUserInfoAsync(int userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

    }
}
