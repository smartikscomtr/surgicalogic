using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Surgicalogic.Contracts.Services;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.OutputModel;
using Surgicalogic.Model.User;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Surgicalogic.Api.Controllers
{
    public class UserController : Controller
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private ITokenService _tokenService;
        private IUserStoreService _userStoreService;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService, IUserStoreService userStoreService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _userStoreService = userStoreService;
        }

        [Route("User/GetUsers")]
        [HttpPost]
        public async Task<ResultModel<UserOutputModel>> GetUsers()
        {
            return await _userStoreService.GetAsync<UserOutputModel>();
        }

        [Route("User/Login")]
        [HttpPost]
        public async Task<object> Login([FromBody]LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // This doesn't count login failures towards user lockout
            // To enable password failures to trigger user lockout, change to shouldLockout: true
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                return _tokenService.GenerateToken(model.Email, appUser);
            }

            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
        }


        [Route("User/InsertUser")]
        [HttpPost]
        public async Task<ActionResult> InsertUser([FromBody]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // For more information on how to enable user confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "User", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your user", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    var token = _tokenService.GenerateToken(model.Email, user);
                    return Ok(token);
                }
            }

            // If we got this far, something failed, redisplay form
            throw new ApplicationException("UNKNOWN_ERROR");
        }

        [Route("Login/ForgotPassword")]
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

                // For more information on how to enable user confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "User", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "User");
            }

            // If we got this far, something failed, redisplay form
            throw new ApplicationException("UNKNOWN_ERROR");
        }

        [Route("User/ResetPassword")]
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
                return RedirectToAction("ResetPasswordConfirmation", "User");
            }

            return View();
        }


        [Route("User/LogOff")]
        [HttpPost]
        public async Task LogOff()
        {
            await _signInManager.SignOutAsync();
        }

        [Route("User/RefreshToken")]
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