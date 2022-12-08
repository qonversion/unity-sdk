//
//  QNUAutomationsDelegate.h
//  Unity-iPhone
//
//  Created by Surik Sarkisyan on 15.03.2022.
//

#import <Foundation/Foundation.h>
@import QonversionSandwich;

NS_ASSUME_NONNULL_BEGIN

@interface QNUAutomationsDelegate : NSObject <AutomationsEventListener>

- (instancetype)initWithListenerName:(char *)unityListenerName;
- (void)subscribe;
- (void)setNotificationsToken:(NSString *)token;
- (BOOL)handleNotification:(NSDictionary *)notificationInfo;
- (NSDictionary *)getNotificationCustomPayload:(NSDictionary *)payload;

@end

NS_ASSUME_NONNULL_END
