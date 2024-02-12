using System.Text;

namespace Utilities.Exceptions
{
    public class SpatiumException:Exception
    {
        public new string Message { get; set; }

        public SpatiumException(string message)
        {
            Message=message;
        }

        public SpatiumException(string[] errorMessages)
        {
            var sb = new StringBuilder();
            foreach (var erorr in errorMessages)
                sb.AppendLine(erorr.ToString());
            Message = sb.ToString();
        }
    }
}