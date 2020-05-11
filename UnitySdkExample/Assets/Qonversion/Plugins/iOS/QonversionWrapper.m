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
//fromNetwork:(RCAttributionNetwork) network

- (void)addAttributionData:(NSString *)data provider:(NSInteger *)provider uid:(NSString *)uid {
    NSLog(@"add attribution data %@ %@ %@", data, provider, uid);
    NSDictionary *dict = dictionaryFromJsonString(data);
    [Qonversion addAttributionData:dict fromProvider:(QAttributionProvider)provider userID:uid];
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

void _QonvPushAttribution(const char *data, const int provider, const char *uid) {
    [_QonvUnityHelperShared() addAttributionData:convertCString(data) provider:provider uid:convertCString(uid)];
}

#pragma mark Helpers

static NSDictionary *dictionaryFromJsonString(const char *jsonString) {
    NSData *jsonData = [[NSData alloc] initWithBytes:jsonString length:strlen(jsonString)];
    NSDictionary *dictionary = [NSJSONSerialization JSONObjectWithData:jsonData options:kNilOptions error:nil];
    
    return dictionary;
}