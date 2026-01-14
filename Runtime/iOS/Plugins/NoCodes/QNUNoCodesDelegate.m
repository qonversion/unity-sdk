//
//  QNUNoCodesDelegate.m
//  Unity-iPhone
//

#import "QNUNoCodesDelegate.h"
#import "UtilityBridge.h"

static NSString *const kEventScreenShown = @"OnNoCodesScreenShown";
static NSString *const kEventActionStarted = @"OnNoCodesActionStarted";
static NSString *const kEventActionFailed = @"OnNoCodesActionFailed";
static NSString *const kEventActionFinished = @"OnNoCodesActionFinished";
static NSString *const kEventFinished = @"OnNoCodesFinished";
static NSString *const kEventScreenFailedToLoad = @"OnNoCodesScreenFailedToLoad";
static NSString *const kEventPurchase = @"OnNoCodesPurchase";
static NSString *const kEventRestore = @"OnNoCodesRestore";

char* noCodesListenerName = nil;

@interface QNUNoCodesDelegate ()

@property (nonatomic, strong, readwrite) NoCodesSandwich *noCodesSandwich;
@property (nonatomic, copy) NSDictionary *noCodesEvents;

@end

@implementation QNUNoCodesDelegate

- (instancetype)initWithListenerName:(char *)unityListenerName {
    self = [super init];
    
    if (self) {
        noCodesListenerName = unityListenerName;
        _noCodesEvents = @{
            @"nocodes_screen_shown": kEventScreenShown,
            @"nocodes_action_started": kEventActionStarted,
            @"nocodes_action_failed": kEventActionFailed,
            @"nocodes_action_finished": kEventActionFinished,
            @"nocodes_finished": kEventFinished,
            @"nocodes_screen_failed_to_load": kEventScreenFailedToLoad
        };
    }
    
    return self;
}

- (void)initializeWithProjectKey:(NSString *)projectKey proxyUrl:(NSString * _Nullable)proxyUrl locale:(NSString * _Nullable)locale sdkVersion:(NSString *)sdkVersion {
    _noCodesSandwich = [[NoCodesSandwich alloc] initWithNoCodesEventListener:self];
    
    NSString *effectiveProxyUrl = proxyUrl.length > 0 ? proxyUrl : nil;
    NSString *effectiveLocale = locale.length > 0 ? locale : nil;
    
    [_noCodesSandwich initializeWithProjectKey:projectKey proxyUrl:effectiveProxyUrl locale:effectiveLocale];
    [_noCodesSandwich storeSdkInfoWithSource:@"unity" version:sdkVersion];
}

- (void)setDelegate {
    // Delegate is already set in initWithNoCodesEventListener
}

- (void)setScreenPresentationConfig:(NSDictionary *)config contextKey:(NSString *)contextKey {
    NSString *effectiveContextKey = contextKey.length > 0 ? contextKey : nil;
    [self.noCodesSandwich setScreenPresentationConfig:config forContextKey:effectiveContextKey];
}

- (void)showScreen:(NSString *)contextKey {
    [self.noCodesSandwich showScreen:contextKey];
}

- (void)close {
    [self.noCodesSandwich close];
}

- (void)setLocale:(NSString *)locale {
    NSString *effectiveLocale = locale.length > 0 ? locale : nil;
    [self.noCodesSandwich setLocale:effectiveLocale];
}

- (void)setPurchaseDelegate {
    [self.noCodesSandwich setPurchaseDelegate:self];
}

- (void)delegatedPurchaseCompleted {
    [self.noCodesSandwich delegatedPurchaseCompleted];
}

- (void)delegatedPurchaseFailed:(NSString *)errorMessage {
    [self.noCodesSandwich delegatedPurchaseFailed:errorMessage];
}

- (void)delegatedRestoreCompleted {
    [self.noCodesSandwich delegatedRestoreCompleted];
}

- (void)delegatedRestoreFailed:(NSString *)errorMessage {
    [self.noCodesSandwich delegatedRestoreFailed:errorMessage];
}

#pragma mark - NoCodesEventListener

- (void)noCodesDidTriggerWithEvent:(NSString *)event payload:(NSDictionary<NSString *, id> *)payload {
    NSString *methodName = self.noCodesEvents[event];
    
    if (methodName) {
        [UtilityBridge sendUnityMessage:payload ?: @{} toMethod:methodName unityListener:noCodesListenerName];
    }
}

#pragma mark - NoCodesPurchaseDelegateBridge

- (void)purchase:(NSDictionary<NSString *, id> *)productData {
    [UtilityBridge sendUnityMessage:productData toMethod:kEventPurchase unityListener:noCodesListenerName];
}

- (void)restore {
    [UtilityBridge sendUnityMessage:@{} toMethod:kEventRestore unityListener:noCodesListenerName];
}

@end
