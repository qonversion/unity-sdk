#import "UtilityBridge.h"
#import "QNUAutomationsDelegate.h"
@import QonversionSandwich;

char* automationsUnityListenerName = nil;

static QNUAutomationsDelegate *automationsBridge;

void _initializeAutomations(const char* unityListener) {
    unsigned long len = strlen(unityListener);
    automationsUnityListenerName = malloc(len + 1);
    strcpy(automationsUnityListenerName, unityListener);

    automationsBridge = [[QNUAutomationsDelegate alloc] initWithListenerName:automationsUnityListenerName];
}

void _setNotificationsToken(const char* token) {
    NSString *tokenStr = [UtilityBridge convertCStringToNSString:token];
    [automationsBridge setNotificationsToken:tokenStr];
}

bool _handleNotification(const char* notification) {
    NSDictionary *notificationInfo = [UtilityBridge dictionaryFromJsonString:[UtilityBridge convertCStringToNSString:notification]];
    
    BOOL isQonversionNotification = [automationsBridge handleNotification:notificationInfo];
    
    return isQonversionNotification;
}

char* _getNotificationCustomPayload(const char* notification) {
  NSDictionary *notificationInfo = [UtilityBridge dictionaryFromJsonString:[UtilityBridge convertCStringToNSString:notification]];
  
  NSDictionary *payload = [automationsBridge getNotificationCustomPayload:notificationInfo];
  
  if (payload == nil) {
    return nil;
  }

  const char *data = [UtilityBridge jsonStringFromObject:payload];

  char* cString = (char*)malloc(strlen(data) + 1);
  strcpy(cString, data);
    
  return cString;
}

void _showScreen(const char* screenId, const char* unityCallbackName) {
    NSString *callbackName = [UtilityBridge convertCStringToNSString:unityCallbackName];
    NSString *screenIdStr = [UtilityBridge convertCStringToNSString:screenId];
    [automationsBridge showScreenWithId:screenIdStr callbackName:callbackName];
}

void _subscribeOnAutomationEvents() {
    [automationsBridge subscribe];
}

void _setScreenPresentationConfig(const char* configData, const char* screenId) {
    NSDictionary *config = [UtilityBridge dictionaryFromJsonString:[UtilityBridge convertCStringToNSString:configData]];
    NSString *screenIdStr = [UtilityBridge convertCStringToNSString:screenId];
    [automationsBridge setScreenPresentationConfig:config screenId:screenIdStr];
}
