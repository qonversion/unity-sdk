package com.qonversion.unitywrapper;

import android.app.Application;
import android.util.Log;

import com.fasterxml.jackson.databind.node.ObjectNode;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.unity3d.player.UnityPlayer;

import org.jetbrains.annotations.NotNull;

import java.util.HashMap;
import java.util.Map;
import java.util.List;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;

import io.qonversion.sandwich.QonversionSandwich;
import io.qonversion.sandwich.ResultListener;
import io.qonversion.sandwich.SandwichError;

import androidx.annotation.NonNull;

public class QonversionWrapper {
    public static String TAG = "QonversionWrapper";
    public static String ON_UPDATED_PURCHASES_LISTENER = "OnReceiveUpdatedPurchases";

    private static MessageSender messageSender;
    private static AutomationsWrapper automationsWrapper;
    private static QonversionSandwich qonversionSandwich;

    public static synchronized void initialize(String unityListener) {
        messageSender = new MessageSender(unityListener);

        final Application application = UnityPlayer.currentActivity.getApplication();
        qonversionSandwich = new QonversionSandwich(
                application,
                () -> UnityPlayer.currentActivity,
                permissions -> sendMessageToUnity(permissions, ON_UPDATED_PURCHASES_LISTENER)
        );
        automationsWrapper = new AutomationsWrapper(messageSender);
    }

    public static synchronized void storeSdkInfo(String version, String source) {
        qonversionSandwich.storeSdkInfo(source, version);
    }

    public static synchronized void launch(String projectKey, boolean observerMode, String unityCallbackName) {
        qonversionSandwich.launch(projectKey, observerMode, getResultListener(unityCallbackName));
    }

    public static synchronized void syncPurchases() {
        qonversionSandwich.syncPurchases();
    }

    public static synchronized void setDebugMode() {
        qonversionSandwich.setDebugMode();
    }

    public static synchronized void setProperty(String key, String value) {
        qonversionSandwich.setDefinedProperty(key, value);
    }

    public static synchronized void setUserProperty(String key, String value) {
        qonversionSandwich.setCustomProperty(key, value);
    }

    public static synchronized void attribution(String conversionData, String attributionSource) {
        try {
            ObjectMapper mapper = new ObjectMapper();

            TypeReference<HashMap<String, Object>> typeRef
                    = new TypeReference<HashMap<String, Object>>() {};
            Map<String, Object> conversionInfo = mapper.readValue(conversionData, typeRef);

            qonversionSandwich.addAttributionData(attributionSource, conversionInfo);
        } catch (JsonProcessingException e) {
            handleSerializationException(e);
        }
    }

    public static synchronized void identify(String value) {
        qonversionSandwich.identify(value);
    }

    public static synchronized void logout() {
        qonversionSandwich.logout();
    }

    public static synchronized void checkPermissions(String unityCallbackName) {
        qonversionSandwich.checkPermissions(getResultListener(unityCallbackName));
    }

    public static synchronized void purchase(String productId, String unityCallbackName) {
        qonversionSandwich.purchase(productId, getResultListener(unityCallbackName));
    }

    public static synchronized void purchaseProduct(String productId, String offeringId, String unityCallbackName) {
        qonversionSandwich.purchaseProduct(productId, offeringId, getResultListener(unityCallbackName));
    }

    public static synchronized void updatePurchase(String productId, String oldProductId, int prorationMode, String unityCallbackName) {
        qonversionSandwich.updatePurchase(productId, oldProductId, prorationMode, getResultListener(unityCallbackName));
    }

    public static synchronized void updatePurchaseWithProduct(String productId, String offeringId, String oldProductId, int prorationMode, String unityCallbackName) {
        qonversionSandwich.updatePurchaseWithProduct(productId, offeringId, oldProductId, prorationMode, getResultListener(unityCallbackName));
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

    public static synchronized void checkTrialIntroEligibilityForProductIds(String productIds, String unityCallbackName) {
        try {
            ObjectMapper mapper = new ObjectMapper();
            TypeReference<List<String>> typeRef = new TypeReference<List<String>>() {};
            List<String> productIdList = mapper.readValue(productIds, typeRef);

            qonversionSandwich.checkTrialIntroEligibility(productIdList, getResultListener(unityCallbackName));
        } catch (JsonProcessingException e) {
            handleSerializationException(e);
        }
    }

    public static synchronized void setNotificationsToken(String token) {
        qonversionSandwich.setNotificationToken(token);
    }

    public static synchronized boolean handleNotification(String notification) {
        try {
            ObjectMapper mapper = new ObjectMapper();

            TypeReference<HashMap<String, String>> typeRef
                    = new TypeReference<HashMap<String, String>>() {
            };
            Map<String, String> notificationInfo = mapper.readValue(notification, typeRef);

            boolean result = qonversionSandwich.handleNotification(notificationInfo);

            return result;
        } catch (Exception e) {
            return false;
        }
    }

    public static synchronized void subscribeOnAutomationEvents() {
        automationsWrapper.subscribe();
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

    private static void handleErrorResponse(@NotNull SandwichError error, @NotNull String methodName) {
        ObjectMapper mapper = new ObjectMapper();

        ObjectNode errorNode = mapper.createObjectNode();
        errorNode.put("code", error.getCode());
        errorNode.put("description", error.getDescription());
        errorNode.put("additionalMessage", error.getAdditionalMessage());

        ObjectNode rootNode = mapper.createObjectNode();
        rootNode.set("error", errorNode);

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