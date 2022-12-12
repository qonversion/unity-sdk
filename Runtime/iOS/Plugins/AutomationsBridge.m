#import "UtilityBridge.h"
#import "QNUAutomationsDelegate.h"
@import QonversionSandwich;

char* unityListenerName = nil;

static QNUAutomationsDelegate *automationsBridge;

void _initialize(const char* unityListener) {
    unsigned long len = strlen(unityListener);
    unityListenerName = malloc(len + 1);
    strcpy(unityListenerName, unityListener);

    automationsBridge = [[QNUAutomationsDelegate alloc] initWithListenerName:unityListenerName];
}

void _setNotificationsToken(const char* token) {
    NSString *tokenStr = [UtilityBridge сonvertCStringToNSString:token];
    [automationsBridge setNotificationsToken:tokenStr];
}

bool _handleNotification(const char* notification) {
    NSDictionary *notificationInfo = [UtilityBridge dictionaryFromJsonString: [UtilityBridge сonvertCStringToNSString: notification]];
    
    BOOL isQonversionNotification = [automationsBridge handleNotification:notificationInfo];
    
    return isQonversionNotification;
}

const char* _getNotificationCustomPayload(const char* notification) {
  NSDictionary *notificationInfo = [UtilityBridge dictionaryFromJsonString: [UtilityBridge сonvertCStringToNSString: notification]];
  
  NSDictionary *payload = [automationsBridge getNotificationCustomPayload:notificationInfo];
  
  if (payload == nil) {
    return nil;
  }

  const char *data = [UtilityBridge jsonStringFromObject:payload];

  return data;
}

void _showScreen(const char* screenId, const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge сonvertCStringToNSString:unityCallbackName];
    NSString *screenIdStr = [UtilityBridge сonvertCStringToNSString:screenId];
    [automationsBridge showScreenWithId:screenIdStr callbackName:callbackName];
}

void _subscribeOnAutomationEvents() {
    [automationsBridge subscribe];
}
