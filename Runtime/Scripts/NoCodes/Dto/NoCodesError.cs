using System.Collections.Generic;
using JetBrains.Annotations;

namespace QonversionUnity
{
    /// <summary>
    /// Error that can occur in No-Codes SDK.
    /// </summary>
    public class NoCodesError
    {
        /// <summary>
        /// Error code identifying the type of error.
        /// </summary>
        public readonly NoCodesErrorCode Code;

        /// <summary>
        /// Human-readable description of the error.
        /// </summary>
        [CanBeNull] public readonly string Description;

        /// <summary>
        /// Additional message with more details about the error.
        /// </summary>
        [CanBeNull] public readonly string AdditionalMessage;

        /// <summary>
        /// Domain of the error (e.g., iOS specific).
        /// </summary>
        [CanBeNull] public readonly string Domain;

        /// <summary>
        /// Underlying Qonversion error if this error was caused by a Qonversion SDK error.
        /// </summary>
        [CanBeNull] public readonly QonversionError QonversionError;

        public NoCodesError(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("code", out object codeValue))
            {
                Code = ParseErrorCode(codeValue as string);
            }

            if (dict.TryGetValue("description", out object descValue))
            {
                Description = descValue as string;
            }

            if (dict.TryGetValue("additionalMessage", out object additionalValue))
            {
                AdditionalMessage = additionalValue as string;
            }

            if (dict.TryGetValue("domain", out object domainValue))
            {
                Domain = domainValue as string;
            }

            if (dict.TryGetValue("error", out object errorValue) && errorValue is Dictionary<string, object> errorDict)
            {
                QonversionError = new QonversionError(errorDict);
            }
        }

        private static NoCodesErrorCode ParseErrorCode(string code)
        {
            switch (code)
            {
                case "Unknown": return NoCodesErrorCode.Unknown;
                case "BadNetworkRequest": return NoCodesErrorCode.BadNetworkRequest;
                case "BadResponse": return NoCodesErrorCode.BadResponse;
                case "ActivityStart": return NoCodesErrorCode.ActivityStart;
                case "NetworkRequestExecution": return NoCodesErrorCode.NetworkRequestExecution;
                case "Serialization": return NoCodesErrorCode.Serialization;
                case "Deserialization": return NoCodesErrorCode.Deserialization;
                case "RequestDenied": return NoCodesErrorCode.RequestDenied;
                case "Mapping": return NoCodesErrorCode.Mapping;
                case "BackendError": return NoCodesErrorCode.BackendError;
                case "ScreenNotFound": return NoCodesErrorCode.ScreenNotFound;
                case "QonversionError": return NoCodesErrorCode.QonversionError;
                case "Internal": return NoCodesErrorCode.Internal;
                case "AuthorizationFailed": return NoCodesErrorCode.AuthorizationFailed;
                case "Critical": return NoCodesErrorCode.Critical;
                case "ProductNotFound": return NoCodesErrorCode.ProductNotFound;
                case "ProductsLoadingFailed": return NoCodesErrorCode.ProductsLoadingFailed;
                case "RateLimitExceeded": return NoCodesErrorCode.RateLimitExceeded;
                case "ScreenLoadingFailed": return NoCodesErrorCode.ScreenLoadingFailed;
                case "SDKInitializationError": return NoCodesErrorCode.SdkInitializationError;
                default: return NoCodesErrorCode.Unknown;
            }
        }

        public override string ToString()
        {
            return $"{nameof(Code)}: {Code}, " +
                   $"{nameof(Description)}: {Description}, " +
                   $"{nameof(AdditionalMessage)}: {AdditionalMessage}, " +
                   $"{nameof(Domain)}: {Domain}";
        }
    }
}
