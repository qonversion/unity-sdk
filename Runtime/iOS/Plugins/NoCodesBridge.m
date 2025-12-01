#import "UtilityBridge.h"
@import QonversionSandwich;

char* noCodesUnityListenerName = nil;

@interface NoCodesEventListenerWrapper : NSObject <NoCodesEventListener>
@end

@implementation NoCodesEventListenerWrapper

- (void)noCodesDidTriggerWithEvent:(NSString *)event payload:(NSDictionary<NSString *,id> * _Nullable)payload {
    // Преобразуем события из NoCodesEvent в соответствующие методы Unity
    NSString *unityMethod = nil;
    
    if ([event isEqualToString:@"nocodes_screen_shown"]) {
        unityMethod = @"OnNoCodesScreenShown";
    } else if ([event isEqualToString:@"nocodes_finished"]) {
        unityMethod = @"OnNoCodesFinished";
    } else if ([event isEqualToString:@"nocodes_action_started"]) {
        unityMethod = @"OnNoCodesActionStarted";
    } else if ([event isEqualToString:@"nocodes_action_failed"]) {
        unityMethod = @"OnNoCodesActionFailed";
    } else if ([event isEqualToString:@"nocodes_action_finished"]) {
        unityMethod = @"OnNoCodesActionFinished";
    } else if ([event isEqualToString:@"nocodes_screen_failed_to_load"]) {
        unityMethod = @"OnNoCodesScreenFailedToLoad";
    } else {
        NSLog(@"Unknown NoCodes event: %@", event);
        return;
    }
    
    if (payload) {
        [UtilityBridge sendUnityMessage:payload toMethod:unityMethod unityListener:noCodesUnityListenerName];
    } else {
        [UtilityBridge sendUnityMessage:@{} toMethod:unityMethod unityListener:noCodesUnityListenerName];
    }
}

@end

// Use id instead of specific class type to work around Swift class visibility issues
static id noCodesSandwich;

void _initializeNoCodes(const char* unityListener) {
    unsigned long len = strlen(unityListener);
    noCodesUnityListenerName = malloc(len + 1);
    strcpy(noCodesUnityListenerName, unityListener);

    noCodesSandwich = [[NoCodesSandwich alloc] initWithNoCodesEventListener:[NoCodesEventListenerWrapper new]];
}

void _initializeNoCodesSdk(const char* projectKey, const char* proxyUrl) {
    if (!noCodesSandwich) {
        NSLog(@"Error: NoCodesSandwich not initialized. Call _initializeNoCodes first.");
        return;
    }
    
    NSString *projectKeyStr = [UtilityBridge convertCStringToNSString:projectKey];
    NSString *proxyUrlStr = [UtilityBridge convertCStringToNSString:proxyUrl];
    if ([proxyUrlStr isEqualToString:@""]) {
        proxyUrlStr = nil;
    }

    [noCodesSandwich initializeWithProjectKey:projectKeyStr proxyUrl:proxyUrlStr];
}

void _setScreenPresentationConfig(const char* configJson, const char* contextKey) {
    if (!noCodesSandwich) {
        NSLog(@"Error: NoCodesSandwich not initialized. Call _initializeNoCodes first.");
        return;
    }
    
    NSString *configJsonStr = [UtilityBridge convertCStringToNSString:configJson];
    NSString *contextKeyStr = [UtilityBridge convertCStringToNSString:contextKey];
    
    NSDictionary *config = [UtilityBridge dictionaryFromJsonString:configJsonStr];
    
    [noCodesSandwich setScreenPresentationConfig:config forContextKey:contextKeyStr];
}

void _showNoCodesScreen(const char* contextKey) {
    if (!noCodesSandwich) {
        NSLog(@"Error: NoCodesSandwich not initialized. Call _initializeNoCodes first.");
        return;
    }
    
    NSString *contextKeyStr = [UtilityBridge convertCStringToNSString:contextKey];
    [noCodesSandwich showScreen:contextKeyStr];
}

void _closeNoCodes() {
    if (!noCodesSandwich) {
        NSLog(@"Error: NoCodesSandwich not initialized. Call _initializeNoCodes first.");
        return;
    }
    
    [noCodesSandwich close];
}
