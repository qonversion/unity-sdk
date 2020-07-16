#import "Qonversion.h"
#import "UtilityBridge.h"

void _setDebugMode(bool debugMode) {
    [Qonversion setDebugMode:debugMode];
}

void _launchWithKey(const char* key, const char* userID) {
    [Qonversion launchWithKey: [UtilityBridge сonvertCStringToNSString: key] 
        userID: [UtilityBridge сonvertCStringToNSString: userID]];
}

void _addAttributionData(const char* conversionData, const int provider) {
    NSDictionary *conversionInfo = [UtilityBridge dictionaryFromJsonString: [UtilityBridge сonvertCStringToNSString: conversionData]];

    [Qonversion addAttributionData:conversionInfo 
        fromProvider:(QAttributionProvider)provider];
}