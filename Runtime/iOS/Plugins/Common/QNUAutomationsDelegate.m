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

@implementation QNUAutomationsDelegate

- (instancetype)initWithListenerName:(char *)unityListenerName {
    self = [super init];
    
    if (self) {
        listenerName = unityListenerName;
        [QONAutomations setDelegate:self];
    }
    
    return self;
}

- (void)automationsDidShowScreen:(NSString * _Nonnull)screenID {
    [UtilityBridge sendUnityMessage:@{@"screenID": screenID} toMethod:kEventScreenShown unityListener: listenerName];
}

- (void)automationsDidStartExecutingActionResult:(QONActionResult * _Nonnull)actionResult {
    NSDictionary *actionResultDict = [UtilityBridge convertActionResult:actionResult];
    [UtilityBridge sendUnityMessage:actionResultDict toMethod:kEventActionStarted unityListener: listenerName];
}

- (void)automationsDidFailExecutingActionResult:(QONActionResult * _Nonnull)actionResult {
    NSDictionary *actionResultDict = [UtilityBridge convertActionResult:actionResult];
    [UtilityBridge sendUnityMessage:actionResultDict toMethod:kEventActionFailed unityListener: listenerName];
}

- (void)automationsDidFinishExecutingActionResult:(QONActionResult * _Nonnull)actionResult {
    NSDictionary *actionResultDict = [UtilityBridge convertActionResult:actionResult];
    [UtilityBridge sendUnityMessage:actionResultDict toMethod:kEventActionFinished unityListener: listenerName];
}

- (void)automationsFinished {
    [UtilityBridge sendUnityMessage:@{} toMethod:kEventAutomationsFinished unityListener: listenerName];
}

@end
