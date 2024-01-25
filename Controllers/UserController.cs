using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Zonal_mvc.Models;
namespace Zonal_mvc.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        // Constructor
        public UserController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? message)
        {
            if (message is not null)
            {
                ViewData["message"] = message;
            }
            return View();
        }

        // redirect to microsoft login
        [AllowAnonymous]
        [HttpGet]
        public ChallengeResult ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action("RegisterExternalUser", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        // register external user
        [AllowAnonymous]
        public async Task<IActionResult> RegisterExternalUser(string? returnUrl = null, string? remoteError = null)
        {
            returnUrl ??= Url.Content("~/");
            var message = "";
            if (remoteError is not null)
            {
                message = $"Error from external provider: {remoteError}";
                return RedirectToAction("login", routeValues: new { message });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info is null)
            {
                message = "Error loading external login information";
                return RedirectToAction("login", routeValues: new { message });
            }

            var externalLoginResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (externalLoginResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            string email = "";
            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                email = info.Principal.FindFirstValue(ClaimTypes.Email)!;
            }
            else
            {
                message = "Email claim not received from: " + info.LoginProvider;
                return RedirectToAction("login", routeValues: new { message });
            }

            var user = new IdentityUser() { Email = email, UserName = email };

            var registerUserResult = await _userManager.CreateAsync(user);

            if (!registerUserResult.Succeeded)
            {
                message = registerUserResult.Errors.First().Description;
                return RedirectToAction("login", routeValues: new { message });
            }

            var addLoginResult = await _userManager.AddLoginAsync(user, info);

            if (addLoginResult.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }

            message = "An error occurred while adding external login";
            return RedirectToAction("login", routeValues: new { message });
        }

        // Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }

        // Login with credentials
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> LoginWithCredentials(LoginViewModel model, string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                {

                    // se efectua el login
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
                }
            }

            string message = "Credenciales erroneas";
            return RedirectToAction("login", routeValues: new { message });

        }
    }


}
