#import "UtilityBridge.h"
#import "QNUAutomationsDelegate.h"
@import QonversionSandwich;

char* unityListenerName = nil;

@interface QonversionEventListenerWrapper : NSObject <QonversionEventListener>

- (void)qonversionDidReceiveUpdatedEntitlements:(NSDictionary<NSString *,id> * _Nonnull)entitlements;
- (void)shouldPurchasePromoProductWith:(NSString * _Nonnull)productId;

@end

@implementation QonversionEventListenerWrapper

- (void)shouldPurchasePromoProductWith:(NSString * _Nonnull)productId {
    UnitySendMessage(unityListenerName, "OnReceivePromoPurchase", productId.UTF8String);
}

- (void)qonversionDidReceiveUpdatedEntitlements:(NSDictionary<NSString *,id> * _Nonnull)entitlements {
    [UtilityBridge sendUnityMessage:entitlements toMethod:@"OnReceivedUpdatedEntitlements" unityListener: unityListenerName];
}

@end

static QNUAutomationsDelegate *automationsBridge;
static QonversionSandwich *qonversionSandwich;

void _initialize(const char* unityListener) {
    unsigned long len = strlen(unityListener);
    unityListenerName = malloc(len + 1);
    strcpy(unityListenerName, unityListener);

    qonversionSandwich = [[QonversionSandwich alloc] initWithQonversionEventListener:[QonversionEventListenerWrapper new]];
    automationsBridge = [[QNUAutomationsDelegate alloc] initWithListenerName:unityListenerName];
}

void _initializeSdk(const char* projectKey, const char* launchMode, const char* environment, const char* entitlementsCacheLifetime) {
    NSString *keyStr = [UtilityBridge сonvertCStringToNSString:projectKey];
    NSString *launchModeStr = [UtilityBridge сonvertCStringToNSString:launchMode];
    NSString *envStr = [UtilityBridge сonvertCStringToNSString:environment];
    NSString *cacheLifetimeStr = [UtilityBridge сonvertCStringToNSString:entitlementsCacheLifetime];

    [qonversionSandwich initializeWithProjectKey:keyStr launchModeKey:launchModeStr environmentKey:envStr entitlementsCacheLifetimeKey:cacheLifetimeStr];
}

//_initializeSdk(string projectKey, string launchMode, string environment, string entitlementsCacheLifetime);
void _storeSdkInfo(const char* version, const char* source) {
    NSString *versionStr = [UtilityBridge сonvertCStringToNSString:version];
    NSString *sourceStr = [UtilityBridge сonvertCStringToNSString:source];
    
    [qonversionSandwich storeSdkInfoWithSource:sourceStr version:versionStr];
}

void _setAdvertisingID() {
    [qonversionSandwich collectAdvertisingId];
}

void _presentCodeRedemptionSheet() {
    if (@available(iOS 14.0, *)) {
        [qonversionSandwich presentCodeRedemptionSheet];
    }
}

void _setAppleSearchAdsAttributionEnabled(const bool enable) {
    [qonversionSandwich collectAppleSearchAdsAttribution];
}

void _setProperty(const char* propertyName, const char* value) {
    NSString *propertyStr = [UtilityBridge сonvertCStringToNSString:propertyName];
    NSString *valueStr = [UtilityBridge сonvertCStringToNSString:value];
    
    [qonversionSandwich setDefinedProperty:propertyStr value:valueStr];
}

void _setUserProperty(const char* key, const char* value) {
    NSString *keyStr = [UtilityBridge сonvertCStringToNSString:key];
    NSString *valueStr = [UtilityBridge сonvertCStringToNSString:value];
    
    [qonversionSandwich setCustomProperty:keyStr value:valueStr];
}

void _addAttributionData(const char* conversionData, const char* provider) {
    NSDictionary *conversionInfo = [UtilityBridge dictionaryFromJsonString: [UtilityBridge сonvertCStringToNSString: conversionData]];
    NSString *providerStr = [UtilityBridge сonvertCStringToNSString:provider];

    [qonversionSandwich attributionWithProviderKey:providerStr value:conversionInfo];
}

void _identify(const char* userId) {
    NSString *userIdStr = [UtilityBridge сonvertCStringToNSString:userId];
    [qonversionSandwich identify:userIdStr];
}

void _userInfo(const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge сonvertCStringToNSString:unityCallbackName];

    [qonversionSandwich userInfo:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _logout() {
    [qonversionSandwich logout];
}

void _checkPermissions(const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge сonvertCStringToNSString:unityCallbackName];
    
    [qonversionSandwich checkEntitlements:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _restore(const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge сonvertCStringToNSString:unityCallbackName];

    [qonversionSandwich restore:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _purchase(const char* productId, const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge сonvertCStringToNSString:unityCallbackName];
    NSString *productIdStr = [UtilityBridge сonvertCStringToNSString:productId];
    
    [qonversionSandwich purchase:productIdStr completion:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _purchaseProduct(const char* productId, const char* offeringId, const char* unityCallbackName) {
    NSString *productIdStr = [UtilityBridge сonvertCStringToNSString:productId];
    NSString *offeringIdStr = [UtilityBridge сonvertCStringToNSString:offeringId];
    NSString *callbackName = [UtilityBridge сonvertCStringToNSString:unityCallbackName];
    
    [qonversionSandwich purchaseProduct:productIdStr offeringId:offeringIdStr completion:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _products(const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge сonvertCStringToNSString:unityCallbackName];
    
    [qonversionSandwich products:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _offerings(const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge сonvertCStringToNSString:unityCallbackName];
    
    [qonversionSandwich offerings:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _checkTrialIntroEligibility(const char* productIdsJson, const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge сonvertCStringToNSString:unityCallbackName];
    NSString *productIdsJsonStr = [UtilityBridge сonvertCStringToNSString:productIdsJson];
    
    NSError *error = nil;
    NSData *data = [productIdsJsonStr dataUsingEncoding:NSUTF8StringEncoding];
    NSArray *products = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
    
    if (error) {
        NSLog(@"An error occurred while serializing data: %@", error.localizedDescription);
        return;
    }
    if (products) {
        [qonversionSandwich checkTrialIntroEligibility:products completion:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
            [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
        }];
    }
}

void _promoPurchase(const char* storeProductId, const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge сonvertCStringToNSString:unityCallbackName];
    NSString *storeProductIdStr = [UtilityBridge сonvertCStringToNSString:storeProductId];
    
    [qonversionSandwich promoPurchase:storeProductIdStr completion:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _setNotificationsToken(const char* token) {
    NSString *tokenStr = [UtilityBridge сonvertCStringToNSString:token];
    [automationsBridge setNotificationsToken:tokenStr];
}

bool _handleNotification(const char* notification) {
    NSDictionary *notificationInfo = [UtilityBridge dictionaryFromJsonString: [UtilityBridge сonvertCStringToNSString: notification]];
    
    BOOL isQonversionNotification = [automationsBridge handleNotification:notificationInfo];
    
    return isQonversionNotification;
}

const char* _getNotificationCustomPayload(const char* notification) {
  NSDictionary *notificationInfo = [UtilityBridge dictionaryFromJsonString: [UtilityBridge сonvertCStringToNSString: notification]];
  
  NSDictionary *payload = [automationsBridge getNotificationCustomPayload:notificationInfo];
  
  if (payload == nil) {
    return nil;
  }

  const char *data = [UtilityBridge jsonStringFromObject:payload];

  return data;
}

void _subscribeOnAutomationEvents() {
    [automationsBridge subscribe];
}
