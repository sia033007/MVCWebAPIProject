using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyMVCProject.Models;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MyMVCProject.Controllers
{

    public class IdentityUserController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<MyUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly UserManager<MyUser> _userManager;
        public IdentityUserController(UserManager<MyUser> userManager, IMapper mapper, SignInManager<MyUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _emailSender = emailSender;
                
        }
        public ViewResult RegisterUser() => View();
        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterViewModel registerViewModel)
        {
            if(registerViewModel == null)
            {
                return View(registerViewModel);
            }
            var user = _mapper.Map<MyUser>(registerViewModel);
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                    TempData["badrequest"] = "Something Went Wrong";
                }
                return View(registerViewModel);
            }
            List<Claim> claims = new List<Claim>
            {
                new Claim("FirstName", registerViewModel.FirstName?? string.Empty),
                new Claim("LastName", registerViewModel.LastName?? string.Empty),
                new Claim("City", registerViewModel.City?? string.Empty),
                new Claim("Profession", registerViewModel.Profession?? string.Empty)

            };
            await _userManager.AddClaimsAsync(user, claims);
            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.ActionLink(action: "ConfirmEmail", controller: "IdentityUser", values:
                new { userId = user.Id, token = confirmationToken });
            await _emailSender.SendAsync("aminkhosravi007@gmail.com", user.Email,
                "Confirm Your Email", $"Click the link to confirm your email : {confirmationLink}");
            TempData["success"] = "Registered Successfully, Now check your email to confirm your account";
            return RedirectToAction("LoginUser", "IdentityUser");
        }
        public ViewResult LoginUser() => View();
        [HttpPost]
        public async Task<IActionResult> LoginUser(LoginViewModel loginViewModel)
        {
            var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password,
                loginViewModel.RememberMe, false);
            if(!result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
                if(user == null)
                {
                    TempData["notfound"] = "Not found such a user";
                    ModelState.AddModelError("", "Not founds such a user");
                    return View(loginViewModel);
                }
                else if(!await _userManager.IsEmailConfirmedAsync(user))
                {
                    TempData["badrequest"] = "Email Not Confirmed";
                    return View(loginViewModel);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid UserName or Password");
                    TempData["badrequest"] = "Invalid UserName or Password";
                    return View(loginViewModel);
                }
            }
            TempData["success"] = "Logged In Successfully";
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> LogoutUser()
        {
            await _signInManager.SignOutAsync();
            TempData["success"] = "Logged out Successfully";
            return RedirectToAction("Index", "Home");
        }
        public ViewResult ForgotPassword() => View();
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);
            if(user == null)
            {
                ModelState.AddModelError("", "Not found such a user");
                TempData["notfound"] = "Not found such a user";
                return View(forgotPasswordViewModel);
            }
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.ActionLink(action: "ResetPassword", controller: "IdentityUser", values: new
            { userId = user.Id, token = resetToken });
            await _emailSender.SendAsync("aminkhosravi007@gmail.com", user.Email,
                "Reset Your Password", $"Click the link to reset your password : {resetLink}");
            TempData["success"] = "Reset password email was sent to your email address";
            return RedirectToAction("LoginUser", "IdentityUser");
        }
        public ViewResult ResetPassword(string userId, string token)
        {
            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                userId = userId,
                token = token,
                Password = string.Empty,
                ConfirmPassword = string.Empty
            };
            return View(resetPasswordViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (!ModelState.IsValid) return View(resetPasswordViewModel);
            var user = await _userManager.FindByIdAsync(resetPasswordViewModel.userId);
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.token,
                resetPasswordViewModel.Password);
            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    TempData["badrequest"] = "Something went wrong";
                    return View(resetPasswordViewModel);
                }
            }
            TempData["success"] = $"Password Reset Successfully for {user.UserName}";
            return RedirectToAction("LoginUser", "IdentityUser");
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                TempData["notfound"] = "Not found such a user";
                ModelState.AddModelError("", "Not found such a user");
                return View();
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    TempData["badrequest"] = "Something Went Wrong";
                }
                return View();
            }
            TempData["success"] = "Successfully Confirmed Email";
            return View();

        }
        private async Task<(MyUser, Claim, Claim, Claim, Claim)> GetUserInfo()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var claims = await _userManager.GetClaimsAsync(user);
            var firstNameClaim = claims.FirstOrDefault(c => c.Type == "FirstName");
            var lastNameClaim = claims.FirstOrDefault(c => c.Type == "LastName");
            var cityClaim = claims.FirstOrDefault(c => c.Type == "City");
            var professionClaim = claims.FirstOrDefault(c => c.Type == "Profession");
            return (user, firstNameClaim, lastNameClaim, cityClaim, professionClaim);

        }
        [Authorize]
        public async Task<IActionResult> UserProfile()
        {
            UserProfileViewModel profileModel = new UserProfileViewModel();
            var (user, firstNameClaim, lastNameClaim, cityClaim, professionClaim) = await GetUserInfo();
            profileModel.UserName = user.UserName;
            profileModel.Email = user.Email;
            profileModel.FirstName = firstNameClaim?.Value;
            profileModel.LastName = lastNameClaim?.Value;
            profileModel.City = cityClaim?.Value;
            profileModel.Profession = professionClaim?.Value;

            return View(profileModel);

        }
        [HttpPost]
        public async Task<IActionResult> UserProfile(UserProfileViewModel profileModel)
        {
            var (user, firstNameClaim, lastNameClaim, cityClaim, professionClaim) = await GetUserInfo();
            try
            {
                await _userManager.ReplaceClaimAsync(user, firstNameClaim, new Claim(firstNameClaim.Type, profileModel.FirstName));
                await _userManager.ReplaceClaimAsync(user, lastNameClaim, new Claim(lastNameClaim.Type, profileModel.LastName));
                await _userManager.ReplaceClaimAsync(user, cityClaim, new Claim(cityClaim.Type, profileModel.City));
                await _userManager.ReplaceClaimAsync(user, professionClaim, new Claim(professionClaim.Type, profileModel.Profession));
            }
            catch
            {
                ModelState.AddModelError("", "Something went wrong while saving data");
                TempData["badrequest"] = "Something went wrong";

            }
            TempData["success"] = "Successfully updated user profile";
            return View();
        }
    }
}
