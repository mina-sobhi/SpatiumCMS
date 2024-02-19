namespace Domain.ApplicationUserAggregate.Inputs
{
    public class ApplicationUserInput
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RoleId { get; set; }
        public string JobTitle { get; set; }
        public string ProfileImagePath { get; set; }
        public string ParentUserId { get; set; }
        public int ParentBlogId { get; set; }   
    }
}
