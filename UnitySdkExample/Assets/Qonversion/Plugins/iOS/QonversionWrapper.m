#import "Qonversion.h"

#pragma mark Utility Methods

NSString *convertCString(const char *string) {
    if (string)
        return [NSString stringWithUTF8String:string];
    else
        return nil;
}

char *makeStringCopy(NSString *nstring) {
    if ((!nstring) || (nil == nstring) || (nstring == (id) [NSNull null]) || (0 == nstring.length))
        return NULL;

    const char *string = [nstring UTF8String];

    if (string == NULL)
        return NULL;

    char *res = (char *) malloc(strlen(string) + 1);
    strcpy(res, string);

    return res;
}

#pragma mark QonversionPurchases Wrapper

@implementation QonvUnityHelperDelegate : NSObject

- (void)setup:(NSString *)apiKey appUserID:(NSString *)appUserID gameObject:(NSString *)gameObject {
    NSLog(@"setup with %@ %@ %@", apiKey, appUserID, gameObject);
   [Qonversion launchWithKey:apiKey userID:appUserID];
}

@end

#pragma mark Bridging Methods

static QonvUnityHelperDelegate *_QonvUnityHelper;

static QonvUnityHelperDelegate *_QonvUnityHelperShared() {
    if (_QonvUnityHelper == nil) {
        _QonvUnityHelper = [[QonvUnityHelperDelegate alloc] init];
    }
    return _QonvUnityHelper;
}

void _QonvSetup(const char *gameObject, const char *apiKey, const char *appUserID) {
    [_QonvUnityHelperShared() setup:convertCString(apiKey) appUserID:convertCString(appUserID) gameObject:convertCString(gameObject)];
}