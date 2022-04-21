using System.Collections.Generic;

namespace QonversionUnity
{
    public class QonversionError
    {
        public string Code;
        public string Message;

        public QonversionError(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("code", out value)) Code = value as string;
            string message = ""
            if (dict.TryGetValue("description", out description))
            {
                message = description;
                if (dict.TryGetValue("domain", out domain))
                {
                    message += ". Domain: " + domain;
                }
                if (dict.TryGetValue("additionalMessage", out additionalMessage))
                {
                    message += "\nDebugInfo: " + additionalMessage
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