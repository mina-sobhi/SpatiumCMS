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
using Domain.LookupsAggregate;
using Spatium_CMS.Extensions;
using Domain.StorageAggregate;
using Spatium_CMS.Controllers.StorageController.Response;
using Spatium_CMS.Controllers.UserManagmentController.Response;
using static System.Net.WebRequestMethods;

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
                CreateMap<Post, TopPostsCommentedResponse>();
                #endregion

                #region User Management
                CreateMap<ApplicationUserUpdateInput, UpdateUserRequest>().ReverseMap();
              
                CreateMap<ApplicationUserUpdateInputSuperAdmin, UpdateUserAsSuperAdminRequest>().ReverseMap();

                #endregion

                #region UserRole
                //get roles mapper
                CreateMap<ApplicationUser, UserResponse>();
                CreateMap<UserRole, RoleResult>();

                CreateMap<UserRole, ViewRoles>()
                      .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                      .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                      .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                      .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color))
                      .ForMember(dest => dest.ApplicationUsers, otp => otp.MapFrom(src => src.ApplicationUsers));

                CreateMap<RoleIcon, RoleIconRespones>().
                    ForMember(dest => dest.IconPath, otp => otp.MapFrom<IconUrlResolver>());

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
                    .ForMember(dest => dest.ProfileImagePath, otp => otp.MapFrom<UserProfileDetailesResolver>())
                    .ForMember(dest => dest.Email, otp => otp.MapFrom(src => src.Email))
                    .ForMember(dest => dest.PhoneNumber, otp => otp.MapFrom(src => src.PhoneNumber))
                    .ForMember(dest => dest.RoleName, otp => otp.MapFrom(r => r.Role.Name));

                CreateMap<ApplicationUser, CreatedByResponse>()
                    .ForMember(dest=>dest.Id,otp=>otp.MapFrom(src=>src.Id))
                    .ForMember(dest=>dest.FullName,otp=>otp.MapFrom(src=>src.FullName))
                    .ForMember(dest=>dest.ProfileImagePath,otp=>otp.MapFrom(src=>src.ProfileImagePath));

                CreateMap<ApplicationUser, ViewUsersResponse>()
                   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                   .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                   .ForMember(dest => dest.ProfileImagePath, otp => otp.MapFrom<UserProfileResolver>())
                   .ForMember(dest => dest.UserStatus, otp => otp.MapFrom(src => src.UserStatus.Name))
                   .ForMember(dest => dest.Email, otp => otp.MapFrom(src => src.Email))
                   .ForMember(dest => dest.RoleName, otp => otp.MapFrom(r => r.Role.Name))
                   .ForMember(dest => dest.CreatedAt, otp => otp.MapFrom(r => r.CreatedAt));
                #endregion

                #region GetPosts
                CreateMap<Post, PostRespone>()
                    .ForMember(des=>des.Status,otp=>otp.MapFrom(src=>src.Status.Name));

                CreateMap<Post, PostSnippetPreviewResponse>();


                #endregion

                #region Storage
                CreateMap<AddFolderInput, CreateFolderRequest>().ReverseMap();
                CreateMap<FileInput, AddFileRequest>().ReverseMap();
                CreateMap<UpdateFileInput, UpdateFileRequest>().ReverseMap();
                CreateMap<StaticFile, ViewFile>()
                    .ForMember(dest => dest.UrlPath, otp => otp.MapFrom<FileUrlResolver>())
                    .ForMember(dest=>dest.LastUpdated,otp=>otp.MapFrom(src=>src.LastUpdate));
                #endregion
            }
        }
    }
}
