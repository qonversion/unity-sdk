package com.qonversion.unitywrapper;

import android.app.Application;
import android.content.Context;
import android.util.Log;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.unity3d.player.UnityPlayer;

import java.util.HashMap;
import java.util.Map;

import io.qonversion.sandwich.NoCodesEventListener;
import io.qonversion.sandwich.NoCodesPurchaseDelegateBridge;
import io.qonversion.sandwich.NoCodesSandwich;

public class NoCodesWrapper {
    private static final String TAG = "NoCodesWrapper";

    private static final String EVENT_SCREEN_SHOWN = "OnNoCodesScreenShown";
    private static final String EVENT_ACTION_STARTED = "OnNoCodesActionStarted";
    private static final String EVENT_ACTION_FAILED = "OnNoCodesActionFailed";
    private static final String EVENT_ACTION_FINISHED = "OnNoCodesActionFinished";
    private static final String EVENT_FINISHED = "OnNoCodesFinished";
    private static final String EVENT_SCREEN_FAILED_TO_LOAD = "OnNoCodesScreenFailedToLoad";
    private static final String EVENT_PURCHASE = "OnNoCodesPurchase";
    private static final String EVENT_RESTORE = "OnNoCodesRestore";

    private static MessageSender messageSender;
    private static NoCodesSandwich noCodesSandwich;

    public static synchronized void initialize(
            String unityListener,
            String projectKey,
            String proxyUrl,
            String locale,
            String theme,
            String sdkVersion
    ) {
        messageSender = new MessageSender(unityListener);
        noCodesSandwich = new NoCodesSandwich();

        Context context = UnityPlayer.currentActivity.getApplicationContext();

        String effectiveProxyUrl = (proxyUrl == null || proxyUrl.isEmpty()) ? null : proxyUrl;
        String effectiveLocale = (locale == null || locale.isEmpty()) ? null : locale;
        String effectiveTheme = (theme == null || theme.isEmpty()) ? null : theme;

        noCodesSandwich.initialize(
                context,
                projectKey,
                effectiveProxyUrl,
                null, // logLevelKey
                null, // logTag
                effectiveLocale,
                effectiveTheme
        );

        // Store SDK info
        noCodesSandwich.storeSdkInfo(context, "unity", sdkVersion);
    }

    public static synchronized void setDelegate() {
        if (noCodesSandwich == null) {
            Log.e(TAG, "NoCodesSandwich is not initialized");
            return;
        }

        noCodesSandwich.setDelegate(new EventListener());
    }

    public static synchronized void setScreenPresentationConfig(String configJson, String contextKey) {
        if (noCodesSandwich == null) {
            Log.e(TAG, "NoCodesSandwich is not initialized");
            return;
        }

        try {
            ObjectMapper mapper = new ObjectMapper();
            TypeReference<HashMap<String, Object>> typeRef = new TypeReference<HashMap<String, Object>>() {};
            Map<String, Object> configData = mapper.readValue(configJson, typeRef);

            String effectiveContextKey = (contextKey == null || contextKey.isEmpty()) ? null : contextKey;
            noCodesSandwich.setScreenPresentationConfig(configData, effectiveContextKey);
        } catch (JsonProcessingException e) {
            Log.e(TAG, "Failed to parse screen presentation config: " + e.getLocalizedMessage());
        }
    }

    public static synchronized void showScreen(String contextKey) {
        if (noCodesSandwich == null) {
            Log.e(TAG, "NoCodesSandwich is not initialized");
            return;
        }

        noCodesSandwich.showScreen(contextKey);
    }

    public static synchronized void close() {
        if (noCodesSandwich == null) {
            Log.e(TAG, "NoCodesSandwich is not initialized");
            return;
        }

        noCodesSandwich.close();
    }

    public static synchronized void setLocale(String locale) {
        if (noCodesSandwich == null) {
            Log.e(TAG, "NoCodesSandwich is not initialized");
            return;
        }

        String effectiveLocale = (locale == null || locale.isEmpty()) ? null : locale;
        noCodesSandwich.setLocale(effectiveLocale);
    }

    public static synchronized void setTheme(String theme) {
        if (noCodesSandwich == null) {
            Log.e(TAG, "NoCodesSandwich is not initialized");
            return;
        }

        noCodesSandwich.setTheme(theme);
    }

    public static synchronized void setPurchaseDelegate() {
        if (noCodesSandwich == null) {
            Log.e(TAG, "NoCodesSandwich is not initialized");
            return;
        }

        noCodesSandwich.setPurchaseDelegate(new PurchaseDelegateBridge());
    }

    public static synchronized void delegatedPurchaseCompleted() {
        if (noCodesSandwich == null) {
            Log.e(TAG, "NoCodesSandwich is not initialized");
            return;
        }

        noCodesSandwich.delegatedPurchaseCompleted();
    }

    public static synchronized void delegatedPurchaseFailed(String errorMessage) {
        if (noCodesSandwich == null) {
            Log.e(TAG, "NoCodesSandwich is not initialized");
            return;
        }

        noCodesSandwich.delegatedPurchaseFailed(errorMessage);
    }

    public static synchronized void delegatedRestoreCompleted() {
        if (noCodesSandwich == null) {
            Log.e(TAG, "NoCodesSandwich is not initialized");
            return;
        }

        noCodesSandwich.delegatedRestoreCompleted();
    }

    public static synchronized void delegatedRestoreFailed(String errorMessage) {
        if (noCodesSandwich == null) {
            Log.e(TAG, "NoCodesSandwich is not initialized");
            return;
        }

        noCodesSandwich.delegatedRestoreFailed(errorMessage);
    }

    static class EventListener implements NoCodesEventListener {
        @Override
        public void onNoCodesEvent(@NonNull Event event, @Nullable Map<String, ?> data) {
            String methodName;
            switch (event) {
                case ScreenShown:
                    methodName = EVENT_SCREEN_SHOWN;
                    break;
                case ActionStarted:
                    methodName = EVENT_ACTION_STARTED;
                    break;
                case ActionFailed:
                    methodName = EVENT_ACTION_FAILED;
                    break;
                case ActionFinished:
                    methodName = EVENT_ACTION_FINISHED;
                    break;
                case Finished:
                    methodName = EVENT_FINISHED;
                    break;
                case ScreenFailedToLoad:
                    methodName = EVENT_SCREEN_FAILED_TO_LOAD;
                    break;
                default:
                    return;
            }

            sendMessageToUnity(data == null ? new HashMap<>() : data, methodName);
        }
    }

    static class PurchaseDelegateBridge implements NoCodesPurchaseDelegateBridge {
        @Override
        public void purchase(@NonNull Map<String, ?> productData) {
            sendMessageToUnity(productData, EVENT_PURCHASE);
        }

        @Override
        public void restore() {
            sendMessageToUnity(new HashMap<>(), EVENT_RESTORE);
        }
    }

    private static void sendMessageToUnity(@NonNull Object objectToConvert, @NonNull String methodName) {
        try {
            if (messageSender != null) {
                messageSender.sendMessageToUnity(objectToConvert, methodName);
            }
        } catch (JsonProcessingException e) {
            Log.e(TAG, "Failed to send message to Unity: " + e.getLocalizedMessage());
        }
    }
}
