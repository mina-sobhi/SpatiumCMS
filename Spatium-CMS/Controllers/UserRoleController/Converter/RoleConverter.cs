using AutoMapper;
using Domain.ApplicationUserAggregate;
using Domain.ApplicationUserAggregate.Inputs;
using Spatium_CMS.Controllers.UserRoleController.Request;

namespace Spatium_CMS.Controllers.UserRoleController.Converter
{
    public class RoleConverter
    {
        private readonly IMapper mapper;
        internal RoleConverter(IMapper mapper)
        {
            this.mapper = mapper;
        }
        internal UserRoleInput GetUserRoleInput(RoleRequest request,string roleOwnerId)
        {
            var RoleInput = mapper.Map<UserRoleInput>(request);
            RoleInput.RoleOwnerId = roleOwnerId;
            
            return RoleInput;
        }
        internal UserRoleInput GetUpdatedRoleInput(RoleRequest request)
        {
            var RoleInput = mapper.Map<UserRoleInput>(request);
            return RoleInput;
        }
    }
}
