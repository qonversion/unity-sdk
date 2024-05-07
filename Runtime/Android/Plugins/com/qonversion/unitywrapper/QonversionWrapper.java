package com.qonversion.unitywrapper;

import android.app.Application;
import android.util.Log;

import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.node.ObjectNode;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.unity3d.player.UnityPlayer;

import org.jetbrains.annotations.NotNull;

import java.util.HashMap;
import java.util.Map;
import java.util.List;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;

import io.qonversion.sandwich.PurchaseResultListener;
import io.qonversion.sandwich.QonversionSandwich;
import io.qonversion.sandwich.ResultListener;
import io.qonversion.sandwich.SandwichError;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

public class QonversionWrapper {
    public static String TAG = "QonversionWrapper";
    public static String ENTITLEMENTS_UPDATE_LISTENER = "OnReceivedUpdatedEntitlements";

    private static MessageSender messageSender;
    private static QonversionSandwich qonversionSandwich;

    public static synchronized void initialize(String unityListener) {
        messageSender = new MessageSender(unityListener);

        final Application application = UnityPlayer.currentActivity.getApplication();
        qonversionSandwich = new QonversionSandwich(
                application,
                () -> UnityPlayer.currentActivity,
                entitlements -> sendMessageToUnity(entitlements, ENTITLEMENTS_UPDATE_LISTENER)
        );
    }

    public static synchronized void storeSdkInfo(String version, String source) {
        qonversionSandwich.storeSdkInfo(source, version);
    }

    public static synchronized void initializeSdk(
            String projectKey,
            String launchModeKey,
            @Nullable String environmentKey,
            @Nullable String entitlementsCacheLifetimeKey,
            @Nullable String proxyUrl,
            boolean kidsMode
    ) {
        qonversionSandwich.initialize(
                UnityPlayer.currentActivity,
                projectKey,
                launchModeKey,
                environmentKey,
                entitlementsCacheLifetimeKey,
                proxyUrl,
                kidsMode
        );
    }

    public static synchronized void syncHistoricalData() {
        qonversionSandwich.syncHistoricalData();
    }

    public static synchronized void syncPurchases() {
        qonversionSandwich.syncPurchases();
    }

    public static synchronized void setUserProperty(String key, String value) {
        qonversionSandwich.setDefinedProperty(key, value);
    }

    public static synchronized void setCustomUserProperty(String key, String value) {
        qonversionSandwich.setCustomProperty(key, value);
    }

    public static synchronized void userProperties(String unityCallbackName) {
        qonversionSandwich.userProperties(getResultListener(unityCallbackName));
    }

    public static synchronized void attribution(String conversionData, String attributionProvider) {
        try {
            ObjectMapper mapper = new ObjectMapper();

            TypeReference<HashMap<String, Object>> typeRef
                    = new TypeReference<HashMap<String, Object>>() {};
            Map<String, Object> conversionInfo = mapper.readValue(conversionData, typeRef);

            qonversionSandwich.addAttributionData(attributionProvider, conversionInfo);
        } catch (JsonProcessingException e) {
            handleSerializationException(e);
        }
    }

    public static synchronized void identify(String value, String unityCallbackName) {
        qonversionSandwich.identify(value, getResultListener(unityCallbackName));
    }

    public static synchronized void logout() {
        qonversionSandwich.logout();
    }

    public static synchronized void userInfo(String unityCallbackName) {
        qonversionSandwich.userInfo(getResultListener(unityCallbackName));
    }

    public static synchronized void checkEntitlements(String unityCallbackName) {
        qonversionSandwich.checkEntitlements(getResultListener(unityCallbackName));
    }

    public static synchronized void purchase(String productId, @Nullable String offerId, boolean applyOffer, String unityCallbackName) {
        qonversionSandwich.purchase(productId, offerId, applyOffer, getPurchaseResultListener(unityCallbackName));
    }

    public static synchronized void updatePurchase(
            String productId,
            @Nullable String offerId,
            boolean applyOffer,
            String oldProductId,
            @Nullable String updatePolicyKey,
            String unityCallbackName
    ) {
        qonversionSandwich.updatePurchase(productId, offerId, applyOffer, oldProductId, updatePolicyKey, getPurchaseResultListener(unityCallbackName));
    }

    public static synchronized void restore(String unityCallbackName) {
        qonversionSandwich.restore(getResultListener(unityCallbackName));
    }

    public static synchronized void products(String unityCallbackName) {
        qonversionSandwich.products(getResultListener(unityCallbackName));
    }

    public static synchronized void offerings(String unityCallbackName) {
        qonversionSandwich.offerings(getResultListener(unityCallbackName));
    }

    public static synchronized void remoteConfig(String contextKey, String unityCallbackName) {
        qonversionSandwich.remoteConfig(contextKey, getRemoteConfigResultListener(contextKey, unityCallbackName));
    }

    public static synchronized void remoteConfigList(String unityCallbackName) {
        qonversionSandwich.remoteConfigList(getResultListener(unityCallbackName));
    }

    public static synchronized void remoteConfigList(String contextKeysJson, boolean includeEmptyContextKey, String unityCallbackName) {
        try {
            ObjectMapper mapper = new ObjectMapper();

            TypeReference<List<String>> typeRef = new TypeReference<List<String>>() {};
            List<String> contextKeys = mapper.readValue(contextKeysJson, typeRef);

            qonversionSandwich.remoteConfigList(contextKeys, includeEmptyContextKey, getResultListener(unityCallbackName));
        } catch (JsonProcessingException e) {
            handleSerializationException(e);
        }
    }

    public static synchronized void attachUserToExperiment(String experimentId, String groupId, String unityCallbackName) {
        qonversionSandwich.attachUserToExperiment(experimentId, groupId, getResultListener(unityCallbackName));
    }

    public static synchronized void detachUserFromExperiment(String experimentId, String unityCallbackName) {
        qonversionSandwich.detachUserFromExperiment(experimentId, getResultListener(unityCallbackName));
    }

    public static synchronized void attachUserToRemoteConfiguration(String remoteConfigurationId, String unityCallbackName) {
        qonversionSandwich.attachUserToRemoteConfiguration(remoteConfigurationId, getResultListener(unityCallbackName));
    }

    public static synchronized void detachUserFromRemoteConfiguration(String remoteConfigurationId, String unityCallbackName) {
        qonversionSandwich.detachUserFromRemoteConfiguration(remoteConfigurationId, getResultListener(unityCallbackName));
    }

    public static synchronized void checkTrialIntroEligibility(String productIds, String unityCallbackName) {
        try {
            ObjectMapper mapper = new ObjectMapper();
            TypeReference<List<String>> typeRef = new TypeReference<List<String>>() {};
            List<String> productIdList = mapper.readValue(productIds, typeRef);

            qonversionSandwich.checkTrialIntroEligibility(productIdList, getResultListener(unityCallbackName));
        } catch (JsonProcessingException e) {
            handleSerializationException(e);
        }
    }

    private static ResultListener getResultListener(@NotNull String methodName) {
        return new ResultListener() {
            @Override
            public void onSuccess(@NonNull Map<String, ?> data) {
                sendMessageToUnity(data, methodName);
            }

            @Override
            public void onError(@NonNull SandwichError error) {
                handleErrorResponse(error, methodName);
            }
        };
    }

    private static PurchaseResultListener getPurchaseResultListener(@NotNull String methodName) {
        return new PurchaseResultListener() {
            @Override
            public void onSuccess(@NonNull Map<String, ?> data) {
                sendMessageToUnity(data, methodName);
            }

            @Override
            public void onError(@NonNull SandwichError error, boolean isCancelled) {
                final ObjectMapper mapper = new ObjectMapper();
                final ObjectNode rootNode = Utils.createErrorNode(error);
                final JsonNode isCancelledNode = mapper.convertValue(isCancelled, JsonNode.class);
                rootNode.set("isCancelled", isCancelledNode);
                sendMessageToUnity(rootNode, methodName);
            }
        };
    }

    private static ResultListener getRemoteConfigResultListener(@Nullable String contextKey, @NotNull String methodName) {
        return new ResultListener() {
            @Override
            public void onSuccess(@NonNull Map<String, ?> data) {
                sendMessageToUnity(data, methodName);
            }

            @Override
            public void onError(@NonNull SandwichError error) {
                final ObjectNode rootNode = Utils.createErrorNode(error);
                rootNode.put("contextKey", contextKey);

                sendMessageToUnity(rootNode, methodName);
            }
        };
    }

    private static void handleErrorResponse(@NotNull SandwichError error, @NotNull String methodName) {
        final ObjectNode rootNode = Utils.createErrorNode(error);

        sendMessageToUnity(rootNode, methodName);
    }

    private static void sendMessageToUnity(@NotNull Object objectToConvert, @NotNull String methodName) {
        try {
            messageSender.sendMessageToUnity(objectToConvert, methodName);
        } catch (JsonProcessingException e) {
            handleSerializationException(e);
        }
    }

    private static void handleSerializationException(JsonProcessingException e) {
        Log.e(TAG, "An error occurred while serializing data: " + e.getLocalizedMessage());
    }
}