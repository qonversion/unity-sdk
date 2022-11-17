package com.qonversion.unitywrapper;

import android.util.Log;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;

import org.jetbrains.annotations.NotNull;

import java.util.HashMap;
import java.util.Map;

import io.qonversion.sandwich.AutomationsEventListener;
import io.qonversion.sandwich.AutomationsSandwich;

@SuppressWarnings("UnnecessaryLocalVariable")
public class AutomationsWrapper implements AutomationsEventListener {
    private static final String EVENT_SCREEN_SHOWN = "OnAutomationsScreenShown";
    private static final String EVENT_ACTION_STARTED = "OnAutomationsActionStarted";
    private static final String EVENT_ACTION_FAILED = "OnAutomationsActionFailed";
    private static final String EVENT_ACTION_FINISHED = "OnAutomationsActionFinished";
    private static final String EVENT_AUTOMATIONS_FINISHED = "OnAutomationsFinished";

    public static String TAG = "AutomationsDelegate";
    private final MessageSender messageSender;
    private final AutomationsSandwich automationsSandwich;

    public AutomationsWrapper(MessageSender messageSender) {
        this.messageSender = messageSender;
        automationsSandwich = new AutomationsSandwich();
    }

    public void initialize() {
        automationsSandwich.initialize();
    }

    public void subscribe() {
        automationsSandwich.setDelegate(this);
    }

    public void setNotificationsToken(String token) {
        automationsSandwich.setNotificationToken(token);
    }

    public boolean handleNotification(String notification) {
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
    public Map<String, Object> getNotificationCustomPayload(String notification) {
        try {
            ObjectMapper mapper = new ObjectMapper();

            TypeReference<HashMap<String, String>> typeRef
                    = new TypeReference<HashMap<String, String>>() {
            };
            Map<String, String> notificationInfo = mapper.readValue(notification, typeRef);

            Map<String, Object> payload = automationsSandwich.getNotificationCustomPayload(notificationInfo);

            return payload;
        } catch (Exception e) {
            return null;
        }
    }

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

    private void sendMessageToUnity(@NotNull Object objectToConvert, @NotNull String methodName) {
        try {
            messageSender.sendMessageToUnity(objectToConvert, methodName);
        } catch (JsonProcessingException e) {
            handleException(e);
        }
    }

    private void handleException(Exception e) {
        Log.e(TAG, "An error occurred while processing automations flow: " + e.getLocalizedMessage());
    }
}
