using AuthenticationAPI.DTOs;
using AuthenticationAPI.Models;
using AuthenticationAPI.util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

namespace AuthenticationAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    //api/account
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly GenerateToken _generateToken;

        public AccountController(
            UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, GenerateToken generateToken)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _generateToken = generateToken;
        }


        //api/account/register
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new AppUser
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            if (registerDto.Roles is null)
            {
                await _userManager.AddToRoleAsync(user, "User"); //set role as user 
            }
            else
            {
                foreach (var role in registerDto.Roles)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }
            return Ok(new AuthResponseDto
            {
                isSuccess = true,
                Message = "Account Created Sucessfully"
            });
        }



        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDto forgotPassword)
        {
            var user = await _userManager.FindByEmailAsync(forgotPassword.Email);

            if (user is null)
            {
                return Ok(
                    new AuthResponseDto
                    {
                        isSuccess = false,
                        Message = $"User does not exist with this {forgotPassword.Email}"
                    });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetUrl = _configuration.GetSection("EmailSetting").GetSection("ResetUrl").Value!; //http://localhost:4200/reset-password?email=
            var resetLink = $"{resetUrl}{user.Email}&token={WebUtility.UrlEncode(token)}";

            var password = _configuration.GetSection("EmailSetting").GetSection("password").Value!;
            var emailSupport = _configuration.GetSection("EmailSetting").GetSection("supportEmail").Value!;

            // Send the reset link via email
            var fromAddress = new MailAddress(emailSupport, "Sasatee Support");
            var toAddress = new MailAddress(user.Email ?? throw new ArgumentNullException(nameof(user.Email)));
            string fromPassword = password; // Use a secure way to store and retrieve your password
            const string subject = "Password Reset";
            string body = $"Please reset your password using the following link: {resetLink}";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            bool emailSent = false;
            smtp.SendCompleted += (s, e) => { emailSent = e.Error == null; };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                await smtp.SendMailAsync(message);
            }

            if (emailSent)
            {
                return Ok(new AuthResponseDto
                {
                    isSuccess = true,
                    Message = "Password reset link has been sent to your email."
                });
            }
            else
            {
                return BadRequest(new AuthResponseDto { Message = "Email failed to send", isSuccess = false });
            }
        }



        //api/account/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByEmailAsync(loginDto.Email); // get email

            if (user is null) // if user obj (email) is null
            {
                return Unauthorized(new AuthResponseDto
                {
                    isSuccess = false,
                    Message = "User not found with this email"
                });
            }
            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password); // verify password authenticity
            var role = await _userManager.GetRolesAsync(user);

            //if password not valid
            if (!result)
            {
                return Unauthorized(new AuthResponseDto { isSuccess = false, Message = "Invalid password" });
            }

            var token = _generateToken.GenerateTokenWithClaim(user);
            return Ok(new AuthResponseDto()
            {
                Token = token,
                Message = "Sucessfully Login.",
                isSuccess = true,
                Roles = role.ToList() // include roles in the response 
            });
        }



        //api/account/detail
        [HttpGet("detail")]
        public async Task<ActionResult<UserDetailDto>> GetUserDetail()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(currentUserId!);
            if (user is null)
            {
                return NotFound(new AuthResponseDto
                {
                    isSuccess = false,
                    Message = "User not found"
                });
            }
            return Ok(new UserDetailDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = [.. await _userManager.GetRolesAsync(user)],
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                AccessFailedCount = user.AccessFailedCount,
            });
        }



        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(changePasswordDto.Email);

            if (user is null)
            {
                return BadRequest(new AuthResponseDto
                {
                    isSuccess = false,
                    Message = $"User does not exist this email: {changePasswordDto.Email}"
                });
            }
            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.ConfirmNewPassword);

            if (result.Succeeded)
            {
                return Ok(new AuthResponseDto { isSuccess = true, Message = "Password changed successfully" });
            }
            else
            {
                return BadRequest(new AuthResponseDto { isSuccess = false, Message = "Password has not been changed" });
            }
        }



        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword(ResetPassowrdDto resetpasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetpasswordDto.Email);
            resetpasswordDto.Token = WebUtility.UrlDecode(resetpasswordDto.Token);

            if (user is null)
            {
                return BadRequest(new AuthResponseDto { isSuccess = false, Message = $"User does not exist with this email: {resetpasswordDto.Email}" });
            }

            var result = await _userManager.ResetPasswordAsync(user, resetpasswordDto.Token, resetpasswordDto.NewPassword);

            if (result.Succeeded)
            {
                return Ok(new AuthResponseDto { isSuccess = false, Message = "Password successfully reset" });
            }

            return BadRequest(new AuthResponseDto { isSuccess = false, Message = result.Errors.FirstOrDefault()!.Description });
        }



        /// <summary>
        /// Retrieves a list of all users with their details.
        /// </summary>
        /// <remarks>
        /// Admin has authorized access to check all available accounts.
        /// </remarks>
        /// <response code="200">Returns the list of all users.</response>
        /// <response code="403">Forbidden. Only Admin has access to this resource.</response>
        [HttpGet("details")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<UserDetailDto>>> GetAllUsers()
        {
            //check if user is in the Admin role
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin)
            {
                return Forbid();
            }
            var users = await _userManager.Users.Select(u => new UserDetailDto
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName
            }).ToListAsync();

            //fetch roles for each user after receiving the users 
            foreach (var user in users)
            {
                var appUser = await _userManager.FindByIdAsync(user.Id!);
                user.Roles = (await _userManager.GetRolesAsync(appUser!)).ToArray();
            }

            return Ok(new
            {
                Message = "Admin has authorized to check all accounts available",
                Users = users
            });
        }



        //api/account/google-login
        [AllowAnonymous]
        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleResponse))
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }



        //api/account/google-response
        [AllowAnonymous]
        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var authResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!authResult.Succeeded)
            {
                return BadRequest(new AuthResponseDto { isSuccess = false, Message = "Google authentication failed" });
            }

            var claims = authResult.Principal.Claims.ToList();
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var firstName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
            var lastName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
            var picture = claims.FirstOrDefault(c => c.Type == "picture")?.Value;
            var googleId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new AuthResponseDto { isSuccess = false, Message = "Email not found in Google response" });
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new AppUser
                {
                    Email = email,
                    UserName = email,
                    FirstName = firstName,
                    LastName = lastName,
                    GoogleId = googleId,
                    GooglePicture = picture,
                    EmailConfirmed = true
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    return BadRequest(new AuthResponseDto { isSuccess = false, Message = "Failed to create user account" });
                }

                //  default role "User" and "EMPLOYEE" 
                await _userManager.AddToRoleAsync(user, "ADMIN");
                await _userManager.AddToRoleAsync(user, "EMPLOYEE");
            }
            else
            {
                user.GoogleId = googleId;
                user.GooglePicture = picture;
                await _userManager.UpdateAsync(user);
            }

            var token = _generateToken.GenerateTokenWithClaim(user);
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new AuthResponseDto
            {
                Token = token,
                Message = "Successfully logged in with Google",
                isSuccess = true,
                Roles = roles.ToList()
            });
        }
    }






}
