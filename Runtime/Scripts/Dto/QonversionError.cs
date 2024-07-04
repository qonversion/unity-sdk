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
        Unknown, // Unknown error
        ApiRateLimitExceeded, // API requests rate limit exceeded
        AppleStoreError, // Apple Store error received
        BackendError, // There was a backend error
        BillingUnavailable, // The Billing service is unavailable on the device
        ClientInvalid, // Client is not allowed to issue the request, etc
        CloudServiceNetworkConnectionFailed, // The device could not connect to the network
        CloudServicePermissionDenied, // User is not allowed to access cloud service information
        CloudServiceRevoked, // User has revoked permission to use this cloud service
        FailedToReceiveData, // Could not receive data
        FeatureNotSupported, // The requested feature is not supported
        FraudPurchase, // Fraud purchase was detected
        IncorrectRequest, // Request failed
        InternalError, // Internal backend error
        InvalidClientUid, // Client Uid is invalid or not set
        InvalidCredentials, // Access token is invalid or not set
        InvalidStoreCredentials, // This account does not have access to the requested application
        LaunchError, // There was an error while launching Qonversion SDK
        NetworkConnectionFailed, // There was a network issue. Make sure that the Internet connection is available on the device
        OfferingsNotFound, // No offerings found
        PaymentInvalid, // Purchase identifier was invalid, etc.
        PaymentNotAllowed, // This device is not allowed to make the payment
        PlayStoreError, // There was an issue with the Play Store service
        PrivacyAcknowledgementRequired, // User needs to acknowledge Apple's privacy policy
        ProductAlreadyOwned, // Failed to purchase since item is already owned
        ProductNotFound, // Failed to purchase since the Qonversion product was not found
        ProductNotOwned, // Failed to consume purchase since item is not owned
        ProjectConfigError, // The project is not configured or configured incorrectly in the Qonversion Dashboard
        PurchaseCanceled, // User pressed back or canceled a dialog for purchase
        PurchaseInvalid, // Failure of purchase
        PurchasePending, // Purchase is pending
        PurchaseUnspecified, // Unspecified state of the purchase
        ReceiptValidationError, // Receipt validation error
        RemoteConfigurationNotAvailable, // Remote configuration is not available for the current user or for the provided context key
        ResponseParsingFailed, // A problem occurred while serializing or deserializing data
        StoreProductNotAvailable, // Requested product is not available for purchase or its product id was not found
        UnauthorizedRequestData, // App is attempting to use SKPayment's requestData property, but does not have the appropriate entitlement
        UnknownClientPlatform, // The current platform is not supported
    }
}