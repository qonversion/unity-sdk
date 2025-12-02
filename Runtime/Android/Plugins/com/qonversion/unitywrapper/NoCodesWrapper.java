package com.qonversion.unitywrapper;

import android.app.Application;
import android.util.Log;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.unity3d.player.UnityPlayer;

import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

import java.util.HashMap;
import java.util.Map;

import io.qonversion.sandwich.NoCodesSandwich;
import io.qonversion.sandwich.NoCodesEventListener;

public class NoCodesWrapper {
    public static String TAG = "NoCodesWrapper";

    private static MessageSender messageSender;
    private static NoCodesSandwich noCodesSandwich;

    public static synchronized void initialize(String unityListener) {
        messageSender = new MessageSender(unityListener);

        final Application application = UnityPlayer.currentActivity.getApplication();
        noCodesSandwich = new NoCodesSandwich(
                application,
                () -> UnityPlayer.currentActivity,
                new NoCodesEventListener() {
                    @Override
                    public void onScreenShown(@NotNull Map<String, ?> payload) {
                        sendMessageToUnity(payload, "OnNoCodesScreenShown");
                    }

                    @Override
                    public void onFinished(@NotNull Map<String, ?> payload) {
                        sendMessageToUnity(payload, "OnNoCodesFinished");
                    }

                    @Override
                    public void onActionStarted(@NotNull Map<String, ?> payload) {
                        sendMessageToUnity(payload, "OnNoCodesActionStarted");
                    }

                    @Override
                    public void onActionFailed(@NotNull Map<String, ?> payload) {
                        sendMessageToUnity(payload, "OnNoCodesActionFailed");
                    }

                    @Override
                    public void onActionFinished(@NotNull Map<String, ?> payload) {
                        sendMessageToUnity(payload, "OnNoCodesActionFinished");
                    }

                    @Override
                    public void onScreenFailedToLoad(@NotNull Map<String, ?> payload) {
                        sendMessageToUnity(payload, "OnNoCodesScreenFailedToLoad");
                    }
                }
        );
    }

    public static synchronized void initializeNoCodes(
            String projectKey,
            String version,
            String source,
            @Nullable String proxyUrl
    ) {
        noCodesSandwich.initialize(projectKey, version, source, proxyUrl);
    }

    public static synchronized void setScreenPresentationConfig(String configJson, @Nullable String contextKey) {
        try {
            ObjectMapper mapper = new ObjectMapper();
            Map<String, Object> config = mapper.readValue(configJson, Map.class);
            noCodesSandwich.setScreenPresentationConfig(config, contextKey);
        } catch (JsonProcessingException e) {
            Log.e(TAG, "An error occurred while parsing presentation config: " + e.getLocalizedMessage());
        }
    }

    public static synchronized void showNoCodesScreen(String contextKey) {
        noCodesSandwich.showScreen(contextKey);
    }

    public static synchronized void closeNoCodes() {
        noCodesSandwich.close();
    }

    private static void sendMessageToUnity(@NotNull Map<String, ?> data, @NotNull String methodName) {
        try {
            messageSender.sendMessageToUnity(data, methodName);
        } catch (JsonProcessingException e) {
            Log.e(TAG, "An error occurred while serializing data: " + e.getLocalizedMessage());
        }
    }
}
