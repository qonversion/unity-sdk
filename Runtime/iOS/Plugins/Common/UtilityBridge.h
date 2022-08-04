#import <Foundation/Foundation.h>
#import <Qonversion/Qonversion.h>
#import "QNUConstants.h"

@interface UtilityBridge : NSObject

+ (const char*)convertNSStringToCString:(NSString*)nsString;
+ (NSString*)сonvertCStringToNSString:(const char *)string;
+ (NSDictionary*)dictionaryFromJsonString:(NSString*) jsonString;

+ (NSNumber *)convertProperty:(NSString *)propertyStr;
+ (NSArray *)convertPermissions:(NSArray<QNPermission *> *)permissions;
+ (NSArray *)convertProducts:(NSArray<QNProduct *> *)products;
+ (QNProduct *)convertProductFromJson:(NSString *)productJson;
+ (NSDictionary *)convertOfferings:(QNOfferings *)offerings;
+ (NSDictionary *)convertIntroEligibility:(NSDictionary<NSString *, QNIntroEligibility *> *)introEligibilityInfo;
+ (NSDictionary *)convertError:(NSError *)error;
+ (NSDictionary *)convertPlainError:(NSError *)error;

+ (void)sendUnityMessage:(NSObject *)objectToConvert toMethod:(NSString *)methodName
           unityListener:(const char *)unityListenerName;

+ (void)handlePermissionsResponse:(NSDictionary<NSString *,QNPermission *> *) result withError:( NSError *)error
                         toMethod:(NSString *)methodName
                    unityListener:(const char *)unityListenerName;

+ (void)handlePurchaseResponse:(NSDictionary<NSString *,QNPermission *> *) permissions
                   isCancelled:(BOOL) cancelled
                     withError:(NSError *) error
                      toMethod:(NSString *) methodName
                 unityListener:(const char *) unityListenerName;

+ (void)handleErrorResponse:(NSError *)error toMethod:(NSString *) methodName
              unityListener:(const char *)unityListenerName;

+ (NSData *)convertHexToData:(NSString *)hex;
+ (NSDictionary *)convertActionResult:(QONActionResult *)actionResult;
+ (NSDictionary *)convertError:(NSError *)error;
+ (NSDictionary *)convertAutomationsEvent:(QONAutomationsEvent *)event;

@end
