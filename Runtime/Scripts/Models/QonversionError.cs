using System.Collections.Generic;

namespace QonversionUnity
{
    public class QonversionError
    {
        public string Code;
        public string Message;

        public QonversionError(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("message", out object value)) Message = value as string;
            if (dict.TryGetValue("code", out value)) Code = value as string;
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