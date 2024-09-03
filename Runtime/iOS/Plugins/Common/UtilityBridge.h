#import <Foundation/Foundation.h>
@import QonversionSandwich;

@interface UtilityBridge : NSObject

+ (NSString*)convertCStringToNSString:(const char *)string;
+ (NSDictionary*)dictionaryFromJsonString:(NSString*) jsonString;
+ (NSArray*)arrayFromJsonString:(NSString*) jsonString;
+ (NSDictionary *)serializeErrorWithCode:(NSString *)code
                                  domain:(NSString *)domain
                             description:(NSString *)description 
                       additionalMessage:(NSString *)additionalMessage;
+ (NSDictionary *)serializeSandwichError:(SandwichError *)error;
+ (const char *)jsonStringFromObject:(NSObject *)objectToConvert;

+ (void)sendUnityMessage:(NSObject *)objectToConvert toMethod:(NSString *)methodName
           unityListener:(const char *)unityListenerName;

+ (void)handleErrorResponse:(SandwichError *)error toMethod:(NSString *)methodName
              unityListener:(const char *)unityListenerName;
+ (void)handleLocalError:(NSError *)error
                 message:(NSString *)message
                toMethod:(NSString *)methodName
           unityListener:(const char *)unityListenerName;
+ (void)handleResult:(NSDictionary *)result
               error:(SandwichError *)error
        callbackName:(NSString *)callbackName
       unityListener:(const char *)unityListenerName;

@end
