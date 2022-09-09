#import <Foundation/Foundation.h>
@import QonversionSandwich;

@interface UtilityBridge : NSObject

+ (NSString*)сonvertCStringToNSString:(const char *)string;
+ (NSDictionary*)dictionaryFromJsonString:(NSString*) jsonString;
+ (NSDictionary *)convertError:(SandwichError *)error;
+ (NSDictionary *)convertSandwichError:(SandwichError *)error;

+ (void)sendUnityMessage:(NSObject *)objectToConvert toMethod:(NSString *)methodName
           unityListener:(const char *)unityListenerName;

+ (void)handleErrorResponse:(SandwichError *)error toMethod:(NSString *)methodName
              unityListener:(const char *)unityListenerName;

+ (void)handleResult:(NSDictionary *)result
               error:(SandwichError *)error
        callbackName:(NSString *)callbackName
       unityListener:(const char *)unityListenerName;

@end
