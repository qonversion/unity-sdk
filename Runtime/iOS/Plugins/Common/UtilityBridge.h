#import <Foundation/Foundation.h>
#import "Qonversion.h"
#import <Qonversion/QNOffering.h>
#import <Qonversion/QNOfferings.h>

@interface UtilityBridge : NSObject

+ (const char*)convertNSStringToCString:(NSString*)nsString;
+ (NSString*)сonvertCStringToNSString:(const char *)string;
+ (NSDictionary*)dictionaryFromJsonString:(NSString*) jsonString;

+ (NSArray *)convertPermissions:(NSArray<QNPermission *> *)permissions;
+ (NSArray *)convertProducts:(NSArray<QNProduct *> *)products;
+ (NSDictionary *)convertOfferings:(QNOfferings *)offerings;
+ (NSDictionary *)convertError:(NSError *)error;

+ (void)sendUnityMessage:(NSObject *)objectToConvert toMethod:(NSString *)methodName
           unityListener:(const char *)unityListenerName;

+ (void)handlePermissionsResponse:(NSDictionary<NSString *,QNPermission *> *) result withError:( NSError *)error
                         toMethod:(NSString *)methodName
                    unityListener:(const char *)unityListenerName;

+ (void)handleErrorResponse:(NSError *)error toMethod:(NSString *) methodName
              unityListener:(const char *)unityListenerName;
@end
