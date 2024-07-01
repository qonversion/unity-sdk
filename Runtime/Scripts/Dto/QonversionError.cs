using System;
using System.Collections.Generic;

namespace QonversionUnity
{
    public class QonversionError
    {
        public QErrorCode Code;
        public string Message;

        public QonversionError(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("code", out object value)) Code = FormatErrorCode(value);
            string message = "";
            if (dict.TryGetValue("description", out object description))
            {
                message += description;
                if (dict.TryGetValue("domain", out object domain))
                {
                    message += ". Domain: " + domain;
                }
                if (dict.TryGetValue("additionalMessage", out object additionalMessage))
                {
                    message += "\nDebugInfo: " + additionalMessage;
                }
            }

            Message = message;
        }

        internal QonversionError(QErrorCode code, string message)
        {
            Code = code;
            Message = message;
        }

        public override string ToString()
        {
            return $"{nameof(Code)}: {Code}, " +
                   $"{nameof(Message)}: {Message}";
        }

        private QErrorCode FormatErrorCode(object code)
        {
            return Enum.TryParse(code.ToString(), out QErrorCode parsedSource)
                ? parsedSource
                : QErrorCode.Unknown;
        }
    }

    public enum QErrorCode
    {
        Unknown,
        ApiRateLimitExceeded,
        AppleStoreError,
        BackendError,
        BillingUnavailable,
        ClientInvalid,
        CloudServiceNetworkConnectionFailed,
        CloudServicePermissionDenied,
        CloudServiceRevoked,
        FailedToReceiveData,
        FeatureNotSupported,
        FraudPurchase,
        IncorrectRequest,
        InternalError,
        InvalidClientUid,
        InvalidCredentials,
        InvalidStoreCredentials,
        LaunchError,
        NetworkConnectionFailed,
        OfferingsNotFound,
        PaymentInvalid,
        PaymentNotAllowed,
        PlayStoreError,
        PrivacyAcknowledgementRequired,
        ProductAlreadyOwned,
        ProductNotFound,
        ProductNotOwned,
        ProjectConfigError,
        PurchaseCanceled,
        PurchaseInvalid,
        PurchasePending,
        PurchaseUnspecified,
        ReceiptValidationError,
        RemoteConfigurationNotAvailable,
        ResponseParsingFailed,
        StoreProductNotAvailable,
        UnauthorizedRequestData,
        UnknownClientPlatform,
    }
}