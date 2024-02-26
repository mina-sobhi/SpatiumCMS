namespace Utilities.Results
{
    public struct ResponseMessages
    {
        #region Auth Service
        public const string InvalidEmailOrPassword = "Invalid Email Or Password!";
        public const string EmailIsAlreadyExist = "Email Already Exist!";
        public const string UserCreatedSuccessfully = "User Created Successfully! a verification email has been sent!";
        public const string InvalidEmail = "Invalid Email Address!";
        public const string EmailConfirmedSuccessfully = "Email Confirmed Successfully!";
        public const string VerificationEmailSent = "A verification email has been sent!";
        public const string EmailIsAlreadyConfirmed = "Email is Already Confirmed!";
        public const string ForgetPasswordEmailSent = "Email has been sent successfully!";
        public const string EmailNotConfirmed = "Email Not Confirmed! A verification Email has been sent to Email.";
        public const string InvalidOTP = "Invalid verification code.";
        public const string OTPExpired = "Verification Code Expired! an Email with a new OTP has been sent!";
        public const string PasswordChangedSuccessfully = "Password Changed Successfully!";
        public const string PasswordDoesnotMatch = "Password and Confirm Password doesn't match!";
        public const string OtpWaitingPeroidError = "Please wait 30 seconds before requesting new OTP.";
        public const string OtpConfirmed = "OTP Confirmed Successfully!";
        public const string UnauthorizedAccessLoginFirst = "Unauthorized Access! Please login First!";
        public const string UserNotFound = "User Not Found!";
        public const string UserStatusChanged = "User Status Changed Successfully!";
        public const string CannotChangeStatus = "User is already in this state!";
        public const string UserUnassignedSuccessfully = "User unassigned Successfully";
        public const string UserAssignedSuccessfully = "User Assigned Successfully";
        public const string UserUpdatedSuccessfully = "User Data Updated Successfully";

        public const string FileNotFound = "File not Found!";

        #endregion

        #region POST
        public const string TocNotFound = "Invalid Table Of content Id!";
        public const string PostNotFound = "Post not found";
        public const string AuthorNotFound = "No Author with Id in Blog";
        #endregion

        #region Blog
        public const string BlogIdCannotBeNull = "Blog Can't be null!";

        #endregion

        #region Roles
        public const string InvalidRole = "Role is Invalid";
        public const string RoleIconsNotFound = "Role Icons not found!";
        public const string RoleCreatedSuccessfully = "Role Created Successfully!";
        public const string InvalidRoleModule = "Invalid Role Module!";
        public const string RoleUpdatedSuccessfully = "Role Updated Successfully!";
        public const string RoleNameIsRequired = "Role Name is Required!";

        #endregion

        #region Media
        public const string InvalidFolder = "Invalid folder!";
        public const string StorageNotFound = "No storage for this blog!";
        public const string InvalidFileName = "Invalid File Name";
        public const string FileDeletedSuccessfully = "File Deleted Successfully";
        public const string FolderCreatedSuccessfully = "Folder Created Successfully";
        public const string FoldersAndFilesMovedSuccessfully = "Selected folders and files are Moved Successfully!";
        public const string FoldersAndFilesDeletedSuccessfully = "Selected folders and files are Deleted Successfully!";
        public const string FileAddedSuccessfully = "File Added Successfully!";
        public const string FileUpdatedSuccessfully = "File Updated Successfully!";
        public const string FileAltSuccessfully = "File Alt Added Successfully!";
        #endregion

        #region Other
        public const string InvalidDateFormat = "Invalid Date Format";
        public const string SearchColumn = "Search column cannot be empty";
        public const string SearchWord = "Search word cannot be empty";

        #endregion
    }
}
