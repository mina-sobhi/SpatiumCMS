//using DemoIdentity.DTO;
//using DemoIdentity.Model;
//using Domain.Interfaces;
//using Infrastructure.Database.Database;
//using Infrastructure.Services.AuthinticationService;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Net;
//using System.Security.Claims;
//using System.Text;
//using Utilities.Helpers;

//namespace Spatium_CMS.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class SpatiumController : ControllerBase
//    {
//        private readonly UserManager<AppUser> userManager;
//        private readonly RoleManager<AppRole> roleManager;
//        private readonly IConfiguration configuration;
//        private readonly ISendMailService mailService;
//        private readonly SpatiumDbContent context;
//        private readonly AuthConfig jwtSetting;

//        public SpatiumController(UserManager<AppUser> userManager,
//            RoleManager<AppRole> roleManager,
//            IConfiguration configuration, ISendMailService mailService, SpatiumDbContent context,
//            IOptions<AuthConfig> jwtSetting)
//        {
//            this.userManager = userManager;
//            this.roleManager = roleManager;
//            this.configuration = configuration;
//            this.mailService = mailService;
//            this.context = context;
//            this.jwtSetting = jwtSetting.Value;
//        }
//        [HttpPost("Register-F1")]
//        public async Task<IActionResult> Register(SpatuimDTORefgister registerDto)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    AppUser user = new AppUser();
//                    user.FullName = registerDto.FullName;
//                    user.UserName = registerDto.Email;
//                    user.NormalizedUserName = registerDto.Email.ToUpper();
//                    user.Email = registerDto.Email;
//                    user.NormalizedEmail = registerDto.Email.ToUpper();

//                    user.EmailConfirmed = false;
//                    user.OTP = OTPGenerator.GenerateOTP();
//                    user.OTPGeneratedAt = DateTime.Now;
//                    var role = roleManager.Roles.FirstOrDefault();
//                    IdentityResult result = await userManager.
//                      CreateAsync(user, registerDto.Password);

//                    IdentityResult r = await userManager.AddToRoleAsync(user, role.Name);


//                    await context.SaveChangesAsync();

//                    if (result.Succeeded && r.Succeeded)
//                    {
//                        bool flag = await mailService.SendMail(user.Email, "SpatuimSW OTP", user.OTP);
//                        if (flag)
//                        {
//                            return Ok($"Go to Your Mail {user.Email}");
//                        }
//                        else
//                        {
//                            return BadRequest();
//                        }

//                    }
//                    else
//                    {
//                        return BadRequest(result.Errors.FirstOrDefault());
//                    }
//                }
//                return BadRequest(ModelState);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);

//            }

//        }


//        [HttpPost("Register-F2")] // Confirm otp when register & forget password 
//        public async Task<IActionResult> ConfirmOTP(ConfirmOTP confirmOTP)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    var modeluser = await userManager.FindByEmailAsync(confirmOTP.Email);
//                    if (modeluser == null)
//                    {
//                        return BadRequest("Invalid User");
//                    }
//                    var validto = modeluser.OTPGeneratedAt.Value.AddMinutes(20);
//                    if (DateTime.Now > validto)
//                    {
//                        return BadRequest("Invalid Time ");
//                    }
//                    if (modeluser.OTP != confirmOTP.OTP)
//                    {
//                        return BadRequest("Invalid OTP ");

//                    }

//                    modeluser.OTP = string.Empty;
//                    modeluser.OTPGeneratedAt = DateTime.MinValue;
//                    modeluser.EmailConfirmed = true;

//                    await context.SaveChangesAsync();
//                    return Ok("User Confimed Successfuly");


//                }
//                else
//                {
//                    return BadRequest(ModelState);
//                }
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }


//        [HttpPost("ResendOTP")]
//        public async Task<IActionResult> ResendOTP(ResendOTPDto resendOTPDto)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    var modeluser = await userManager.FindByEmailAsync(resendOTPDto.Email);
//                    if (modeluser == null || modeluser.EmailConfirmed == true)
//                    {
//                        return BadRequest("Invalid User");
//                    }
//                    modeluser.OTP = OTPGenerator.GenerateOTP();
//                    modeluser.OTPGeneratedAt = DateTime.Now;
//                    await context.SaveChangesAsync();
//                    bool flag = await mailService.SendMail(modeluser.Email, "SpatuimSW OTP", modeluser.OTP);
//                    if (flag)
//                    {
//                        return Ok($"Go to Your Mail {modeluser.Email}");
//                    }
//                    else
//                    {
//                        return BadRequest();
//                    }
//                }
//                else
//                {
//                    return BadRequest(ModelState);
//                }
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        [HttpPost("Login")]
//        public async Task<IActionResult> Login(LoginSpatuimDTO loginSpatuim)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    var modeluser = await userManager.FindByEmailAsync(loginSpatuim.Email);
//                    if (modeluser != null || modeluser.EmailConfirmed == false)
//                    {
//                        if (await userManager.CheckPasswordAsync(modeluser, loginSpatuim.Password))
//                        {


//                            List<Claim> myClaims = new List<Claim>();
//                            // myClaims.Add(new Claim(ClaimTypes.NameIdentifier, modeluser.));
//                            myClaims.Add(new Claim(ClaimTypes.Email, modeluser.Email));
//                            myClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

//                            var roles = await userManager.GetRolesAsync(modeluser);
//                            if (roles != null)
//                            {
//                                foreach (var role in roles)
//                                {
//                                    myClaims.Add(new Claim(ClaimTypes.Role, role));

//                                }
//                            }
//                            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey));
//                            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//                            JwtSecurityToken myToken = new JwtSecurityToken(
//                                issuer: jwtSetting.ValidIssuer,
//                                audience: jwtSetting.ValidAudience,
//                                expires: DateTime.Now.AddDays(Convert.ToInt32( jwtSetting.TokenExpireInDays)),
//                                claims: myClaims,
//                                signingCredentials: credentials
//                                );

//                            var token = new JwtSecurityTokenHandler().WriteToken(myToken);
//                            return Ok(new
//                            {
//                                Token = token,
//                                Exp = myToken.ValidTo
//                            });
//                        }
//                        return BadRequest("Invalid Password");

//                    }
//                    return BadRequest("Invalid UserName");
//                }
//                return BadRequest(ModelState);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        [HttpPost("ForgetPassword")]
//        public async Task<IActionResult> ForgetPassword(ForgetPasswordDTO forgetPasswordDTO)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    var modeluser = await userManager.FindByEmailAsync(forgetPasswordDTO.Email);
//                    if (modeluser == null)
//                    {
//                        return BadRequest("Invalid User");
//                    }
//                    modeluser.OTP = OTPGenerator.GenerateOTP();
//                    modeluser.OTPGeneratedAt = DateTime.Now;
//                    // mail confirm to false
//                    modeluser.EmailConfirmed = false;

//                    await context.SaveChangesAsync();
//                    // generate token 
//                    var token = await userManager.GeneratePasswordResetTokenAsync(modeluser);
//                    var code = WebUtility.UrlEncode(token);

//                    bool flag = await mailService.SendMail(modeluser.Email, "SpatuimSW OTP", modeluser.OTP);
//                    if (flag)
//                    {
//                        return Ok(new
//                        {
//                            Message = $"Go to Your Mail  {modeluser.Email}",
//                            Code = code
//                        });
//                    }
//                    else
//                    {
//                        return BadRequest();
//                    }
//                }
//                else
//                {
//                    return BadRequest(ModelState);
//                }
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        [HttpPost("ChangePassword")]
//        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
//        {

//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    var modeluser = await userManager.FindByEmailAsync(changePasswordDTO.Email);
//                    if (modeluser == null)
//                    {
//                        return BadRequest("Invalid User");
//                    }


//                    var code = WebUtility.UrlDecode(changePasswordDTO.Code);

//                    IdentityResult result = await userManager.ResetPasswordAsync(modeluser, code, changePasswordDTO.NewPassword);
//                    if (result.Succeeded)
//                    {
//                        return Ok("Password Changed");
//                    }
//                    else
//                    {
//                        return BadRequest("error");
//                    }

//                }
//                else
//                {
//                    return BadRequest(ModelState);
//                }
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }
//    }
//}
