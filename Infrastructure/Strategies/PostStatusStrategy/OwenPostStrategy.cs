using Utilities.Enums;

namespace Infrastructure.Strategies.PostStatusStrategy
{
    public class OwenPostStrategy : IPostStutsStrategy
    {
        public PostStatusEnum GetPostStatus()
        {
            return PostStatusEnum.Pending;
        }
    }
}
