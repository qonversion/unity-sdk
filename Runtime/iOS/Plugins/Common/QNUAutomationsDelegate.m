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

char* unityListenerName = nil;

@implementation QNUAutomationsDelegate

- (instancetype)initWithListenerName:(char *)listenerName {
    self = [super init];
    
    if (self) {
        unityListenerName = listenerName;
        [QONAutomations setDelegate:self];
    }
    
    return self;
}

- (void)automationsDidShowScreen:(NSString * _Nonnull)screenID {
    [UtilityBridge sendUnityMessage:@{@"screenID": screenID} toMethod:kEventScreenShown unityListener: unityListenerName];
}

- (void)automationsDidStartExecutingActionResult:(QONActionResult * _Nonnull)actionResult {
    NSDictionary *actionResultDict = [UtilityBridge convertActionResult:actionResult];
    [UtilityBridge sendUnityMessage:actionResultDict toMethod:kEventActionStarted unityListener: unityListenerName];
}

- (void)automationsDidFailExecutingActionResult:(QONActionResult * _Nonnull)actionResult {
    NSDictionary *actionResultDict = [UtilityBridge convertActionResult:actionResult];
    [UtilityBridge sendUnityMessage:actionResultDict toMethod:kEventActionFailed unityListener: unityListenerName];
}

- (void)automationsDidFinishExecutingActionResult:(QONActionResult * _Nonnull)actionResult {
    NSDictionary *actionResultDict = [UtilityBridge convertActionResult:actionResult];
    [UtilityBridge sendUnityMessage:actionResultDict toMethod:kEventActionFinished unityListener: unityListenerName];
}

- (void)automationsFinished {
    [UtilityBridge sendUnityMessage:@{} toMethod:kEventAutomationsFinished unityListener: unityListenerName];
}

@end
