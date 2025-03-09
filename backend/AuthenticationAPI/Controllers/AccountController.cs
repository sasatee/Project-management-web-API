using AuthenticationAPI.DTOs;
using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

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

        public AccountController(
            UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
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
            var resetLink = $"http://localhost:4200/reset-password?email={user.Email}&token={WebUtility.UrlEncode(token)}";

            // Send the reset link via email
            var fromAddress = new MailAddress("sarvam2601@gmail.com", "Sasatee Support");
            var toAddress = new MailAddress(user.Email ?? throw new ArgumentNullException(nameof(user.Email)));
            const string fromPassword = "hkcf nufq zbrm heoq"; // Use a secure way to store and retrieve your password
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

            var token = GenerateToken(user);
            return Ok(new AuthResponseDto()
            {
                Token = token,
                Message = "Sucessfully Login.",
                isSuccess = true,
                Roles = role.ToList() // include roles in the response 



            });
        }
        private string GenerateToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII
                .GetBytes(_configuration.GetSection("JWTSetting").GetSection("securityKey").Value!); // private secret key

            var roles = _userManager.GetRolesAsync(user).Result;

            List<Claim> claims = new List<Claim>
            {
                 new (JwtRegisteredClaimNames.Email,user.Email ?? ""),
                new(JwtRegisteredClaimNames.Name,user.FirstName ?? ""),
                new(JwtRegisteredClaimNames.Name,user.LastName ?? ""),
                new(JwtRegisteredClaimNames.NameId,user.Id ?? ""),
                new(JwtRegisteredClaimNames.Aud,_configuration.GetSection("JWTSetting").GetSection("ValidAudience").Value!),
                new(JwtRegisteredClaimNames.Iss,_configuration.GetSection("JWTSetting").GetSection("ValidIssuer").Value!)

            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256



                    )


            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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

    }






}
