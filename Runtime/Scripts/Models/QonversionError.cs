using System.Collections.Generic;

namespace QonversionUnity
{
    public class QonversionError
    {
        public string Code;
        public string Message;

        public QonversionError(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("code", out object value)) Code = value as string;
            string message = "";
            if (dict.TryGetValue("description", out object description))
            {
                message += description;
                if (dict.TryGetValue("domain", out object domain))
                {
                    message += ". Domain: " + domain.ToString();
                }
                if (dict.TryGetValue("additionalMessage", out object additionalMessage))
                {
                    message += "\nDebugInfo: " + additionalMessage;
                }
            }

            Message = message;
        }

        internal QonversionError(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public override string ToString()
        {
            return $"{nameof(Code)}: {Code}, " +
                   $"{nameof(Message)}: {Message}";
        }
    }
}