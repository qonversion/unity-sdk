//
//  QNUAutomationsDelegate.m
//  Unity-iPhone
//
//  Created by Surik Sarkisyan on 15.03.2022.
//

#import "QNUAutomationsDelegate.h"
#import "UtilityBridge.h"

static NSString *const kEventScreenShown = @"OnAutomationsScreenShown";
static NSString *const kEventActionStarted = @"OnAutomationsActionStarted";
static NSString *const kEventActionFailed = @"OnAutomationsActionFailed";
static NSString *const kEventActionFinished = @"OnAutomationsActionFinished";
static NSString *const kEventAutomationsFinished = @"OnAutomationsFinished";

char* listenerName = nil;

@interface QNUAutomationsDelegate ()

@property (nonatomic, strong) AutomationsSandwich *automationsSandwich;
@property (nonatomic, copy) NSDictionary *automationEvents;

@end

@implementation QNUAutomationsDelegate

- (instancetype)initWithListenerName:(char *)unityListenerName {
    self = [super init];
    
    if (self) {
        listenerName = unityListenerName;
        _automationsSandwich = [AutomationsSandwich new];
        _automationEvents = @{
            @"automations_screen_shown": kEventScreenShown,
            @"automations_action_started": kEventActionStarted,
            @"automations_action_failed": kEventActionFailed,
            @"automations_action_finished": kEventActionFinished,
            @"automations_finished": kEventAutomationsFinished
        };
    }
    
    return self;
}

- (void)subscribe {
    [self.automationsSandwich subscribe:self];
}

- (void)setNotificationsToken:(NSString *)token {
    [self.automationsSandwich setNotificationToken:token];
}

- (BOOL)handleNotification:(NSDictionary *)notificationInfo {
    return [self.automationsSandwich handleNotification:notificationInfo];
}

- (NSDictionary *)getNotificationCustomPayload:(NSDictionary *)payload {
    return [self.automationsSandwich getNotificationCustomPayload:payload];
}

- (void)automationDidTriggerWithEvent:(NSString * _Nonnull)event payload:(NSDictionary<NSString *,id> * _Nullable)payload {
    NSString *methodName = self.automationEvents[event];

    [UtilityBridge sendUnityMessage:payload ?: @{} toMethod:methodName unityListener: listenerName];
}

- (void)showScreenWithId:(NSString *)screenId callbackName:(NSString *)callbackName {
    [self.automationsSandwich showScreen:screenId completion:^(NSDictionary<NSString *,id> * _Nullable result, SandwichError * _Nullable error) {
        [UtilityBridge handleResult:result error:error callbackName:callbackName unityListener:listenerName];
    }];
}

- (void)setScreenPresentationConfig:(NSDictionary *)config screenId:(NSString *)screenId {
    [self.automationsSandwich setScreenPresentationConfig:config forScreenId:screenId];
}

@end
