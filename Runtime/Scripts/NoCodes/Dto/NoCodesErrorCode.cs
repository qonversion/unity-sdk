namespace QonversionUnity
{
    /// <summary>
    /// Error codes for No-Codes SDK.
    /// </summary>
    public enum NoCodesErrorCode
    {
        Unknown,
        BadNetworkRequest,
        BadResponse,
        
        // Android specific
        ActivityStart,
        NetworkRequestExecution,
        Serialization,
        Deserialization,
        RequestDenied,
        Mapping,
        BackendError,
        ScreenNotFound,
        
        // Common
        QonversionError,
        
        // iOS specific
        Internal,
        AuthorizationFailed,
        Critical,
        ProductNotFound,
        ProductsLoadingFailed,
        RateLimitExceeded,
        ScreenLoadingFailed,
        SdkInitializationError
    }
}
