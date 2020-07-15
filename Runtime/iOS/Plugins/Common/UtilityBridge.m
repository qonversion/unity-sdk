#import "UtilityBridge.h"

@implementation UtilityBridge

+ (const char*)convertNSStringToCString:(NSString*) nsString {
    if (nsString == NULL)
        return NULL;

    const char* nsStringUtf8 = [nsString UTF8String];
    char* cString = (char*)malloc(strlen(nsStringUtf8) + 1);
    strcpy(cString, nsStringUtf8);

    return cString;
}

+ (NSString*)сonvertCStringToNSString:(const char *)string {
    if (string == NULL)
    {
        return nil;
    }

    return [NSString stringWithUTF8String:string];
}

+ (NSDictionary*)dictionaryFromJsonString:(NSString*) jsonString {
    NSData *jsonData = [jsonString dataUsingEncoding:NSUTF8StringEncoding];
    NSDictionary *dictionary = [NSJSONSerialization JSONObjectWithData:jsonData options:kNilOptions error:nil];
    
    return dictionary;
}

@end