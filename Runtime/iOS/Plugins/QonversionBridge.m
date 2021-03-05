#import "Qonversion.h"
#import "UtilityBridge.h"

void _setDebugMode() {
    [Qonversion setDebugMode];
}

void _launchWithKey(const char* key)
{
    [Qonversion launchWithKey:[UtilityBridge сonvertCStringToNSString:key]];
}

void _setUserID(const char* userID)
{
    [Qonversion setUserID:[UtilityBridge сonvertCStringToNSString:userID]];
}

void _addAttributionData(const char* conversionData, const int provider) {
    NSDictionary *conversionInfo = [UtilityBridge dictionaryFromJsonString: [UtilityBridge сonvertCStringToNSString: conversionData]];

    [Qonversion addAttributionData:conversionInfo
        fromProvider:(QNAttributionProvider)provider];
}
