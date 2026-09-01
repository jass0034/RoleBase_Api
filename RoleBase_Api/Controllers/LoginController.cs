using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RoleBase_Api.Data;
using RoleBase_Api.Jwt;
using RoleBase_Api.Models;
using RoleBase_Api.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RoleBase_Api.Controllers
{
    [Route("api/Login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly Appsettings _appsettings;
        private readonly TwilioSmsService _twilioSmsService;
        public LoginController(ApplicationDbContext context, IOptions<Appsettings> appSettings, TwilioSmsService twilioSmsService)
        {
            _context = context;
            _appsettings = appSettings.Value;
            _twilioSmsService = twilioSmsService;
        }

        [HttpPost("sendOtp")]
        public async Task<IActionResult> SendOtp(SendOtpDTO dto)
        {
            var user = _context.Users.FirstOrDefault(x => x.Name == dto.Name && x.Email == dto.Email && x.MobileNumber == dto.MobileNumber);
            if (user == null)
            {
                return NotFound("User not found");
            }
            Random random = new Random();
            string otp = random.Next(100000, 999999).ToString();
            UserOTP userOTP = new UserOTP()
            {
                UserId = user.Id,
                OTP = otp,
                ExpiryTime = DateTime.UtcNow.AddSeconds(30),
                IsUsed = false
            };
            _context.UserOTPs.Add(userOTP);
            _context.SaveChanges();
            await _twilioSmsService.SendSmsAsync(user.MobileNumber, otp);
            return Ok(new
            {
                Message = "OTP sent successfully",
            });
        }

        [HttpPost("verifyOtp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpDTO dto)
        {
            var user = await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.MobileNumber == dto.MobileNumber);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var otpRecord = _context.UserOTPs.Where
                (x => x.UserId == user.Id && x.OTP == dto.OTP && x.IsUsed == false).
                OrderByDescending(x => x.Id).FirstOrDefault();
            if (otpRecord == null)
            {
                return NotFound("Invalid OTP");
            }
            if (otpRecord.ExpiryTime < DateTime.UtcNow)
            {
                return BadRequest("OTP expired");
            }
            otpRecord.IsUsed = true;
            await _context.SaveChangesAsync();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appsettings.Secret);
            var expiresAt = DateTime.UtcNow.AddMinutes(30);
            var tokenDescritor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.Name),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.MobilePhone,user.MobileNumber),
                    new Claim(ClaimTypes.Role,user.Role.Name)

                }),
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescritor);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new
            {
                token = jwtToken,
                UserName = user.Name,
                expiresAt = expiresAt,
                Role = user.Role.Name
            });
        }
    }
}