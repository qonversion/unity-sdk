#import <Qonversion/Qonversion.h>
#import "UtilityBridge.h"
#import "QNUAutomationsDelegate.h"

char* unityListenerName = nil;

@interface PurchasesDelegateWrapper : NSObject <QNPurchasesDelegate, QNPromoPurchasesDelegate>

- (void)qonversionDidReceiveUpdatedPermissions:(NSDictionary<NSString *, QNPermission *>  *_Nonnull)permissions;
- (void)shouldPurchasePromoProductWithIdentifier:(NSString *)productID executionBlock:(QNPromoPurchaseCompletionHandler)executionBlock;

@property (nonatomic, strong) NSMutableDictionary *promoPurchasesExecutionBlocks;

@end

@implementation PurchasesDelegateWrapper

- (void)shouldPurchasePromoProductWithIdentifier:(NSString *)productID executionBlock:(QNPromoPurchaseCompletionHandler)executionBlock {
    if (!_promoPurchasesExecutionBlocks) {
        _promoPurchasesExecutionBlocks = [[NSMutableDictionary alloc] init];
    }
    [_promoPurchasesExecutionBlocks setObject:executionBlock forKey:productID];

    UnitySendMessage(unityListenerName, "OnReceivePromoPurchase", productID.UTF8String);
}

- (void)qonversionDidReceiveUpdatedPermissions:(NSDictionary<NSString *, QNPermission *>  *_Nonnull)permissions {
    NSArray *permissionsArray = [UtilityBridge convertPermissions:permissions.allValues];
    [UtilityBridge sendUnityMessage:permissionsArray toMethod:@"OnReceiveUpdatedPurchases" unityListener: unityListenerName];
}

@end

static PurchasesDelegateWrapper *purchasesDelegate;
static PurchasesDelegateWrapper *promoPurchasesDelegate;
static QNUAutomationsDelegate *automationsDelegate;

void _storeSdkInfo(const char* version, const char* versionKey, const char* source, const char* sourceKey) {
    NSString *versionStr = [UtilityBridge сonvertCStringToNSString:version];
    NSString *versionKeyStr = [UtilityBridge сonvertCStringToNSString:versionKey];
    NSString *sourceStr = [UtilityBridge сonvertCStringToNSString:source];
    NSString *sourceKeyStr = [UtilityBridge сonvertCStringToNSString:sourceKey];
    
    [[NSUserDefaults standardUserDefaults] setValue:versionStr forKey:versionKeyStr];
    [[NSUserDefaults standardUserDefaults] setValue:sourceStr forKey:sourceKeyStr];
}

void _setDebugMode() {
    [Qonversion setDebugMode];
}

void _launchWithKey(const char* unityListener, const char* key) {
    unsigned long len = strlen(unityListener);
    unityListenerName = malloc(len + 1);
    strcpy(unityListenerName, unityListener);
    
    [Qonversion launchWithKey:[UtilityBridge сonvertCStringToNSString:key]];
}

void _setAdvertisingID() {
    [Qonversion setAdvertisingID];
}

void _presentCodeRedemptionSheet() {
    if (@available(iOS 14.0, *)) {
        [Qonversion presentCodeRedemptionSheet];
    }
}

void _setAppleSearchAdsAttributionEnabled(const bool enable) {
    [Qonversion setAppleSearchAdsAttributionEnabled:enable];
}

void _setProperty(const char* propertyName, const char* value) {
    NSString *propertyNameStr = [UtilityBridge сonvertCStringToNSString:propertyName];
    NSString *valueStr = [UtilityBridge сonvertCStringToNSString:value];
    NSNumber *propertyIndex = [UtilityBridge convertProperty:propertyNameStr];
    
    if (propertyIndex) {
        [Qonversion setProperty:propertyIndex.integerValue value:valueStr];
    }
}

void _setUserProperty(const char* key, const char* value) {
    NSString *keyStr = [UtilityBridge сonvertCStringToNSString:key];
    NSString *valueStr = [UtilityBridge сonvertCStringToNSString:value];

    [Qonversion setUserProperty:keyStr value:valueStr];
}

void _addAttributionData(const char* conversionData, const int provider) {
    NSDictionary *conversionInfo = [UtilityBridge dictionaryFromJsonString: [UtilityBridge сonvertCStringToNSString: conversionData]];
    
    [Qonversion addAttributionData:conversionInfo
                      fromProvider:(QNAttributionProvider)provider];
}

void _identify(const char* userId) {
    NSString *userIdStr = [UtilityBridge сonvertCStringToNSString:userId];
    [Qonversion identify:userIdStr];
}

void _logout() {
    [Qonversion logout];
}

void _checkPermissions(const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge сonvertCStringToNSString:unityCallbackName];
    
    [Qonversion checkPermissions:^(NSDictionary<NSString *,QNPermission *> *result, NSError *error) {
        [UtilityBridge handlePermissionsResponse:result withError:error toMethod:callbackName unityListener:unityListenerName];
    }];
}

void _restore(const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge сonvertCStringToNSString:unityCallbackName];

    [Qonversion restoreWithCompletion:^(NSDictionary *result, NSError *error) {
        [UtilityBridge handlePermissionsResponse:result withError:error toMethod:callbackName unityListener:unityListenerName];
    }];
}

void _purchase(const char* productId, const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge сonvertCStringToNSString:unityCallbackName];

    [Qonversion purchase:[UtilityBridge сonvertCStringToNSString:productId] completion:^(NSDictionary *result, NSError *error, BOOL cancelled) {
        [UtilityBridge handlePermissionsResponse:result withError:error toMethod:callbackName unityListener:unityListenerName];
    }];
}

void _purchaseProduct(const char* productJson, const char* unityCallbackName) {
    NSString *productJsonStr = [UtilityBridge сonvertCStringToNSString:productJson];
    NSString *callbackName = [UtilityBridge сonvertCStringToNSString:unityCallbackName];

    QNProduct *product = [UtilityBridge convertProductFromJson: productJsonStr];
    [Qonversion purchaseProduct:product completion:^(NSDictionary<NSString *,QNPermission *> * _Nonnull result,
                                                     NSError * _Nullable error,
                                                     BOOL cancelled) {
        [UtilityBridge handlePermissionsResponse:result withError:error toMethod:callbackName unityListener:unityListenerName];
    }];
}

void _products(const char* unityCallbackName) {
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

void _offerings(const char* unityCallbackName) {
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

void _checkTrialIntroEligibilityForProductIds(const char* productIdsJson, const char* unityCallbackName) {
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
        [Qonversion checkTrialIntroEligibilityForProductIds:products completion:^(NSDictionary<NSString *,QNIntroEligibility *> * _Nonnull result, NSError * _Nullable error) {
            if (error) {
                [UtilityBridge handleErrorResponse:error toMethod:callbackName unityListener:unityListenerName];
                return;
            }
            
            NSDictionary *eligibilities = [UtilityBridge convertIntroEligibility:result];
            [UtilityBridge sendUnityMessage:eligibilities toMethod:callbackName unityListener: unityListenerName];
        }];
    }
}

void _promoPurchase(const char* storeProductId, const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge сonvertCStringToNSString:unityCallbackName];
    NSString *storeProductIdStr = [UtilityBridge сonvertCStringToNSString:storeProductId];
    QNPromoPurchaseCompletionHandler executionBlock = [promoPurchasesDelegate.promoPurchasesExecutionBlocks objectForKey:storeProductIdStr];
    if (executionBlock) {
        [promoPurchasesDelegate.promoPurchasesExecutionBlocks removeObjectForKey:storeProductIdStr];
        QNPurchaseCompletionHandler completion = ^(NSDictionary<NSString *, QNPermission*> *result, NSError  *_Nullable error, BOOL cancelled) {
            [UtilityBridge handlePermissionsResponse:result withError:error toMethod:callbackName unityListener:unityListenerName];
        };

        executionBlock(completion);
    } else {
        NSError *error = [NSError errorWithDomain:keyQNErrorDomain code:QNErrorProductNotFound userInfo:nil];
        [UtilityBridge handleErrorResponse:error toMethod:callbackName unityListener:unityListenerName];
    }
}

void _addPromoPurchasesDelegate() {
    if (!promoPurchasesDelegate) {
        promoPurchasesDelegate = [PurchasesDelegateWrapper alloc];
    }
    [Qonversion setPromoPurchasesDelegate:promoPurchasesDelegate];
}

void _removePromoPurchasesDelegate() {
    promoPurchasesDelegate = nil;
 }

void _addUpdatedPurchasesDelegate() {
    if (!purchasesDelegate) {
        purchasesDelegate = [PurchasesDelegateWrapper alloc];
    }
    [Qonversion setPurchasesDelegate:purchasesDelegate];
}

void _removeUpdatedPurchasesDelegate() {
    purchasesDelegate = nil;
}

void _setNotificationsToken(const char* token) {
    NSString *hexString = [UtilityBridge сonvertCStringToNSString:token];
    NSData *tokenData = [UtilityBridge convertHexToData:hexString];
    
    [Qonversion setNotificationsToken:tokenData];
}

bool _handleNotification(const char* notification) {
    NSDictionary *notificationInfo = [UtilityBridge dictionaryFromJsonString: [UtilityBridge сonvertCStringToNSString: notification]];
    
    BOOL result = [Qonversion handleNotification:notificationInfo];
    
    return result;
}

void _subscribeAutomationsDelegate() {
    automationsDelegate = [[QNUAutomationsDelegate alloc] initWithListenerName:unityListenerName];
}
