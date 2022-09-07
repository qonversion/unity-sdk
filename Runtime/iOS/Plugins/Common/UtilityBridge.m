#import "UtilityBridge.h"

@implementation UtilityBridge

+ (NSString*)сonvertCStringToNSString:(const char *)string {
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

+ (NSDictionary *)convertError:(SandwichError *)error {
    NSDictionary *errorDict = [UtilityBridge convertSandwichError:error];
    
    NSMutableDictionary *result = [NSMutableDictionary new];
    result[@"error"] = errorDict;
    result[@"isCancelled"] = error.additionalInfo[@"isCancelled"];
    
    return [result copy];
}

+ (NSDictionary *)convertSandwichError:(SandwichError *)error {
    NSMutableDictionary *errorDict = [NSMutableDictionary new];
    errorDict[@"code"] = error.code;
    errorDict[@"domain"] = error.domain;
    errorDict[@"description"] = error.details;
    errorDict[@"additionalMessage"] = error.additionalMessage;
    
    return [errorDict copy];
}

+ (void)handleErrorResponse:(SandwichError *)error toMethod:(NSString *) methodName
              unityListener:(const char *)unityListenerName{
    NSDictionary *errorDict = [UtilityBridge convertError:error];
    [UtilityBridge sendUnityMessage:errorDict toMethod:methodName unityListener: unityListenerName];
}

+ (void)sendUnityMessage:(NSObject *)objectToConvert toMethod:(NSString *)methodName
           unityListener:(const char *)unityListenerName{
    NSError *error = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:objectToConvert options:0 error:&error];
    
    if (error) {
        NSLog(@"An error occurred while serializing data: %@", error.localizedDescription);
        return;
    }
    if (jsonData) {
        NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        UnitySendMessage(unityListenerName, methodName.UTF8String, json.UTF8String);
    }
}

+ (void)handleResult:(NSDictionary *)result
               error:(SandwichError *)error
        callbackName:(NSString *)callbackName
       unityListener:(const char *)unityListenerName {
    if (error) {
        [UtilityBridge handleErrorResponse:error toMethod:callbackName unityListener:unityListenerName];
    } else {
        [UtilityBridge sendUnityMessage:result toMethod:callbackName unityListener: unityListenerName];
    }
}

@end
