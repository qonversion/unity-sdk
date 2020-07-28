#import "Qonversion.h"
#import "UtilityBridge.h"

typedef void (*QonversionSuccessInitCallback)(const char *);

static QonversionSuccessInitCallback onSuccessInitCallback;

void _setDebugMode(bool debugMode) {
    [Qonversion setDebugMode:debugMode];
}

void _launchWithKey(const char* key, const char* userID,
	QonversionSuccessInitCallback _onSuccessInitCallback) 
{
	onSuccessInitCallback = _onSuccessInitCallback;

    [Qonversion launchWithKey: [UtilityBridge сonvertCStringToNSString: key] 
        userID: [UtilityBridge сonvertCStringToNSString: userID] completion:^(NSString * _Nonnull uid) {
		_CallSuccessInitCallback(uid);
	}];
}

void _addAttributionData(const char* conversionData, const int provider) {
    NSDictionary *conversionInfo = [UtilityBridge dictionaryFromJsonString: [UtilityBridge сonvertCStringToNSString: conversionData]];

    [Qonversion addAttributionData:conversionInfo 
        fromProvider:(QAttributionProvider)provider];
}

void _CallSuccessInitCallback(NSString* uid) {
    if (onSuccessInitCallback != NULL) {
        onSuccessInitCallback([uid UTF8String]);
    }
}