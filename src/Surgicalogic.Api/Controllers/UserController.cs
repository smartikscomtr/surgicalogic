using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Smartiks.Framework.IO;
using Surgicalogic.Common.Settings;
using Surgicalogic.Contracts.Services;
using Surgicalogic.Contracts.Stores;
using Surgicalogic.Data.Entities;
using Surgicalogic.Model.CommonModel;
using Surgicalogic.Model.EntityModel;
using Surgicalogic.Model.ExportModel;
using Surgicalogic.Model.InputModel;
using Surgicalogic.Model.OutputModel;
using Surgicalogic.Model.User;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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

        [Route("User/Login")]
        [HttpPost]
        public async Task<object> Login([FromBody]LoginViewModel model)
        {
            var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email && r.IsActive);

            if (appUser != null)
            {
                // This doesn't count login failures towards user lockout
                // To enable password failures to trigger user lockout, change to shouldLockout: true
                var result = await _signInManager.PasswordSignInAsync(appUser, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return _tokenService.GenerateToken(model.Email, appUser);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Get user methode
        /// </summary>
        /// <returns>UserOutputModel list</returns>
        [Route("User/GetUsers")]
        [HttpPost]
        public async Task<ResultModel<UserOutputModel>> GetUsers()
        {
            var users = await _userStoreService.GetAsync<UserOutputModel>();

            foreach (var item in users.Result)
            {
                var email = (string)item.Email;
                var user = _userManager.Users.SingleOrDefault(r => r.Email == email && r.IsActive);
                item.IsAdmin = await _userManager.IsInRoleAsync(user, AppSettings.AdminRole);
            }

            return users;
        }
        
        [Route("User/ExcelExport")]
        public async Task<string> ExcelExport()
        {
            var parentDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var fileName = string.Format("Users_{0}.xlsx", Guid.NewGuid().ToString());

            FileStream fs = new FileStream(Path.Combine(parentDirectory, "Surgicalogic.Web", "static", fileName), FileMode.CreateNew);
            var excelService = new ExcelDocumentService();

            var items = await _userStoreService.GetExportAsync<UserExportModel>();

            excelService.Write(fs, "Worksheet", typeof(UserExportModel), items, System.Globalization.CultureInfo.CurrentCulture);

            return fileName;
        }
        /// <summary>
        /// Add user methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>UserOutputModel</returns>
        [Route("User/InsertUser")]
        [HttpPost]
        public async Task<ResultModel<UserOutputModel>> InsertUser([FromBody] UserInputModel item)
        {
            var userItem = new UserModel()
            {
                UserName = item.UserName,
                Email = item.Email,
            };

            var isDuplicateEmail = await _userStoreService.IsDuplicateEmail(item.Email, item.Id);

            if (isDuplicateEmail)
            {
                var errorResult = new ResultModel<UserOutputModel>
                {
                    Info = new Info
                    {
                        Succeeded = false,
                        InfoType = Model.Enum.InfoType.Error,
                        Message = Model.Enum.MessageType.CodeIsNotUnique
                    }
                };

                return errorResult;
            }

            var result =  await _userStoreService.InsertAndSaveAsync<UserOutputModel>(userItem);

            await SetUserRolesAsync(item.Email, item.IsAdmin);

            return result;
        }

        /// <summary>
        /// Remove user item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Int</returns>
        [Route("User/DeleteUser/{id:int}")]
        [HttpPost]
        public async Task<ResultModel<int>> DeleteUser(int id)
        {
            return await _userStoreService.DeleteByIdAsync(id);
        }

        /// <summary>
        /// Update User methode
        /// </summary>
        /// <param name="item"></param>
        /// <returns>PersonnelCategoryModel</returns>
        [Route("User/UpdateUser")]
        [HttpPost]
        public async Task<ResultModel<UserModel>> UpdateUser([FromBody] UserInputModel item)
        {
            var userItem = new UserModel()
            {
                Id = item.Id,
                UserName = item.UserName,
                Email = item.Email
            };

            var isDuplicateEmail = await _userStoreService.IsDuplicateEmail(item.Email, item.Id);

            if (isDuplicateEmail)
            {
                var errorResult = new ResultModel<UserModel>
                {
                    Info = new Info
                    {
                        Succeeded = false,
                        InfoType = Model.Enum.InfoType.Error,
                        Message = Model.Enum.MessageType.CodeIsNotUnique
                    }
                };

                return errorResult;
            }

            var updateResult = await _userStoreService.UpdateAndSaveAsync(userItem);

            await SetUserRolesAsync(item.Email, item.IsAdmin);

            return updateResult;
        }

        private async Task SetUserRolesAsync(string email, bool isAdmin)
        {
            var user = _userManager.Users.SingleOrDefault(r => r.Email == email);

            if (isAdmin)
            {
                var isAlreadyAdmin = await _userManager.IsInRoleAsync(user, AppSettings.AdminRole);

                if (!isAlreadyAdmin)
                {
                    await _userManager.AddToRoleAsync(user, AppSettings.AdminRole);
                    await _userManager.RemoveFromRoleAsync(user, AppSettings.MemberRole);
                }
            }
            else
            {
                var isMember = await _userManager.IsInRoleAsync(user, AppSettings.MemberRole);

                if (!isMember)
                {
                    await _userManager.AddToRoleAsync(user, AppSettings.MemberRole);
                    await _userManager.RemoveFromRoleAsync(user, AppSettings.AdminRole);
                }
            }

        }

        [Route("User/ForgotPassword")]
        [HttpPost]
        public async Task<ResultModel<User>> ForgotPassword([FromBody]ForgotPasswordViewModel model)
        {
            var result = new ResultModel<User>() { Info = new Info { Succeeded = false } };

            var user = _userManager.Users.SingleOrDefault(r => r.Email == model.Email && r.IsActive);

            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                return result;
            }

                //For more information on how to enable user confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                //Send an email with this link
                string code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "User", new { email = user.Email, code = code }, protocol: HttpContext.Request.Scheme);
                var resetUrl = AppSettings.WebSiteUrl + callbackUrl.Substring(callbackUrl.IndexOf("/User/") + 6, callbackUrl.Length - callbackUrl.IndexOf("/User/") - 6);

            //TODO: Send Email
            // await UserManager<User>.SendEmailAsync(user, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
            result.Result = user;
            result.Info.Succeeded = true;

            return result;
        }

        [Route("User/ResetPassword")]
        [HttpPost]
        public async Task<ResultModel<bool>> ResetPassword([FromBody]ResetPasswordViewModel model)
        {
            var returnResult = new ResultModel<bool> { Info = new Info { Succeeded = false } };

            var user = _userManager.Users.SingleOrDefault(r => r.Email == model.Email && r.IsActive);

            if (user == null)
            {
                returnResult.Info.Message = Model.Enum.MessageType.UserNotFound;
                return returnResult;
            }

            if (string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.ConfirmPassword) || !model.Password.Equals(model.ConfirmPassword))
            {
                returnResult.Info.Message = Model.Enum.MessageType.PasswordsDoNotMatch;
                return returnResult;
            }

            var code = model.Code.Replace(" ", "+");
            var result = await _userManager.ResetPasswordAsync(user, code, model.Password);

            if (result.Succeeded)
            {
                returnResult.Info.Succeeded = true;
            }
            else if (result.Errors.Any())
            {
                returnResult.Info.Message = Model.Enum.MessageType.InvalidCode;
                returnResult.Result = result.Errors.FirstOrDefault().Description;
            }

            return returnResult;
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

            if (user == null || !user.IsActive)
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