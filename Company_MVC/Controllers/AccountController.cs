using Company.DAL.Models;
using Company_MVC.Dtos;
using Company_MVC.Helpers;
using MailKit;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

using Microsoft.AspNetCore.Identity.UI.Services;



namespace Company_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailingService _mailingService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMailingService mailingService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailingService = mailingService;
        }

        #region SignUp

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto model)
        {

            if (ModelState.IsValid)  //server side Validation
            {

                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user is null)
                {
                    user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == model.Email.ToLower());
                    if (user is null)

                        //Register

                        user = new AppUser()
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            ISAgree = model.IsAgree
                        };

                        var result = await _userManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {
                            //Confirmition  To SendEmail

                            // Generate email confirmation token
                            var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                            // Create the confirmation URL
                            var confirmationUrl = Url.Action(
                                "ConfirmEmail",               // Action name
                                "Account",                    // Controller name
                                new { userId = user.Id, token = confirmEmailToken },
                                protocol: Request.Scheme
                            );

                          

                            // Create the email
                            var email = new Email()
                            {
                                To = user.Email,
                                Subject = "Confirm Your Email",
                                Body = $"Please confirm your email by clicking this link: <a href='{confirmationUrl}'>Click here</a>"
                            };

                            // Send the email
                            _mailingService.SendEmail(email);

                            return RedirectToAction(nameof(CheckYourEmail));

                            return RedirectToAction("SignIn");

                        }

                        foreach (var error in result.Errors)
                        {

                            ModelState.AddModelError("", error.Description);
                        }

                    }
                }

                ModelState.AddModelError("", "Invalid  SignUp !!");
                return View(model);
        }
           
        

        #endregion

        #region SignIn

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == model.Email.ToLower());
                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        // SignIn

                        // Token علشان مفضلش اسالة كل مرة انت مين فبعملة

                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                        if (result.Succeeded)
                            return RedirectToAction("Index", "Home");
                    }
                }
            }

            ModelState.AddModelError("", "Invalid Login !!");
            return View(model);
        }

         


        #endregion


        #region SignOut

        [HttpPost]
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }

        #endregion

        #region Forget Password

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == model.Email.ToLower());
                if (user is not null)
                {
                    //Generate Token 
                    var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                    //Create Url 
                    var passwordUrl = Url.Action(
                        "ResetPassword",              // Action name
                        "Account",                    // Controller
                        new { email = user.Email, token = resetPasswordToken }, // Route values
                        protocol: Request.Scheme      // << ده بيخلي الرابط absolute
                    );

                    // create email content and send it
                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = passwordUrl

                    };


                    //Send Email

                    _mailingService.SendEmail(email);

                    return RedirectToAction(nameof(CheckYourEmail));
                }
            }
            ModelState.AddModelError("", "Invalid Reset Password !!");
            return View("ForgetPassword");
        }


        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                var userName = user.UserName; // أو user.FirstName + user.LastName حسب نظامك

                ViewData["UserName"] = userName;
                return View("AccountActivated");
            }

            return View("Error");
        }




        public IActionResult CheckYourEmail()
        {
            return View();
        }



        public IActionResult AccountActivated()
        {
            return View();
        }


        #endregion


        #region Reset Password

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();

           
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;
                if (email is null || token is null) return BadRequest("Invalid Operation!");

                var user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }
                }
            }
            return View(model);
        }
        #endregion

        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Google Login
        //public IActionResult GoogleLogin()
        //{
        //    var prop = new AuthenticationProperties()
        //    {
        //        RedirectUri = Url.Action(nameof(GoogleResponse))
        //    };

        //    return Challenge(prop, GoogleDefaults.AuthenticationScheme);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GoogleResponse()
        //{
        //    var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        //    if (!result.Succeeded || result.Principal == null)
        //        return RedirectToAction("SignIn", "Account");

        //    // ✳️ امسح أي جلسة سابقة
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //    // ✳️ استخدم claims اللي جت من جوجل زي ما هي
        //    await HttpContext.SignInAsync(
        //        CookieAuthenticationDefaults.AuthenticationScheme,
        //        result.Principal,
        //        result.Properties);

        //    return RedirectToAction("Index", "Home");
        //}

        #endregion

    }
}

