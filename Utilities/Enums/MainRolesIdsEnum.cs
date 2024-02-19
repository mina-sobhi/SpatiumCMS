using static Utilities.Extensions.EnumExtension;

namespace Utilities.Enums
{
    public enum MainRolesIdsEnum
    {
        [DisplayMessage("10def0fc-dbe7-4807-b01c-9d75dfdc67de")]
        SuperAdmin,

        [DisplayMessage("8a3f1c05-9db4-447a-989c-d4db082fb6ca")]
        ArticleCreator,

        [DisplayMessage("93b6c7df-b613-4b7f-a793-afc260d0a5a0")]
        Editor,

        [DisplayMessage("97152995-238d-44a2-a727-f75b0c530f0c")]
        Viewer,

        [DisplayMessage("b62e1554-257e-4e83-94df-9d56e8b27367")]
        SEO,

        [DisplayMessage("5c78edbb-0121-4a88-a7b1-5172d77e2aed")]
        Unassigned
    }
}