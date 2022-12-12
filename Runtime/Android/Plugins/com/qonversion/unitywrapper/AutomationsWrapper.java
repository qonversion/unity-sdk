package com.qonversion.unitywrapper;

import android.util.Log;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.node.ObjectNode;

import org.jetbrains.annotations.NotNull;

import java.util.HashMap;
import java.util.Map;

import io.qonversion.sandwich.AutomationsEventListener;
import io.qonversion.sandwich.AutomationsSandwich;
import io.qonversion.sandwich.ResultListener;
import io.qonversion.sandwich.SandwichError;

@SuppressWarnings("UnnecessaryLocalVariable")
public class AutomationsWrapper {
    private static final String EVENT_SCREEN_SHOWN = "OnAutomationsScreenShown";
    private static final String EVENT_ACTION_STARTED = "OnAutomationsActionStarted";
    private static final String EVENT_ACTION_FAILED = "OnAutomationsActionFailed";
    private static final String EVENT_ACTION_FINISHED = "OnAutomationsActionFinished";
    private static final String EVENT_AUTOMATIONS_FINISHED = "OnAutomationsFinished";

    public static String TAG = "AutomationsDelegate";
    private static MessageSender messageSender;
    private static AutomationsSandwich automationsSandwich;

    public static synchronized void initialize(String unityListener) {
        messageSender = new MessageSender(unityListener);
        automationsSandwich = new AutomationsSandwich();
    }

    public static synchronized void subscribeOnAutomationEvents() {
        automationsSandwich.setDelegate(new EventListener());
    }

    public static synchronized void setNotificationsToken(String token) {
        automationsSandwich.setNotificationToken(token);
    }

    public static synchronized boolean handleNotification(String notification) {
        try {
            ObjectMapper mapper = new ObjectMapper();

            TypeReference<HashMap<String, String>> typeRef
                    = new TypeReference<HashMap<String, String>>() {
            };
            Map<String, String> notificationInfo = mapper.readValue(notification, typeRef);

            boolean result = automationsSandwich.handleNotification(notificationInfo);

            return result;
        } catch (Exception e) {
            return false;
        }
    }

    @Nullable
    public static synchronized String getNotificationCustomPayload(String notification) {
        try {
            final ObjectMapper mapper = new ObjectMapper();

            final TypeReference<HashMap<String, String>> typeRef
                    = new TypeReference<HashMap<String, String>>() {
            };
            final Map<String, String> notificationInfo = mapper.readValue(notification, typeRef);

            final Map<String, Object> payload = automationsSandwich.getNotificationCustomPayload(notificationInfo);
            final String json = mapper.writeValueAsString(payload);

            return json;
        } catch (Exception e) {
            return null;
        }
    }

    public static synchronized void showScreen(String screenId, String unityCallbackName) {
        automationsSandwich.showScreen(screenId, new ResultListener() {
            @Override
            public void onSuccess(@NonNull Map<String, ?> data) {
                sendMessageToUnity(data, unityCallbackName);
            }

            @Override
            public void onError(@NonNull SandwichError error) {
                handleErrorResponse(error, unityCallbackName);
            }
        });
    }

    private static void handleErrorResponse(@NotNull SandwichError error, @NotNull String methodName) {
        final ObjectNode rootNode = Utils.createErrorNode(error);

        sendMessageToUnity(rootNode, methodName);
    }

    static class EventListener implements AutomationsEventListener {
        @Override
        public void onAutomationEvent(@NonNull Event event, @Nullable Map<String, ?> data) {
            String methodName;
            switch (event) {
                case ScreenShown:
                    methodName = EVENT_SCREEN_SHOWN;
                    break;
                case ActionStarted:
                    methodName = EVENT_ACTION_STARTED;
                    break;
                case ActionFinished:
                    methodName = EVENT_ACTION_FINISHED;
                    break;
                case ActionFailed:
                    methodName = EVENT_ACTION_FAILED;
                    break;
                case AutomationsFinished:
                    methodName = EVENT_AUTOMATIONS_FINISHED;
                    break;
                default:
                    return;
            }

            sendMessageToUnity(data == null ? new HashMap<>() : data, methodName);
        }
    }

    private static void sendMessageToUnity(@NotNull Object objectToConvert, @NotNull String methodName) {
        try {
            messageSender.sendMessageToUnity(objectToConvert, methodName);
        } catch (JsonProcessingException e) {
            handleException(e);
        }
    }

    private static void handleException(Exception e) {
        Log.e(TAG, "An error occurred while processing automations flow: " + e.getLocalizedMessage());
    }
}
