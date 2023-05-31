using Microsoft.AspNetCore.Identity;

namespace AlunosApi.Services {
    public class AuthenticateService : IAuthenticate {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;


        public AuthenticateService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager) {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<bool> RegisterUser(string email, string password) {
            var addUser = new IdentityUser {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(addUser,password);

            if (result.Succeeded) {
                await _signInManager.SignInAsync(addUser,isPersistent:true);
            }

            return result.Succeeded;

        }

        public async Task<bool> Authenticate(string email, string password) {
            var result = await _signInManager.PasswordSignInAsync(email, password, true, lockoutOnFailure: false);
            return result.Succeeded;
        }

        public async Task Logout() {
            await _signInManager.SignOutAsync();
        }

    }
}
