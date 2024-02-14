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

        #endregion

        #region POST
        public const string TocNotFound = "Invalid Table Of content Id!";
        #endregion

        #region Blog
        public const string BlogIdCannotBeNull = "Blog Cann't be null!";

        #endregion
    }
}
