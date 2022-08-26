package com.qonversion.unitywrapper;

import android.app.Activity;
import android.content.Context;
import android.content.SharedPreferences;
import android.util.Log;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.node.ObjectNode;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.qonversion.android.sdk.QUserProperties;

import com.qonversion.android.sdk.QonversionEligibilityCallback;
import com.qonversion.android.sdk.QonversionErrorCode;
import com.qonversion.android.sdk.QonversionOfferingsCallback;
import com.qonversion.android.sdk.QonversionProductsCallback;
import com.qonversion.android.sdk.UpdatedPurchasesListener;
import com.qonversion.android.sdk.dto.eligibility.QEligibility;
import com.qonversion.android.sdk.dto.offerings.QOfferings;
import com.qonversion.android.sdk.dto.products.QProduct;
import com.unity3d.player.UnityPlayer;

import org.jetbrains.annotations.NotNull;

import java.util.HashMap;
import java.util.Map;
import java.util.List;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;

import com.qonversion.android.sdk.AttributionSource;
import com.qonversion.android.sdk.Qonversion;
import com.qonversion.android.sdk.QonversionError;
import com.qonversion.android.sdk.QonversionLaunchCallback;
import com.qonversion.android.sdk.QonversionPermissionsCallback;
import com.qonversion.android.sdk.dto.QLaunchResult;
import com.qonversion.android.sdk.dto.QPermission;
import com.qonversion.android.sdk.dto.QPermissionsCacheLifetime;

import android.preference.PreferenceManager;

public class QonversionWrapper {
    public static String TAG = "QonversionWrapper";
    public static String ON_UPDATED_PURCHASES_LISTENER = "OnReceiveUpdatedPurchases";

    private static UpdatedPurchasesListener updatedPurchasesListener = null;
    private static MessageSender messageSender;
    private static AutomationsDelegate automationsDelegate = null;

    public static synchronized void storeSdkInfo(String version, String versionKey, String source, String sourceKey) {
        Context context = UnityPlayer.currentActivity.getApplicationContext();

        SharedPreferences.Editor editor = PreferenceManager.getDefaultSharedPreferences(context).edit();
        editor.putString(versionKey, version);
        editor.putString(sourceKey, source);
        editor.apply();
    }

    public static synchronized void launch(String unityListener, String projectKey, boolean observerMode) {
        Log.d(TAG, "Qonversion Launch starting. Project key: " + projectKey);

        messageSender = new MessageSender(unityListener);

        Activity unityActivity = UnityPlayer.currentActivity;

        Qonversion.launch(unityActivity.getApplication(), projectKey, observerMode, new QonversionLaunchCallback() {
            @Override
            public void onSuccess(@NotNull QLaunchResult launchResult) {
                Log.d(TAG, "Qonversion initialized. UID: " + launchResult.getUid());
            }

            @Override
            public void onError(@NotNull QonversionError qonversionError) {
                Log.d(TAG, "Qonversion initializing error: " + qonversionError.getCode() + ", " + qonversionError.getDescription() + ", " + qonversionError.getAdditionalMessage());
            }
        });
    }

    public static synchronized void syncPurchases() {
        Qonversion.syncPurchases();
    }

    public static synchronized void setDebugMode() {
        Qonversion.setDebugMode();
    }

    public static synchronized void setProperty(String key, String value) {
        try {
            QUserProperties enumKey = QUserProperties.valueOf(key);
            Qonversion.setProperty(enumKey, value);
        } catch (IllegalArgumentException e) {
            Log.e(TAG, "Failed to map QUserProperties. " + e.getLocalizedMessage());
        }
    }

    public static synchronized void setUserProperty(String key, String value) {
        Qonversion.setUserProperty(key, value);
    }

    public static synchronized void attribution(String conversionData, String attributionSource) {
        try {
            AttributionSource source = AttributionSource.valueOf(attributionSource);

            ObjectMapper mapper = new ObjectMapper();

            TypeReference<HashMap<String, Object>> typeRef
                    = new TypeReference<HashMap<String, Object>>() {
            };
            Map<String, Object> conversionInfo = mapper.readValue(conversionData, typeRef);

            Qonversion.attribution(conversionInfo, source);
            Log.d(TAG, "Attribution sent");
        } catch (Exception e) {
            Log.e(TAG, "pushAttribution error: " + e.getLocalizedMessage());
        }
    }

    public static synchronized void identify(String value) {
        Qonversion.identify(value);
    }

    public static synchronized void logout() {
        Qonversion.logout();
    }

    public static synchronized void checkPermissions(String unityCallbackName) {
        Qonversion.checkPermissions(new QonversionPermissionsCallback() {
            @Override
            public void onSuccess(@NotNull Map<String, QPermission> permissions) {
                handlePermissionsResponse(permissions, unityCallbackName);
            }

            @Override
            public void onError(@NotNull QonversionError error) {
                handleErrorResponse(error, unityCallbackName);
            }
        });
    }

    public static synchronized void purchase(String productId, String unityCallbackName) {
        Qonversion.purchase(UnityPlayer.currentActivity, productId, new QonversionPermissionsCallback() {
            @Override
            public void onSuccess(@NotNull Map<String, QPermission> permissions) {
                handlePurchaseResponse(permissions, unityCallbackName);
            }

            @Override
            public void onError(@NotNull QonversionError error) {
                handlePurchaseErrorResponse(error, unityCallbackName);
            }
        });
    }

    public static synchronized void purchaseProduct(String productJson, String unityCallbackName) {
        try {
            QProduct product = Mapper.mapProductFromJson(productJson);
            if (product == null) {
                QonversionError error = new QonversionError(QonversionErrorCode.PurchaseInvalid, "Qonversion Product is null");
                handleErrorResponse(error, unityCallbackName);
                return;
            }
            Qonversion.purchase(UnityPlayer.currentActivity, product, new QonversionPermissionsCallback() {
                @Override
                public void onSuccess(@NotNull Map<String, QPermission> permissions) {
                    handlePurchaseResponse(permissions, unityCallbackName);
                }

                @Override
                public void onError(@NotNull QonversionError error) {
                    handlePurchaseErrorResponse(error, unityCallbackName);
                }
            });
        } catch (Exception e) {
            handleException(e);
        }
    }

    public static synchronized void updatePurchase(String productId, String oldProductId, int prorationMode, String unityCallbackName) {
        Qonversion.updatePurchase(UnityPlayer.currentActivity, productId, oldProductId, prorationMode, new QonversionPermissionsCallback() {
            @Override
            public void onSuccess(@NotNull Map<String, QPermission> permissions) {
                handlePurchaseResponse(permissions, unityCallbackName);
            }

            @Override
            public void onError(@NotNull QonversionError error) {
                handlePurchaseErrorResponse(error, unityCallbackName);
            }
        });
    }

    public static synchronized void updatePurchaseWithProduct(String newProductJson, String oldProductId, int prorationMode, String unityCallbackName) {
        try {
            QProduct product = Mapper.mapProductFromJson(newProductJson);
            if (product == null) {
                QonversionError error = new QonversionError(QonversionErrorCode.PurchaseInvalid, "Qonversion Product is null");
                handleErrorResponse(error, unityCallbackName);
                return;
            }
            Qonversion.updatePurchase(UnityPlayer.currentActivity, product, oldProductId, prorationMode, new QonversionPermissionsCallback() {
                @Override
                public void onSuccess(@NotNull Map<String, QPermission> permissions) {
                    handlePurchaseResponse(permissions, unityCallbackName);
                }

                @Override
                public void onError(@NotNull QonversionError error) {
                    handlePurchaseErrorResponse(error, unityCallbackName);
                }
            });
        } catch (Exception e) {
            handleException(e);
        }
    }

    public static synchronized void restore(String unityCallbackName) {
        Qonversion.restore(new QonversionPermissionsCallback() {
            @Override
            public void onSuccess(@NotNull Map<String, QPermission> permissions) {
                handlePermissionsResponse(permissions, unityCallbackName);
            }

            @Override
            public void onError(@NotNull QonversionError error) {
                handleErrorResponse(error, unityCallbackName);
            }
        });
    }

    public static synchronized void products(String unityCallbackName) {
        Qonversion.products(new QonversionProductsCallback() {
            @Override
            public void onSuccess(@NotNull Map<String, QProduct> products) {
                List<Map<String, Object>> mappedProducts = Mapper.mapProducts(products);
                sendMessageToUnity(mappedProducts, unityCallbackName);
            }

            @Override
            public void onError(@NotNull QonversionError error) {
                handleErrorResponse(error, unityCallbackName);
            }
        });
    }

    public static synchronized void offerings(String unityCallbackName) {
        Qonversion.offerings(new QonversionOfferingsCallback() {
            @Override
            public void onSuccess(@NotNull QOfferings qOfferings) {
                Map<String, Object> mappedOfferings = Mapper.mapOfferings(qOfferings);
                sendMessageToUnity(mappedOfferings, unityCallbackName);
            }

            @Override
            public void onError(@NotNull QonversionError error) {
                handleErrorResponse(error, unityCallbackName);
            }
        });
    }

    public static synchronized void checkTrialIntroEligibilityForProductIds(String productIds, String unityCallbackName) {
        try {
            ObjectMapper mapper = new ObjectMapper();
            TypeReference<List<String>> typeRef = new TypeReference<List<String>>() {};
            List<String> productIdsList = mapper.readValue(productIds, typeRef);

            Qonversion.checkTrialIntroEligibilityForProductIds(productIdsList, new QonversionEligibilityCallback() {
                @Override
                public void onSuccess(@NotNull Map<String, QEligibility> eligibilities) {
                    List<Map<String, Object>> mappedEligibilities = Mapper.mapEligibilities(eligibilities);
                    sendMessageToUnity(mappedEligibilities, unityCallbackName);
                }

                @Override
                public void onError(@NotNull QonversionError error) {
                    handleErrorResponse(error, unityCallbackName);
                }
            });
        } catch (JsonProcessingException e) {
            handleException(e);
        }
    }

    public static synchronized void addUpdatedPurchasesDelegate() {
        updatedPurchasesListener = permissions -> handlePermissionsResponse(permissions, ON_UPDATED_PURCHASES_LISTENER);
        Qonversion.setUpdatedPurchasesListener(updatedPurchasesListener);
    }

    public static synchronized void removeUpdatedPurchasesDelegate() {
        updatedPurchasesListener = null;
    }

    public static synchronized void setPermissionsCacheLifetime(String lifetimeKey) {
        try {
            QPermissionsCacheLifetime lifetime = QPermissionsCacheLifetime.valueOf(lifetimeKey);
            Qonversion.setPermissionsCacheLifetime(lifetime);
        } catch (IllegalArgumentException e) {
            Log.e(TAG, "Failed to map QPermissionsCacheLifetime. " + e.getLocalizedMessage());
        }
    }

    public static synchronized void setNotificationsToken(String token) {
        Qonversion.setNotificationsToken(token);
    }

    public static synchronized boolean handleNotification(String notification) {
        try {
            ObjectMapper mapper = new ObjectMapper();

            TypeReference<HashMap<String, String>> typeRef
                    = new TypeReference<HashMap<String, String>>() {
            };
            Map<String, String> notificationInfo = mapper.readValue(notification, typeRef);

            boolean result =  Qonversion.handleNotification(notificationInfo);

            return result;
        } catch (Exception e) {
            return false;
        }
    }

    public static synchronized void subscribeAutomationsDelegate() {
        automationsDelegate = new AutomationsDelegate(messageSender);
    }

    private static void handlePermissionsResponse(@NotNull Map<String, QPermission> permissions, @NotNull String methodName) {
        List<Map<String, Object>> mappedPermissions = Mapper.mapPermissions(permissions);
        sendMessageToUnity(mappedPermissions, methodName);
    }

    private static void handlePurchaseResponse(@NotNull Map<String, QPermission> permissions, @NotNull String methodName) {
        List<Map<String, Object>> mappedPermissions = Mapper.mapPermissions(permissions);
        Map<String, Object> result = new HashMap<>();
        result.put("permissions", mappedPermissions);
        sendMessageToUnity(result, methodName);
    }

    private static void handlePurchaseErrorResponse(@NotNull QonversionError error, @NotNull String methodName) {
        final ObjectMapper mapper = new ObjectMapper();
        final ObjectNode rootNode = createErrorNode(error);
        final boolean isCancelled = error.getCode() == QonversionErrorCode.CanceledPurchase;
        final JsonNode isCancelledNode = mapper.convertValue(isCancelled, JsonNode.class);
        rootNode.set("isCancelled", isCancelledNode);
        sendMessageToUnity(rootNode, methodName);
    }

    private static void handleErrorResponse(@NotNull QonversionError error, @NotNull String methodName) {
        final ObjectNode rootNode = createErrorNode(error);
        sendMessageToUnity(rootNode, methodName);
    }

    private static ObjectNode createErrorNode(@NotNull QonversionError error) {
        ObjectMapper mapper = new ObjectMapper();
        ObjectNode node = mapper.createObjectNode();
        String message = String.format("%s. %s", error.getDescription(), error.getAdditionalMessage());

        ObjectNode errorNode = mapper.createObjectNode();
        errorNode.put("message", message);
        errorNode.put("code", error.getCode().name());

        node.set("error", errorNode);
        return node;
    }

    private static void sendMessageToUnity(@NotNull Object objectToConvert, @NotNull String methodName) {
        try {
            messageSender.sendMessageToUnity(objectToConvert, methodName);
        } catch (JsonProcessingException e) {
            handleException(e);
        }
    }

    private static void handleException(Exception e) {
        Log.e(TAG, "An error occurred while serializing data: " + e.getLocalizedMessage());
    }
}