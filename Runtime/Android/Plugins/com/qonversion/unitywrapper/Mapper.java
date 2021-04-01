package com.qonversion.unitywrapper;

import com.android.billingclient.api.SkuDetails;

import com.qonversion.android.sdk.dto.QPermission;
import com.qonversion.android.sdk.dto.offerings.QOffering;
import com.qonversion.android.sdk.dto.offerings.QOfferings;
import com.qonversion.android.sdk.dto.products.QProduct;
import com.qonversion.android.sdk.dto.products.QProductDuration;
import com.qonversion.android.sdk.dto.products.QTrialDuration;

import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public final class Mapper {

    public static List<Map<String, Object>> mapPermissions(Map<String, QPermission> permissions) {
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

        result.put("id", product.getQonversionID());
        result.put("store_id", product.getStoreID());
        result.put("type", product.getType().getType());

        QProductDuration duration = product.getDuration();
        if (duration != null) {
            result.put("duration", duration.getType());
        }

        QTrialDuration trialDuration = product.getTrialDuration();
        if (trialDuration != null) {
            result.put("trialDuration", trialDuration.getType());
        }

        SkuDetails skuDetails = product.getSkuDetail();
        if (skuDetails != null) {
            Map<String, Object> mappedSkuDetails = mapSkuDetails(product.getSkuDetail());
            result.put("storeProduct", mappedSkuDetails);
            result.put("prettyPrice", product.getPrettyPrice());
        }

        return result;
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

    static Map<String, Object> mapOffering(QOffering offering) {
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
}