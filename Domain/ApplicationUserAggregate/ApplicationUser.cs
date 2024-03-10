using Domain.ApplicationUserAggregate.Inputs;
using Domain.BlogsAggregate;
using Domain.LookupsAggregate;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Utilities.Enums;

namespace Domain.ApplicationUserAggregate
{
    public class ApplicationUser : IdentityUser
    {
        #region CTOR
        public ApplicationUser() { }

        public ApplicationUser(ApplicationUserInput input)
        {
            FullName = input.FullName.Trim();
            Email = input.Email.Trim();
            UserName = input.Email.Trim();
            RoleId = input.RoleId;
            JobTitle = input.JobTitle.Trim();
            ProfileImagePath = input.ProfileImagePath.Trim();
            UserStatusId = 3;
            if (!string.IsNullOrEmpty(input.ParentUserId))
            {
                ParentUserId = input.ParentUserId;
                BlogId = input.ParentBlogId;
            }
            else
            {
                Blog = new Blog(new BlogsAggregate.Input.BlogInput()
                {
                    FavIconPath = "Icon path",
                    Name = FullName,
                    OwnerId = this.Id
                });
                
            }
            CreatedAt = DateTime.UtcNow;
        }
        #endregion

        #region Properties
        public string FullName { get; private set; }
        public string RoleId { get; private set; }
        public string JobTitle { get; private set; }
        public string ProfileImagePath { get; private set; }
        //public bool IsAccountActive { get; private set; }
        public int UserStatusId { get;private set; }
        public string ParentUserId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string OTP { get; private set; }
        public DateTime? OTPGeneratedAt { get; private set; }
        public int BlogId { get; private set; }
        #endregion

        #region Navigational Properties
        public virtual ApplicationUser ParentUser { get; private set; }
        public virtual UserRole Role { get; private set; }
        public virtual Blog Blog { get; private set; }
        [EnumDataType(typeof(UserStatus))]
        public virtual UserStatus UserStatus { get; private set; }
        #endregion

        #region Virtual List
        //private readonly List<Audit> _audits = new List<Audit>();
        //public virtual IReadOnlyList<Audit> Audits => _audits.ToList();

        //self relation
        private readonly List<ApplicationUser> _applicationUsers = new List<ApplicationUser>();
        public virtual IReadOnlyList<ApplicationUser> ApplicationUsers => _applicationUsers.ToList();

        private readonly List<UserRole> _OwnedRoles = new List<UserRole>();
        public virtual IReadOnlyList<UserRole> OwnedRoles => _OwnedRoles.ToList();
        #endregion

        public void ChangeOTP(string otp)
        {
            this.OTP = otp;
            this.OTPGeneratedAt = DateTime.UtcNow;
        }

        public void ClearOTP()
        {
            this.OTP = null;
            this.OTPGeneratedAt = null;
        }
        public void Update(ApplicationUserUpdateInput input)
        {
            this.FullName = input.FullName;
            //this.RoleId = input.RoleId;
            PhoneNumber=input.PhoneNumber;
            ProfileImagePath=input.ProfileImagePath;
        }
        public void Update(ApplicationUserUpdateInputSuperAdmin  input)
        {
            this.FullName = input.UserName;
            this.RoleId = input.RoleId;
            PhoneNumber = input.Phone;
     
        }

        public void ChangeActivation(UserStatusEnum userStatus)
        {
            this.UserStatusId= (int)userStatus;
        }

        public void Unassign()
        {
            RoleId = "5c78edbb-0121-4a88-a7b1-5172d77e2aed";
        }
        public void AssigneToRole(string roleId)
        {
           this.RoleId = roleId;
        }
    }
}
