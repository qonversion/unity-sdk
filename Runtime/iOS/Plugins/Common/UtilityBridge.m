#import "UtilityBridge.h"

@implementation UtilityBridge

+ (const char*)convertNSStringToCString:(NSString*) nsString {
    if (nsString == NULL)
        return NULL;
    
    const char* nsStringUtf8 = [nsString UTF8String];
    char* cString = (char*)malloc(strlen(nsStringUtf8) + 1);
    strcpy(cString, nsStringUtf8);
    
    return cString;
}

+ (NSString*)сonvertCStringToNSString:(const char *)string {
    if (string == NULL) {
        return nil;
    }
    
    return [NSString stringWithUTF8String:string];
}

+ (NSData *)convertHexToData:(NSString *)hex {
    NSString *token = [hex stringByReplacingOccurrencesOfString:@" " withString:@""];
    NSMutableData *data = [[NSMutableData alloc] init];
    unsigned char whole_byte;
    char byte_chars[3] = {'\0','\0','\0'};
    int i;
    for (i=0; i < [token length] / 2; i++) {
        byte_chars[0] = [token characterAtIndex:i * 2];
        byte_chars[1] = [token characterAtIndex:i * 2 + 1];
        whole_byte = strtol(byte_chars, NULL, 16);
        [data appendBytes:&whole_byte length:1];
    }
    
    return [data copy];
}

+ (NSDictionary*)dictionaryFromJsonString:(NSString*) jsonString {
    if (!jsonString) {
        return @{};
    }
    
    NSData *jsonData = [jsonString dataUsingEncoding:NSUTF8StringEncoding];
    NSDictionary *dictionary = [NSJSONSerialization JSONObjectWithData:jsonData options:kNilOptions error:nil];
    
    return dictionary;
}

+ (NSNumber *)convertProperty:(NSString *)propertyStr {
    NSDictionary *propertiesDict = @{
        @"Email": @(QNPropertyEmail),
        @"Name": @(QNPropertyName),
        @"AppsFlyerUserId": @(QNPropertyAppsFlyerUserID),
        @"AdjustAdId": @(QNPropertyAdjustUserID),
        @"KochavaDeviceId": @(QNPropertyKochavaDeviceID),
        @"CustomUserId": @(QNPropertyUserID),
    };
    
    NSNumber *propertyIndex = propertiesDict[propertyStr];
    return propertyIndex;
}

+ (NSDictionary *)convertError:(NSError *)error{
    NSString *errorMessage = [NSString stringWithFormat:@"%@. Domain: %@", error.localizedDescription, error.domain];
    NSMutableDictionary *errorDict = [NSMutableDictionary new];
    errorDict[@"code"] = @(error.code).stringValue;
    
    NSString *debugMessage = error.userInfo[NSDebugDescriptionErrorKey];
    if (debugMessage.length > 0) {
        errorMessage = [NSString stringWithFormat:@"%@\nDebugInfo:%@", errorMessage, debugMessage];
    }
    
    errorDict[@"message"] = errorMessage;
    
    NSMutableDictionary *result = [NSMutableDictionary new];
    result[@"error"] = errorDict;
    
    return result;
}

+ (NSArray *)convertPermissions:(NSArray<QNPermission *> *)permissions {
    NSMutableArray *result = [NSMutableArray new];
    
    for (QNPermission *permission in permissions) {
        NSMutableDictionary *permissionDict = [@{
            @"id": permission.permissionID,
            @"associated_product": permission.productID,
            @"renew_state": @(permission.renewState),
            @"started_timestamp": @(permission.startedDate.timeIntervalSince1970 * 1000),
            @"active": @(permission.isActive)
        } mutableCopy];
        
        if (permission.expirationDate) {
            permissionDict[@"expiration_timestamp"] = @(permission.expirationDate.timeIntervalSince1970 * 1000);
        }
        
        [result addObject:permissionDict];
    }
    
    return result;
}

+ (NSArray *)convertProducts:(NSArray<QNProduct *> *)products {
    NSMutableArray *result = [NSMutableArray new];
    
    for (QNProduct *product in products) {
        NSNumber *trialDuration = product.trialDuration ? @(product.trialDuration) : @(QNTrialDurationNotAvailable);
        NSMutableDictionary *productsDict = [@{
            qonversionIdKey: product.qonversionID,
            storeIdKey: product.storeID,
            offeringIdKey: product.offeringID,
            typeKey: @(product.type),
            durationKey: @(product.duration),
            prettyPriceKey: product.prettyPrice,
            trialDurationKey: trialDuration
        } mutableCopy];
        
        if (product.skProduct) {
            NSDictionary *skProductInfo = [UtilityBridge convertSKProduct:product.skProduct];
            productsDict[storeProductKey] = skProductInfo;
        }
        [result addObject:productsDict];
    }
    
    return result;
}

+ (QNProduct *)convertProductFromJson:(NSString *)productJson {
    NSDictionary *productDict = [UtilityBridge dictionaryFromJsonString: productJson];

    NSNumber *type = productDict[typeKey] != nil ? productDict[typeKey] : @(QNProductTypeUnknown);
    NSNumber *duration = productDict[durationKey] != nil ? productDict[durationKey] : @(QNProductDurationUnknown);
    NSNumber *trialDuration = productDict[trialDurationKey] != nil ? productDict[trialDurationKey] : @(QNTrialDurationNotAvailable);

    QNProductType productType = type.integerValue;
    QNProductDuration productDuration = duration.integerValue;
    QNTrialDuration productTrialDuration = trialDuration.integerValue;

    QNProduct *product = [QNProduct new];
    product.qonversionID = productDict[qonversionIdKey];
    product.storeID = productDict[storeIdKey];
    product.offeringID = productDict[offeringIdKey];
    product.prettyPrice = productDict[prettyPriceKey];
    product.type = productType;
    product.duration = productDuration;
    product.trialDuration = productTrialDuration;

    return product;
}

+ (NSDictionary *)convertSKProduct:(SKProduct *)product {
    NSMutableDictionary *result = [NSMutableDictionary new];
    result[@"localizedDescription"] = product.localizedDescription;
    result[@"localizedTitle"] = product.localizedTitle;
    result[@"price"] = [product.price stringValue];
    result[@"localeIdentifier"] = product.priceLocale.localeIdentifier;
    result[@"productIdentifier"] = product.productIdentifier;
    result[@"isDownloadable"] = @(product.isDownloadable);
    result[@"downloadContentVersion"] = product.downloadContentVersion;
    result[@"downloadContentLengths"] = product.downloadContentLengths;
    
    if (@available(iOS 10.0, *)) {
        result[@"currencyCode"] = product.priceLocale.currencyCode;
    }
    
    if (@available(iOS 11.2, *)) {
        NSDictionary *subscriptionPeriod = [UtilityBridge convertSubscriptionPeriod:product.subscriptionPeriod];
        result[@"subscriptionPeriod"] = [subscriptionPeriod copy];
        
        NSDictionary *introductoryPrice = [UtilityBridge convertDiscount:product.introductoryPrice];
        result[@"introductoryPrice"] = [introductoryPrice copy];
        
        if (@available(iOS 12.2, *)) {
            NSArray *discounts = [UtilityBridge convertDiscounts:product.discounts];
            result[@"discounts"] = discounts;
        }
    }
    
    if (@available(iOS 12.0, *)) {
        result[@"subscriptionGroupIdentifier"] = product.subscriptionGroupIdentifier;
    }
    
    if (@available(iOS 14.0, *)) {
        result[@"isFamilyShareable"] = @(product.isFamilyShareable);
    }
    
    return [result copy];
}


+ (NSArray<NSDictionary *> *)convertDiscounts:(NSArray<SKProductDiscount *> *)discounts API_AVAILABLE(ios(11.2)) {
    NSMutableArray *result = [NSMutableArray new];
    for (SKProductDiscount *discount in discounts) {
        NSDictionary *introductoryPriceInfo = [UtilityBridge convertDiscount:discount];
        [result addObject:introductoryPriceInfo];
    }
    
    return [result copy];
}

+ (NSDictionary *)convertDiscount:(SKProductDiscount *)discount API_AVAILABLE(ios(11.2)) {
    NSMutableDictionary *introductoryPrice = [NSMutableDictionary new];
    introductoryPrice[@"price"] = [discount.price stringValue];
    introductoryPrice[@"localeIdentifier"] = discount.priceLocale.localeIdentifier;
    introductoryPrice[@"numberOfPeriods"] = @(discount.numberOfPeriods);
    
    NSDictionary *subscriptionPeriod = [UtilityBridge convertSubscriptionPeriod:discount.subscriptionPeriod];
    introductoryPrice[@"subscriptionPeriod"] = [subscriptionPeriod copy];
    
    introductoryPrice[@"paymentMode"] = @(discount.paymentMode);
    introductoryPrice[@"currencySymbol"] = discount.priceLocale.currencySymbol;
    
    if (@available(iOS 12.2, *)) {
        introductoryPrice[@"identifier"] = discount.identifier;
        introductoryPrice[@"type"] = @(discount.type);
    }
    
    return [introductoryPrice copy];
}

+ (NSDictionary *)convertSubscriptionPeriod:(SKProductSubscriptionPeriod *)subscriptionPeriod API_AVAILABLE(ios(11.2)) {
    NSMutableDictionary *introductorySubscriptionPeriod = [NSMutableDictionary new];
    introductorySubscriptionPeriod[@"numberOfUnits"] = @(subscriptionPeriod.numberOfUnits);
    introductorySubscriptionPeriod[@"unit"] = @(subscriptionPeriod.unit);
    
    return [introductorySubscriptionPeriod copy];
}

+ (NSDictionary *)convertOfferings:(QNOfferings *)offerings {
    NSMutableDictionary *result = [NSMutableDictionary new];
    
    if (offerings.main) {
        result[@"main"] = [UtilityBridge convertOffering:offerings.main];
    }
    
    NSMutableArray *availableOfferings = [NSMutableArray new];
    
    for (QNOffering *offering in offerings.availableOfferings) {
        NSDictionary *convertedOffering = [UtilityBridge convertOffering:offering];
        
        [availableOfferings addObject:convertedOffering];
    }
    
    result[@"availableOfferings"] = [availableOfferings copy];
    
    return [result copy];
}

+ (NSDictionary *)convertOffering:(QNOffering *)offering {
    NSMutableDictionary *result = [NSMutableDictionary new];
    
    result[@"id"] = offering.identifier;
    result[@"tag"] = @(offering.tag);
    
    NSArray *convertedProducts = [UtilityBridge convertProducts:offering.products];
    
    result[@"products"] = [convertedProducts copy];
    
    return [result copy];
}

+ (NSDictionary *)convertIntroEligibility:(NSDictionary<NSString *, QNIntroEligibility *> *)introEligibilityInfo {
    NSDictionary *statuses = @{
        @(QNIntroEligibilityStatusNonIntroProduct): @"non_intro_or_trial_product",
        @(QNIntroEligibilityStatusEligible): @"intro_or_trial_eligible",
        @(QNIntroEligibilityStatusIneligible): @"intro_or_trial_ineligible"
    };

    NSMutableArray *convertedData = [NSMutableArray new];

    for (NSString *key in introEligibilityInfo.allKeys) {
        QNIntroEligibility *eligibility = introEligibilityInfo[key];
        NSString *statusValue = statuses[@(eligibility.status)] ? : @"unknonw";

        NSDictionary *eligibilityInfo = @{@"productId": key, @"status": statusValue};

        [convertedData addObject:eligibilityInfo];
    }

    return [convertedData copy];
}

+ (void)handlePermissionsResponse:(NSDictionary<NSString *,QNPermission *> *) result withError:( NSError *)error
                         toMethod:(NSString *) methodName
                    unityListener:(const char *)unityListenerName{
    if (error) {
        [UtilityBridge handleErrorResponse:error toMethod:methodName unityListener:unityListenerName];
        return;
    }
    
    NSArray *permissions = [UtilityBridge convertPermissions:result.allValues];
    [UtilityBridge sendUnityMessage:permissions toMethod:methodName unityListener: unityListenerName];
}

+ (void)handleErrorResponse:(NSError *)error toMethod:(NSString *) methodName
              unityListener:(const char *)unityListenerName{
    NSDictionary *errorDict = [UtilityBridge convertError:error];
    [UtilityBridge sendUnityMessage:errorDict toMethod:methodName unityListener: unityListenerName];
}

+ (void)sendUnityMessage:(NSObject *)objectToConvert toMethod:(NSString *)methodName
           unityListener:(const char *)unityListenerName{
    NSError *error = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:objectToConvert options:0 error:&error];
    
    if (error) {
        NSLog(@"An error occurred while serializing data: %@", error.localizedDescription);
        return;
    }
    if (jsonData) {
        NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        UnitySendMessage(unityListenerName, methodName.UTF8String, json.UTF8String);
    }
}

@end
