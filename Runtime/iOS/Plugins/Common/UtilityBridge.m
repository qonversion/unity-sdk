#import "UtilityBridge.h"

static NSString *const nativeModuleErrorCode = @"NativeModuleError";

@implementation UtilityBridge

+ (NSString*)convertCStringToNSString:(const char *)string {
    if (string == NULL) {
        return nil;
    }
    
    return [NSString stringWithUTF8String:string];
}

+ (NSDictionary*)dictionaryFromJsonString:(NSString*) jsonString {
    if (!jsonString) {
        return @{};
    }
    
    NSData *jsonData = [jsonString dataUsingEncoding:NSUTF8StringEncoding];
    NSDictionary *dictionary = [NSJSONSerialization JSONObjectWithData:jsonData options:kNilOptions error:nil];
    
    return dictionary;
}

+ (NSArray*)arrayFromJsonString:(NSString*) jsonString {
    if (!jsonString) {
        return @[];
    }

    NSData *jsonData = [jsonString dataUsingEncoding:NSUTF8StringEncoding];
    NSArray *array = [NSJSONSerialization JSONObjectWithData:jsonData options:kNilOptions error:nil];
    
    return array;
}

+ (NSDictionary *)serializeErrorWithCode:(NSString *)code
                                  domain:(NSString *)domain
                             description:(NSString *)description 
                       additionalMessage:(NSString *)additionalMessage {
    NSMutableDictionary *errorDict = [NSMutableDictionary new];
    errorDict[@"code"] = code;
    errorDict[@"domain"] = domain;
    errorDict[@"description"] = description;
    errorDict[@"additionalMessage"] = additionalMessage;
    
    NSMutableDictionary *result = [NSMutableDictionary new];
    result[@"error"] = errorDict;
    
    return [result copy];
}

+ (NSDictionary *)serializeSandwichError:(SandwichError *)error {
    NSDictionary *errorDict = [UtilityBridge serializeErrorWithCode:error.code
                                                                    domain:error.domain
                                                               description:error.details
                                                         additionalMessage:error.additionalMessage];
    
    return [errorDict copy];
}

+ (void)handleErrorResponse:(SandwichError *)error toMethod:(NSString *)methodName
              unityListener:(const char *)unityListenerName {
    NSDictionary *errorDict = [UtilityBridge serializeSandwichError:error];
    [UtilityBridge sendUnityMessage:errorDict toMethod:methodName unityListener:unityListenerName];
}

+ (void)handleLocalError:(NSError *)error
                 message:(NSString *)message
                toMethod:(NSString *)methodName
           unityListener:(const char *)unityListenerName {
    NSDictionary *errorDict = [UtilityBridge serializeErrorWithCode:nativeModuleErrorCode
                                                             domain:error.domain
                                                        description:message
                                                  additionalMessage:error.localizedDescription];
    [UtilityBridge sendUnityMessage:errorDict toMethod:methodName unityListener:unityListenerName];
}

+ (const char *)jsonStringFromObject:(NSObject *)objectToConvert {
    if (objectToConvert == nil) {
        return nil;
    }
    
    NSError *error = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:objectToConvert options:0 error:&error];
    
    if (error) {
        NSLog(@"An error occurred while serializing data: %@", error.localizedDescription);
        return nil;
    }

    if (jsonData) {
        NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        return json.UTF8String;
    }
    return nil;
}

+ (void)sendUnityMessage:(NSObject *)objectToConvert
                toMethod:(NSString *)methodName
           unityListener:(const char *)unityListenerName {
    const char *data = [UtilityBridge jsonStringFromObject:objectToConvert];
    
    if (data) {
      UnitySendMessage(unityListenerName, methodName.UTF8String, data);
    }
    return;
}

+ (void)handleResult:(NSDictionary *)result
               error:(SandwichError *)error
        callbackName:(NSString *)callbackName
       unityListener:(const char *)unityListenerName {
    if (error) {
        [UtilityBridge handleErrorResponse:error toMethod:callbackName unityListener:unityListenerName];
    } else {
        [UtilityBridge sendUnityMessage:result toMethod:callbackName unityListener:unityListenerName];
    }
}

@end
