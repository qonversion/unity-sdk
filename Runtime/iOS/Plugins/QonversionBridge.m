#import "Qonversion.h"
#import "UtilityBridge.h"

char* unityListenerName = nil;

void _setDebugMode() {
    [Qonversion setDebugMode];
}

void _launchWithKey(const char* unityListener, const char* key)
{
    unsigned long len = strlen(unityListener);
    unityListenerName = malloc(len + 1);
    strcpy(unityListenerName, unityListener);
    
    [Qonversion launchWithKey:[UtilityBridge сonvertCStringToNSString:key]];
}

void _setAdvertisingID() {
    [Qonversion setAdvertisingID];
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

void _checkPermissions(const char* unityCallbackName){
    NSString *callbackName = [UtilityBridge сonvertCStringToNSString:unityCallbackName];
    
    [Qonversion checkPermissions:^(NSDictionary<NSString *,QNPermission *> *result, NSError *error) {
        [UtilityBridge handlePermissionsResponse:result withError:error toMethod:callbackName unityListener:unityListenerName];
    }];
}

void _restore(const char* unityCallbackName){
    NSString *callbackName = [UtilityBridge сonvertCStringToNSString:unityCallbackName];

    [Qonversion restoreWithCompletion:^(NSDictionary *result, NSError *error) {
        [UtilityBridge handlePermissionsResponse:result withError:error toMethod:callbackName unityListener:unityListenerName];
    }];
}

void _purchase(const char* productId, const char* unityCallbackName){
    NSString *callbackName = [UtilityBridge сonvertCStringToNSString:unityCallbackName];

    [Qonversion purchase:[UtilityBridge сonvertCStringToNSString:productId] completion:^(NSDictionary *result, NSError *error, BOOL cancelled) {
        [UtilityBridge handlePermissionsResponse:result withError:error toMethod:callbackName unityListener:unityListenerName];
    }];
}

void _products(const char* unityCallbackName){
    NSString *callbackName = [UtilityBridge сonvertCStringToNSString:unityCallbackName];

    [Qonversion products:^(NSDictionary *result, NSError *error) {
        if (error) {
            [UtilityBridge handleErrorResponse:error toMethod:callbackName unityListener:unityListenerName];
            return;
        }
        
        NSArray *products = [UtilityBridge convertProducts:result.allValues];
        [UtilityBridge sendUnityMessage:products toMethod:callbackName unityListener: unityListenerName];
    }];
}

void _offerings(const char* unityCallbackName){
    NSString *callbackName = [UtilityBridge сonvertCStringToNSString:unityCallbackName];

    [Qonversion offerings:^(QNOfferings * _Nullable result, NSError * _Nullable error) {
        if (error) {
            [UtilityBridge handleErrorResponse:error toMethod:callbackName unityListener:unityListenerName];
            return;
        }
        
        NSDictionary *offerings = [UtilityBridge convertOfferings:result];
        [UtilityBridge sendUnityMessage:offerings toMethod:callbackName unityListener: unityListenerName];
    }];
}
