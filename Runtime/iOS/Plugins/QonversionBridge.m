#import "UtilityBridge.h"
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

static QonversionSandwich *qonversionSandwich;

void _initialize(const char* unityListener) {
    unsigned long len = strlen(unityListener);
    unityListenerName = malloc(len + 1);
    strcpy(unityListenerName, unityListener);

    qonversionSandwich = [[QonversionSandwich alloc] initWithQonversionEventListener:[QonversionEventListenerWrapper new]];
}

void _initializeSdk(const char* projectKey, const char* launchMode, const char* environment, const char* entitlementsCacheLifetime, const char* proxyUrl) {
    NSString *keyStr = [UtilityBridge convertCStringToNSString:projectKey];
    NSString *launchModeStr = [UtilityBridge convertCStringToNSString:launchMode];
    NSString *envStr = [UtilityBridge convertCStringToNSString:environment];
    NSString *cacheLifetimeStr = [UtilityBridge convertCStringToNSString:entitlementsCacheLifetime];
    NSString *proxyUrlStr = [UtilityBridge convertCStringToNSString:proxyUrl];

    [qonversionSandwich initializeWithProjectKey:keyStr launchModeKey:launchModeStr environmentKey:envStr entitlementsCacheLifetimeKey:cacheLifetimeStr proxyUrl:proxyUrlStr];
}

void _storeSdkInfo(const char* version, const char* source) {
    NSString *versionStr = [UtilityBridge convertCStringToNSString:version];
    NSString *sourceStr = [UtilityBridge convertCStringToNSString:source];
    
    [qonversionSandwich storeSdkInfoWithSource:sourceStr version:versionStr];
}

void _syncHistoricalData() {
    [qonversionSandwich syncHistoricalData];
}

void _syncStoreKit2Purchases() {
    [qonversionSandwich syncStoreKit2Purchases];
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

void _setUserProperty(const char* propertyName, const char* value) {
    NSString *propertyStr = [UtilityBridge convertCStringToNSString:propertyName];
    NSString *valueStr = [UtilityBridge convertCStringToNSString:value];
    
    [qonversionSandwich setDefinedProperty:propertyStr value:valueStr];
}

void _setCustomUserProperty(const char* key, const char* value) {
    NSString *keyStr = [UtilityBridge convertCStringToNSString:key];
    NSString *valueStr = [UtilityBridge convertCStringToNSString:value];
    
    [qonversionSandwich setCustomProperty:keyStr value:valueStr];
}

void _userProperties(const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge convertCStringToNSString:unityCallbackName];

    [qonversionSandwich userProperties:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _addAttributionData(const char* conversionData, const char* provider) {
    NSDictionary *conversionInfo = [UtilityBridge dictionaryFromJsonString: [UtilityBridge convertCStringToNSString: conversionData]];
    NSString *providerStr = [UtilityBridge convertCStringToNSString:provider];

    [qonversionSandwich attributionWithProviderKey:providerStr value:conversionInfo];
}

void _identify(const char* userId, const char* unityCallbackName) {
    NSString *userIdStr = [UtilityBridge convertCStringToNSString:userId];
    NSString *callbackName = [UtilityBridge convertCStringToNSString:unityCallbackName];

    [qonversionSandwich identify:userIdStr :^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _userInfo(const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge convertCStringToNSString:unityCallbackName];

    [qonversionSandwich userInfo:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _logout() {
    [qonversionSandwich logout];
}

void _checkPermissions(const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge convertCStringToNSString:unityCallbackName];
    
    [qonversionSandwich checkEntitlements:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _restore(const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge convertCStringToNSString:unityCallbackName];

    [qonversionSandwich restore:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _purchase(const char* productId, const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge convertCStringToNSString:unityCallbackName];
    NSString *productIdStr = [UtilityBridge convertCStringToNSString:productId];
    
    [qonversionSandwich purchase:productIdStr completion:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _products(const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge convertCStringToNSString:unityCallbackName];
    
    [qonversionSandwich products:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _offerings(const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge convertCStringToNSString:unityCallbackName];
    
    [qonversionSandwich offerings:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _remoteConfig(const char* contextKey, const char* unityCallbackName) {
    NSString *contextKeyStr = [UtilityBridge convertCStringToNSString:contextKey];
    NSString *callbackName = [UtilityBridge convertCStringToNSString:unityCallbackName];

    [qonversionSandwich remoteConfig:contextKeyStr :^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        if (error) {
            NSDictionary *errorDict = [UtilityBridge convertError:error];
            NSMutableDictionary *mutableErrorDict = [errorDict mutableCopy];
            mutableErrorDict[@"contextKey"] = contextKeyStr;
            [UtilityBridge sendUnityMessage:mutableErrorDict toMethod:callbackName unityListener:unityListenerName];
        } else {
            [UtilityBridge sendUnityMessage:result toMethod:callbackName unityListener:unityListenerName];
        }
    }];
}

void _remoteConfigList(const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge convertCStringToNSString:unityCallbackName];
    
    [qonversionSandwich remoteConfigList:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _remoteConfigListForContextKeys(const char* contextKeysJson, const char* includeEmptyContextKey, const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge convertCStringToNSString:unityCallbackName];
    NSArray *contextKeys = [UtilityBridge arrayFromJsonString:[UtilityBridge convertCStringToNSString:contextKeysJson]];

    [qonversionSandwich remoteConfigList:contextKeys includeEmptyContextKey:includeEmptyContextKey :^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _attachUserToExperiment(const char* experimentId, const char* groupId, const char* unityCallbackName) {
    NSString *experimentIdStr = [UtilityBridge convertCStringToNSString:experimentId];
    NSString *groupIdStr = [UtilityBridge convertCStringToNSString:groupId];
    NSString *callbackName = [UtilityBridge convertCStringToNSString:unityCallbackName];
    
    [qonversionSandwich attachUserToExperimentWith:experimentIdStr groupId:groupIdStr completion:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _detachUserFromExperiment(const char* experimentId, const char* unityCallbackName) {
    NSString *experimentIdStr = [UtilityBridge convertCStringToNSString:experimentId];
    NSString *callbackName = [UtilityBridge convertCStringToNSString:unityCallbackName];
    
    [qonversionSandwich detachUserFromExperimentWith:experimentIdStr completion:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _attachUserToRemoteConfiguration(const char* remoteConfigurationId, const char* unityCallbackName) {
    NSString *remoteConfigurationIdStr = [UtilityBridge convertCStringToNSString:remoteConfigurationId];
    NSString *callbackName = [UtilityBridge convertCStringToNSString:unityCallbackName];
    
    [qonversionSandwich attachUserToRemoteConfigurationWith:remoteConfigurationIdStr completion:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _detachUserFromRemoteConfiguration(const char* remoteConfigurationId, const char* unityCallbackName) {
    NSString *remoteConfigurationIdStr = [UtilityBridge convertCStringToNSString:remoteConfigurationId];
    NSString *callbackName = [UtilityBridge convertCStringToNSString:unityCallbackName];
    
    [qonversionSandwich detachUserFromRemoteConfigurationWith:remoteConfigurationIdStr completion:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _isFallbackFileAccessible(const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge convertCStringToNSString:unityCallbackName];
    
    [qonversionSandwich isFallbackFileAccessibleWithCompletion:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}

void _checkTrialIntroEligibility(const char* productIdsJson, const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge convertCStringToNSString:unityCallbackName];
    NSString *productIdsJsonStr = [UtilityBridge convertCStringToNSString:productIdsJson];
    
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
    NSString *callbackName = [UtilityBridge convertCStringToNSString:unityCallbackName];
    NSString *storeProductIdStr = [UtilityBridge convertCStringToNSString:storeProductId];
    
    [qonversionSandwich promoPurchase:storeProductIdStr completion:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:unityListenerName];
    }];
}
