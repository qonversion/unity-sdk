package com.qonversion.unitywrapper;

import com.android.billingclient.api.SkuDetails;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.qonversion.android.sdk.dto.QPermission;
import com.qonversion.android.sdk.dto.eligibility.QEligibility;
import com.qonversion.android.sdk.dto.offerings.QOffering;
import com.qonversion.android.sdk.dto.offerings.QOfferings;
import com.qonversion.android.sdk.dto.products.QProduct;
import com.qonversion.android.sdk.dto.products.QProductDuration;
import com.qonversion.android.sdk.dto.products.QProductType;
import com.qonversion.android.sdk.dto.products.QTrialDuration;

import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public final class Mapper {
    private static final String ID = "id";
    private static final String STORE_ID = "store_id";
    private static final String TYPE = "type";
    private static final String OFFERING_ID = "offeringId";
    private static final String DURATION = "duration";
    private static final String TRIAL_DURATION = "trialDuration";
    private static final String STORE_PRODUCT = "storeProduct";
    private static final String PRETTY_PRICE = "prettyPrice";
    private static final String SKU_DETAILS_JSON = "originalJson";

    static List<Map<String, Object>> mapPermissions(Map<String, QPermission> permissions) {
        List<Map<String, Object>> result = new ArrayList<>();

        for (Map.Entry<String, QPermission> entry : permissions.entrySet()) {
            Map<String, Object> permissionMap = new HashMap<>();
            QPermission permission = entry.getValue();

            permissionMap.put("id", permission.getPermissionID());
            permissionMap.put("associated_product", permission.getProductID());
            permissionMap.put("renew_state", permission.getRenewState().getType());
            permissionMap.put("active", permission.isActive());
            permissionMap.put("started_timestamp", permission.getStartedDate().getTime());

            Date expirationDate = permission.getExpirationDate();
            if (expirationDate != null) {
                permissionMap.put("expiration_timestamp", expirationDate.getTime());
            }

            result.add(permissionMap);
        }

        return result;
    }

    static List<Map<String, Object>> mapProducts(Map<String, QProduct> products) {
        List<Map<String, Object>> result = new ArrayList<>();

        for (Map.Entry<String, QProduct> entry : products.entrySet()) {
            Map<String, Object> mappedProducts = Mapper.mapProduct(entry.getValue());
            result.add(mappedProducts);
        }

        return result;
    }

    static Map<String, Object> mapProduct(QProduct product) {
        Map<String, Object> result = new HashMap<>();

        result.put(ID, product.getQonversionID());
        result.put(STORE_ID, product.getStoreID());
        result.put(TYPE, product.getType().getType());

        String offeringId = product.getOfferingID();
        result.put(OFFERING_ID, offeringId);

        QProductDuration duration = product.getDuration();
        if (duration != null) {
            result.put(DURATION, duration.getType());
        }

        QTrialDuration trialDuration = product.getTrialDuration();
        if (trialDuration != null) {
            result.put(TRIAL_DURATION, trialDuration.getType());
        }

        SkuDetails skuDetails = product.getSkuDetail();
        if (skuDetails != null) {
            Map<String, Object> mappedSkuDetails = mapSkuDetails(skuDetails);
            result.put(STORE_PRODUCT, mappedSkuDetails);
            result.put(PRETTY_PRICE, product.getPrettyPrice());
        }

        return result;
    }

    static QProduct mapProductFromJson(String productJson) throws Exception {
        ObjectMapper mapper = new ObjectMapper();

        TypeReference<Map<String, Object>> typeRef = new TypeReference<Map<String, Object>>() {};
        Map<String, Object> map = mapper.readValue(productJson, typeRef);

        String id = String.valueOf(map.get(ID));
        String storeId = String.valueOf(map.get(STORE_ID));
        String offeringId = String.valueOf(map.get(OFFERING_ID));
        String prettyPrice = String.valueOf(map.get(PRETTY_PRICE));

        Object skuDetailsObj = map.get(STORE_PRODUCT);
        SkuDetails skuDetails = mapSkuDetailsFromObject(skuDetailsObj);

        Integer type = (Integer) map.get(TYPE);
        if (type == null) return null;
        QProductType qProductType = QProductType.Companion.fromType(type);

        QProductDuration qProductDuration = null;
        Integer duration = (Integer) map.get(DURATION);
        if (duration != null) {
            qProductDuration = QProductDuration.Companion.fromType(duration);
        }

        QTrialDuration qTrialDuration = null;
        Integer trialDuration = (Integer) map.get(TRIAL_DURATION);
        if (trialDuration != null) {
            qTrialDuration = QTrialDuration.Companion.fromType(trialDuration);
        }

        QProduct product = new QProduct(id, storeId, qProductType, qProductDuration);
        product.setOfferingID(offeringId);
        product.setSkuDetail(skuDetails);
        product.setPrettyPrice(prettyPrice);
        product.setTrialDuration(qTrialDuration);

        return product;
    }

    static private SkuDetails mapSkuDetailsFromObject(Object map) throws Exception {
        ObjectMapper mapper = new ObjectMapper();
        TypeReference<Map<String, Object>> typeRef = new TypeReference<Map<String, Object>>() {
        };

        Map<String, Object> skuDetailsMap = mapper.convertValue(map, typeRef);
        String originalJson = String.valueOf(skuDetailsMap.get(SKU_DETAILS_JSON));

        return new SkuDetails(originalJson);
    }

    static private Map<String, Object> mapSkuDetails(SkuDetails skuDetails) {
        Map<String, Object> result = new HashMap<>();

        result.put("description", skuDetails.getDescription());
        result.put("freeTrialPeriod", skuDetails.getFreeTrialPeriod());
        result.put("iconUrl", skuDetails.getIconUrl());
        result.put("introductoryPrice", skuDetails.getIntroductoryPrice());
        result.put("introductoryPriceAmountMicros", skuDetails.getIntroductoryPriceAmountMicros());
        result.put("introductoryPriceCycles", skuDetails.getIntroductoryPriceCycles());
        result.put("introductoryPricePeriod", skuDetails.getIntroductoryPricePeriod());
        result.put("originalJson", skuDetails.getOriginalJson());
        result.put("originalPrice", skuDetails.getOriginalPrice());
        result.put("originalPriceAmountMicros", skuDetails.getOriginalPriceAmountMicros());
        result.put("price", skuDetails.getPrice());
        result.put("priceAmountMicros", skuDetails.getPriceAmountMicros());
        result.put("priceCurrencyCode", skuDetails.getPriceCurrencyCode());
        result.put("sku", skuDetails.getSku());
        result.put("subscriptionPeriod", skuDetails.getSubscriptionPeriod());
        result.put("title", skuDetails.getTitle());
        result.put("type", skuDetails.getType());
        result.put("hashCode", skuDetails.hashCode());

        return result;
    }

    static Map<String, Object> mapOfferings(QOfferings offerings) {
        Map<String, Object> result = new HashMap<>();

        if (offerings.getMain() != null) {
            Map<String, Object> mainOffering = Mapper.mapOffering(offerings.getMain());
            result.put("main", mainOffering);
        }

        List<Map<String, Object>> availableOfferings = new ArrayList<>();

        for (QOffering offering : offerings.getAvailableOfferings()) {
            Map<String, Object> mappedOffering = Mapper.mapOffering(offering);
            availableOfferings.add(mappedOffering);
        }

        result.put("availableOfferings", availableOfferings);

        return result;
    }

    private static Map<String, Object> mapOffering(QOffering offering) {
        Map<String, Object> result = new HashMap<>();
        result.put("id", offering.getOfferingID());

        Integer tagValue = offering.getTag().getTag();
        if (tagValue != null) {
            result.put("tag", tagValue);
        }

        List<Map<String, Object>> mappedProducts = new ArrayList<>();

        for (QProduct product : offering.getProducts()) {
            Map<String, Object> mappedProduct = Mapper.mapProduct(product);
            mappedProducts.add(mappedProduct);
        }

        result.put("products", mappedProducts);

        return result;
    }

    static List<Map<String, Object>> mapEligibilities(Map<String, QEligibility> map) {
        List<Map<String, Object>> result = new ArrayList<>();

        for (Map.Entry<String, QEligibility> entry : map.entrySet()) {
            Map<String, Object> mappedEligibility = new HashMap<>();
            QEligibility eligibility = entry.getValue();
            mappedEligibility.put("productId", entry.getKey());
            mappedEligibility.put("status", eligibility.getStatus().getType());

            result.add(mappedEligibility);
        }

        return result;
    }
}
