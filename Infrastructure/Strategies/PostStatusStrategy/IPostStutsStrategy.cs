using Utilities.Enums;

namespace Infrastructure.Strategies.PostStatusStrategy
{
    public interface IPostStutsStrategy
    {
        PostStatusEnum GetPostStatus();
    }
}
