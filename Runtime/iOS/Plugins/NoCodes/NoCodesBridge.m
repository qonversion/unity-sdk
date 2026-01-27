#import "UtilityBridge.h"
#import "QNUNoCodesDelegate.h"
@import QonversionSandwich;

char* noCodesUnityListenerName = nil;

static QNUNoCodesDelegate *noCodesBridge;

void _initializeNoCodes(const char* unityListener, const char* projectKey, const char* proxyUrl, const char* locale, const char* theme, const char* sdkVersion) {
    unsigned long len = strlen(unityListener);
    noCodesUnityListenerName = malloc(len + 1);
    strcpy(noCodesUnityListenerName, unityListener);
    
    noCodesBridge = [[QNUNoCodesDelegate alloc] initWithListenerName:noCodesUnityListenerName];
    
    NSString *projectKeyStr = [UtilityBridge convertCStringToNSString:projectKey];
    NSString *proxyUrlStr = [UtilityBridge convertCStringToNSString:proxyUrl];
    NSString *localeStr = [UtilityBridge convertCStringToNSString:locale];
    NSString *themeStr = [UtilityBridge convertCStringToNSString:theme];
    NSString *sdkVersionStr = [UtilityBridge convertCStringToNSString:sdkVersion];
    
    [noCodesBridge initializeWithProjectKey:projectKeyStr proxyUrl:proxyUrlStr locale:localeStr theme:themeStr sdkVersion:sdkVersionStr];
}

void _setNoCodesDelegate() {
    [noCodesBridge setDelegate];
}

void _setNoCodesScreenPresentationConfig(const char* configData, const char* contextKey) {
    NSDictionary *config = [UtilityBridge dictionaryFromJsonString:[UtilityBridge convertCStringToNSString:configData]];
    NSString *contextKeyStr = [UtilityBridge convertCStringToNSString:contextKey];
    [noCodesBridge setScreenPresentationConfig:config contextKey:contextKeyStr];
}

void _showNoCodesScreen(const char* contextKey) {
    NSString *contextKeyStr = [UtilityBridge convertCStringToNSString:contextKey];
    [noCodesBridge showScreen:contextKeyStr];
}

void _closeNoCodes() {
    [noCodesBridge close];
}

void _setNoCodesLocale(const char* locale) {
    NSString *localeStr = [UtilityBridge convertCStringToNSString:locale];
    [noCodesBridge setLocale:localeStr];
}

void _setNoCodesTheme(const char* theme) {
    NSString *themeStr = [UtilityBridge convertCStringToNSString:theme];
    [noCodesBridge setTheme:themeStr];
}

void _setNoCodesPurchaseDelegate() {
    [noCodesBridge setPurchaseDelegate];
}

void _noCodesDelegatedPurchaseCompleted() {
    [noCodesBridge delegatedPurchaseCompleted];
}

void _noCodesDelegatedPurchaseFailed(const char* errorMessage) {
    NSString *errorMessageStr = [UtilityBridge convertCStringToNSString:errorMessage];
    [noCodesBridge delegatedPurchaseFailed:errorMessageStr];
}

void _noCodesDelegatedRestoreCompleted() {
    [noCodesBridge delegatedRestoreCompleted];
}

void _noCodesDelegatedRestoreFailed(const char* errorMessage) {
    NSString *errorMessageStr = [UtilityBridge convertCStringToNSString:errorMessage];
    [noCodesBridge delegatedRestoreFailed:errorMessageStr];
}
