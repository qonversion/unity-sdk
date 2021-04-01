#import "Qonversion.h"
#import "UtilityBridge.h"

char* unityListenerName = nil;
const char *onRestoreMethod = "OnRestore";
const char *onPurchaseMethod = "OnPurchase";
const char *onCheckPermissionsMethod = "OnCheckPermissions";
const char *onProductsMethod = "OnProducts";
const char *onOfferingsMethod = "OnOfferings";

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

void _setUserID(const char* userID)
{
    [Qonversion setUserID:[UtilityBridge сonvertCStringToNSString:userID]];
}

void _addAttributionData(const char* conversionData, const int provider) {
    NSDictionary *conversionInfo = [UtilityBridge dictionaryFromJsonString: [UtilityBridge сonvertCStringToNSString: conversionData]];
    
    [Qonversion addAttributionData:conversionInfo
                      fromProvider:(QNAttributionProvider)provider];
}

void _checkPermissions(){
    [Qonversion checkPermissions:^(NSDictionary<NSString *,QNPermission *> *result, NSError *error) {
        [UtilityBridge handlePermissionsResponse:result withError:error toMethod:onCheckPermissionsMethod unityListener:unityListenerName];
    }];
}

void _restore(){
    [Qonversion restoreWithCompletion:^(NSDictionary *result, NSError *error) {
        [UtilityBridge handlePermissionsResponse:result withError:error toMethod:onRestoreMethod unityListener:unityListenerName];
    }];
}

void _purchase(const char* productId){
    [Qonversion purchase:[UtilityBridge сonvertCStringToNSString:productId] completion:^(NSDictionary *result, NSError *error, BOOL cancelled) {
        [UtilityBridge handlePermissionsResponse:result withError:error toMethod:onPurchaseMethod unityListener:unityListenerName];
    }];
}

void _products(){
    [Qonversion products:^(NSDictionary *result, NSError *error) {
        if (error) {
            [UtilityBridge handleErrorResponse:error toMethod:onProductsMethod unityListener:unityListenerName];
            return;
        }
        
        NSArray *products = [UtilityBridge convertProducts:result.allValues];
        [UtilityBridge sendUnityMessage:products toMethod:onProductsMethod unityListener: unityListenerName];
    }];
}

void _offerings(){
    [Qonversion offerings:^(QNOfferings * _Nullable result, NSError * _Nullable error) {
        if (error) {
            [UtilityBridge handleErrorResponse:error toMethod:onOfferingsMethod unityListener:unityListenerName];
            return;
        }
        
        NSDictionary *offerings = [UtilityBridge convertOfferings:result];
        [UtilityBridge sendUnityMessage:offerings toMethod:onOfferingsMethod unityListener: unityListenerName];
    }];
}
