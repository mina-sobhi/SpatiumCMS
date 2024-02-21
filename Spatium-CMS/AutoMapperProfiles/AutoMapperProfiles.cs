using AutoMapper;
using Domain.ApplicationUserAggregate;
using Domain.ApplicationUserAggregate.Inputs;
using Domain.BlogsAggregate;
using Domain.BlogsAggregate.Input;
using Spatium_CMS.Controllers.AuthenticationController.Request;
using Spatium_CMS.Controllers.AuthenticationController.Response;
using Spatium_CMS.Controllers.BlogsController.Request;
using Spatium_CMS.Controllers.BlogsController.Response;
using Spatium_CMS.Controllers.CommentController.Request;
using Spatium_CMS.Controllers.CommentController.Response;
using Spatium_CMS.Controllers.PostController.Request;
using Spatium_CMS.Controllers.UserManagmentController.Request;
using Spatium_CMS.Controllers.UserRoleController.Response;
using Spatium_CMS.Controllers.UserRoleController.Request;
using Spatium_CMS.Controllers.PostController.Response;
using Domain.StorageAggregate.Input;
using Spatium_CMS.Controllers.StorageController.Request;

namespace Spatium_CMS.AutoMapperProfiles
{
    public class AutoMapperProfiles
    {
        public class AutoMapperProfile : Profile
        {
            public AutoMapperProfile()
            {
                #region Authentication
                CreateMap<ApplicationUserInput, RegisterRequest>().ReverseMap();
                CreateMap<CreateBlogRequest , BlogInput>().ReverseMap();
                CreateMap<UpdateBlogRequest , BlogInput>().ReverseMap();
                CreateMap<Blog, BlogResult>().ReverseMap();
                CreateMap<CreateUserRequest, ApplicationUserInput>();
                #endregion

                #region posts
                CreateMap<PostInput, CreatePostRequest>().ReverseMap();
                CreateMap<UpdatePostInput, UpdatePostRequest>().ReverseMap();
                CreateMap<TableOfContentInput, TableOfContentRequest>().ReverseMap();
                CreateMap<CommentUpdateInput, Comment>().ReverseMap();
                CreateMap<UpdateTableOfContentRequest ,UpdateTableOfContentInput>().ReverseMap();
                #endregion

                #region Comment
                CreateMap<Comment, CommentResponse>().ReverseMap();
                CreateMap<CommentRequest, CommentInput>().ReverseMap();
                #endregion

                #region User Management
                CreateMap<ApplicationUserUpdateInput, UpdateUserRequest>().ReverseMap();
              


                #endregion

                #region UserRole
                //get roles mapper
                CreateMap<ApplicationUser, UserResponse>();
                CreateMap<UserRole, RoleResult>();


                CreateMap<UserRole, ViewRoles>()
                      .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
                      .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Name))
                      .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                      .ForMember(dest => dest.IconPath, opt => opt.MapFrom(src => src.IconPath))
                      .ForMember(dest => dest.ApplicationUsers, otp => otp.MapFrom(src => src.ApplicationUsers));

                CreateMap<UpdateUserRoleInput, UpdateUserRoleRequest>().ReverseMap();
              
                CreateMap<UserRoleInput, RoleRequest>().ReverseMap();

                CreateMap<UserModule, ViewModule>();
                CreateMap<UserPermission,UserModulePermissions>()
                    .ForMember(dest => dest.Id,otp=>otp.MapFrom(src => src.Id))
                    .ForMember(dest=>dest.Name,otp=>otp.MapFrom(src => src.Name));    
                #endregion

                #region ApplicationUser
                //GetUserDetailes
                CreateMap<ApplicationUser, ViewUserProfileResult>()
                    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                    .ForMember(dest => dest.ProfileImagePath, otp => otp.MapFrom(src => src.ProfileImagePath))
                    .ForMember(dest => dest.Email, otp => otp.MapFrom(src => src.Email))
                    .ForMember(dest => dest.PhoneNumber, otp => otp.MapFrom(src => src.PhoneNumber))
                    .ForMember(dest => dest.RoleName, otp => otp.MapFrom(r => r.Role.Name));

                CreateMap<ApplicationUser, CreatedByResponse>()
                    .ForMember(dest=>dest.Id,otp=>otp.MapFrom(src=>src.Id))
                    .ForMember(dest=>dest.FullName,otp=>otp.MapFrom(src=>src.FullName))
                    .ForMember(dest=>dest.ProfileImagePath,otp=>otp.MapFrom(src=>src.ProfileImagePath));
                #endregion

                #region GetPosts
                CreateMap<Post, PostRespone>()
                    .ForMember(des=>des.Status,otp=>otp.MapFrom(src=>src.Status.Name));

                CreateMap<Post, PostSnippetPreviewResponse>();

          
                #endregion

                CreateMap<AddFolderInput,CreateFolderRequest>().ReverseMap();
                CreateMap<FileInput, AddFileRequest>().ReverseMap();
            }
        }
    }
}
