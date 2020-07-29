#import "Qonversion.h"
#import "UtilityBridge.h"

typedef void (*QonversionSuccessInitCallback)();

void _setDebugMode(bool debugMode) {
    [Qonversion setDebugMode:debugMode];
}

void _launchWithKey(const char* key, const char* userID,
	QonversionSuccessInitCallback _onSuccessInitCallback) 
{
    [Qonversion launchWithKey: [UtilityBridge сonvertCStringToNSString: key] 
        userID: [UtilityBridge сonvertCStringToNSString: userID]];
	
	if (_onSuccessInitCallback != NULL) {
        _onSuccessInitCallback();
    }
}

void _addAttributionData(const char* conversionData, const int provider) {
    NSDictionary *conversionInfo = [UtilityBridge dictionaryFromJsonString: [UtilityBridge сonvertCStringToNSString: conversionData]];

    [Qonversion addAttributionData:conversionInfo 
        fromProvider:(QAttributionProvider)provider];
}