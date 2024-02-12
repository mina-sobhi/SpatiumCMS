namespace Domain.ApplicationUserAggregate
{
    public class UserModule
    {
        #region Prop 
        public int Id { get; private set; }
        public string Name { get; private set; }
        #endregion

        #region Virtual List
        private readonly List<UserPermission> _userPermissions = new List<UserPermission>();
        public virtual IReadOnlyList<UserPermission> UserPermissions => _userPermissions.ToList();
        #endregion

        #region Ctor
        public UserModule()
        {

        }
        public UserModule(string Name)
        {
            this.Name = Name;
        }

        #endregion
    }
}