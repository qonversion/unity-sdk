//
//  QNUNoCodesDelegate.h
//  Unity-iPhone
//

#import <Foundation/Foundation.h>
@import QonversionSandwich;

NS_ASSUME_NONNULL_BEGIN

@interface QNUNoCodesDelegate : NSObject <NoCodesEventListener, NoCodesPurchaseDelegateBridge>

- (instancetype)initWithListenerName:(char *)unityListenerName;
- (void)initializeWithProjectKey:(NSString *)projectKey proxyUrl:(NSString * _Nullable)proxyUrl locale:(NSString * _Nullable)locale sdkVersion:(NSString *)sdkVersion;
- (void)setDelegate;
- (void)setScreenPresentationConfig:(NSDictionary *)config contextKey:(NSString * _Nullable)contextKey;
- (void)showScreen:(NSString *)contextKey;
- (void)close;
- (void)setLocale:(NSString * _Nullable)locale;
- (void)setPurchaseDelegate;
- (void)delegatedPurchaseCompleted;
- (void)delegatedPurchaseFailed:(NSString *)errorMessage;
- (void)delegatedRestoreCompleted;
- (void)delegatedRestoreFailed:(NSString *)errorMessage;

@property (nonatomic, strong, readonly) NoCodesSandwich *noCodesSandwich;

@end

NS_ASSUME_NONNULL_END
