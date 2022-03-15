//
//  QNUAutomationsDelegate.h
//  Unity-iPhone
//
//  Created by Surik Sarkisyan on 15.03.2022.
//

#import <Foundation/Foundation.h>
#import "Qonversion.h"

NS_ASSUME_NONNULL_BEGIN

@interface QNUAutomationsDelegate : NSObject <QONAutomationsDelegate>

- (instancetype)initWithListenerName:(char *)listenerName;

@end

NS_ASSUME_NONNULL_END
