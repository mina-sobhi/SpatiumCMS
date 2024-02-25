using Utilities.Enums;

namespace Infrastructure.Strategies.PostStatusStrategy
{
    public class AllPostStrategy : IPostStutsStrategy
    {
        public PostStatusEnum GetPostStatus()
        {
            return PostStatusEnum.Published;
        }
    }
}
