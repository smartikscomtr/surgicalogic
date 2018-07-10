using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Contracts.Services;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.Account;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Surgicalogic.Api.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private ITokenService _tokenService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [Route("Account/Login")]
        [HttpPost]
        public async Task<object> Login([FromBody]LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                return _tokenService.GenerateToken(model.Email, appUser);
            }

            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
        }

        [Route("Account/Register")]
        [HttpPost]
        public async Task<ActionResult> Register([FromBody]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    var token = _tokenService.GenerateToken(model.Email, user);
                    return Ok(token);
                }
            }

            // If we got this far, something failed, redisplay form
            throw new ApplicationException("UNKNOWN_ERROR");
        }

        [Route("Account/ForgotPassword")]
        [HttpPost]
        public async Task<ActionResult> ForgotPassword([FromBody]ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);

                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            throw new ApplicationException("UNKNOWN_ERROR");
        }

        [Route("Account/ResetPassword")]
        [HttpPost]
        public async Task<ActionResult> ResetPassword([FromBody]ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                // Don't reveal that the user does not exist
                return BadRequest();
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            return View();
        }


        [Route("Account/LogOff")]
        [HttpPost]
        public async Task LogOff()
        {
            await _signInManager.SignOutAsync();
        }

        [Route("Account/RefreshToken")]
        [HttpPost]
        public async Task<object> RefreshToken([FromBody]TokenViewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Token) || string.IsNullOrEmpty(model.RefreshToken))
            {
                return BadRequest();
            }

            var email = _tokenService.GetEmailFromExpiredToken(model.Token);
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BadRequest();
            }

            var savedRefreshToken = _tokenService.GetRefreshToken(user.Email, model.Token);

            if (savedRefreshToken != model.RefreshToken)
            {
                return BadRequest();
            }

            return _tokenService.GenerateToken(user.Email, user);
        }
    }
}