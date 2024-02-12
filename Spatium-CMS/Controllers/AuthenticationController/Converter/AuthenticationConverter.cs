using AutoMapper;
using Domain.ApplicationUserAggregate.Inputs;
using Spatium_CMS.Controllers.AuthenticationController.Request;
using Spatium_CMS.Controllers.AuthenticationController.Response;
using Utilities.Results;

namespace Spatium_CMS.Controllers.AuthenticationController.Converter
{
    internal class AuthenticationConverter
    {
        private readonly IMapper mapper;
        internal AuthenticationConverter(IMapper mapper)
        {
            this.mapper = mapper;
        }

        internal ApplicationUserInput GetApplicationUserInput(RegisterRequest request, string roleId)
        {
            var userInput = mapper.Map<ApplicationUserInput>(request);
            userInput.ProfileImagePath = "Default Image Path";
            userInput.RoleId = roleId;
            return userInput;
        }

        internal ForgetPasswordResponse GetForgetPasswordResponse (SpatiumResponse<string> forgetPasswordRes,string email)
        {
            return new ForgetPasswordResponse()
            {
                Token=forgetPasswordRes.Data,
                Email=email,
                Message =forgetPasswordRes.Message
            };
        }
     
    }
}