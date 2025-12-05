package com.qonversion.unitywrapper;

import android.util.Log;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.unity3d.player.UnityPlayer;

import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

import java.util.Map;

import io.qonversion.sandwich.NoCodesSandwich;
import io.qonversion.sandwich.NoCodesEventListener;
import io.qonversion.sandwich.NoCodesEventListener.Event;

public class NoCodesWrapper {
    public static String TAG = "NoCodesWrapper";

    private static MessageSender messageSender;
    private static NoCodesSandwich noCodesSandwich;
    private static NoCodesEventListener noCodesEventListener;

    public static synchronized void initialize(String unityListener) {
        messageSender = new MessageSender(unityListener);

        noCodesSandwich = new NoCodesSandwich();
        
        noCodesEventListener = new NoCodesEventListener() {
            @Override
            public void onNoCodesEvent(@NotNull Event event, @NotNull Map<String, ? extends Object> payload) {
                String methodName;
                if (event == Event.ScreenShown) {
                    methodName = "OnNoCodesScreenShown";
                } else if (event == Event.Finished) {
                    methodName = "OnNoCodesFinished";
                } else if (event == Event.ActionStarted) {
                    methodName = "OnNoCodesActionStarted";
                } else if (event == Event.ActionFailed) {
                    methodName = "OnNoCodesActionFailed";
                } else if (event == Event.ActionFinished) {
                    methodName = "OnNoCodesActionFinished";
                } else if (event == Event.ScreenFailedToLoad) {
                    methodName = "OnNoCodesScreenFailedToLoad";
                } else {
                    Log.w(TAG, "Unknown NoCodes event: " + event);
                    return;
                }
                sendMessageToUnity(payload, methodName);
            }
        };
        
        noCodesSandwich.setDelegate(noCodesEventListener);
    }

    public static synchronized void initializeNoCodes(
            String projectKey,
            String version,
            String source,
            @Nullable String proxyUrl
    ) {
        noCodesSandwich.initialize(
                UnityPlayer.currentActivity,
                projectKey,
                proxyUrl,
                null,
                null
        );
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

    private static void sendMessageToUnity(@NotNull Object objectToConvert, @NotNull String methodName) {
        try {
            messageSender.sendMessageToUnity(objectToConvert, methodName);
        } catch (JsonProcessingException e) {
            Log.e(TAG, "An error occurred while serializing data: " + e.getLocalizedMessage());
        }
    }
}
