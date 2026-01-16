using System.Collections.Generic;
using JetBrains.Annotations;

namespace QonversionUnity
{
    /// <summary>
    /// Represents an action that can be executed from No-Codes screens.
    /// </summary>
    public class NoCodesAction
    {
        /// <summary>
        /// Type of the action.
        /// </summary>
        public readonly NoCodesActionType Type;

        /// <summary>
        /// Additional values associated with the action.
        /// </summary>
        [CanBeNull] public readonly Dictionary<string, string> Value;

        /// <summary>
        /// Error that occurred during action execution, if any.
        /// </summary>
        [CanBeNull] public readonly NoCodesError Error;

        public NoCodesAction(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("type", out object typeValue))
            {
                Type = ParseActionType(typeValue as string);
            }

            if (dict.TryGetValue("value", out object valueObj) && valueObj is Dictionary<string, object> valueDict)
            {
                Value = new Dictionary<string, string>();
                foreach (var kvp in valueDict)
                {
                    Value[kvp.Key] = kvp.Value?.ToString();
                }
            }

            if (dict.TryGetValue("error", out object errorValue) && errorValue is Dictionary<string, object> errorDict)
            {
                Error = new NoCodesError(errorDict);
            }
        }

        private static NoCodesActionType ParseActionType(string type)
        {
            switch (type)
            {
                case "url": return NoCodesActionType.Url;
                case "deeplink": return NoCodesActionType.Deeplink;
                case "navigation": return NoCodesActionType.Navigation;
                case "purchase": return NoCodesActionType.Purchase;
                case "restore": return NoCodesActionType.Restore;
                case "close": return NoCodesActionType.Close;
                case "closeAll": return NoCodesActionType.CloseAll;
                default: return NoCodesActionType.Unknown;
            }
        }

        public override string ToString()
        {
            return $"{nameof(Type)}: {Type}, " +
                   $"{nameof(Value)}: {Value}, " +
                   $"{nameof(Error)}: {Error}";
        }
    }
}
