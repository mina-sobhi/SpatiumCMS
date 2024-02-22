namespace Infrastructure.Strategies.PostStatusStrategy.Factory
{
    public interface IPostStatusFactory
    {
        IPostStutsStrategy GetStrategy(string RoleId);
    }
}
