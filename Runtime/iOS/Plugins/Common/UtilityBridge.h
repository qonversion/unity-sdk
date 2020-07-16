#import <Foundation/Foundation.h>

@interface UtilityBridge : NSObject

+ (const char*)convertNSStringToCString:(NSString*)nsString;
+ (NSString*)сonvertCStringToNSString:(const char *)string;
+ (NSDictionary*)dictionaryFromJsonString:(NSString*) jsonString;
@end