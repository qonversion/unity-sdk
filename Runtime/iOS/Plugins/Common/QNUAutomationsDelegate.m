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
@property (nonatomic, strong) NSDictionary *automationEvents;

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

- (void)automationDidTriggerWithEvent:(NSString * _Nonnull)event payload:(NSDictionary<NSString *,id> * _Nullable)payload {
    NSString *methodName = self.automationEvents[event];

    [UtilityBridge sendUnityMessage:payload ?: @{} toMethod:methodName unityListener: listenerName];
}

@end
