namespace Utilities.Extensions
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum value)
        {
            var descriptionAttribute = (DisplayMessage)value.GetType().GetField(value.ToString()).GetCustomAttributes(false).Where(a => a is DisplayMessage).FirstOrDefault();
            return descriptionAttribute != null ? descriptionAttribute.Text : value.ToString();
        }
        public sealed class DisplayMessage : Attribute
        {
            #region Properties
            private readonly string _displayMessage;
            public string Text => _displayMessage;
            #endregion

            #region Constructor
            public DisplayMessage(string text)
            {
                _displayMessage = text;
            }
            #endregion
        }
    }
}
