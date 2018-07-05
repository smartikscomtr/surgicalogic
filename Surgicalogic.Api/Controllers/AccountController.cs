using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.Account;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Surgicalogic.Common.Extensions;
using Surgicalogic.Services.Services;

namespace Surgicalogic.Api.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private IConfiguration _configuration;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration Configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = Configuration;
        }

        [Route("Account/GetToken")]
        public ActionResult GetToken()
        {
            var token = TokenService.GenerateToken("gktest@gmail.com");
            bool verified = TokenService.VerifyToken(token);
            return Ok(token);
        }

        [Route("Account/Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result == Microsoft.AspNetCore.Identity.SignInResult.Success)
            {
                var token = TokenService.GenerateToken(model.Email);
                return Ok(token);
            }
            else
            {
                return BadRequest();
            }
        }

        [Route("Account/Register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
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

                    var token = TokenService.GenerateToken(model.Email);
                    return Ok(token);
                }

            }

            // If we got this far, something failed, redisplay form
            return BadRequest();
        }

        [Route("Account/ForgotPassword")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
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
            return BadRequest();
        }

        [Route("Account/ResetPassword")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
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
        [ValidateAntiForgeryToken]
        public void LogOff()
        {
            //TODO: LogOff
        }
    }
}