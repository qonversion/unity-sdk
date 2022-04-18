package com.qonversion.unitywrapper;

import android.util.Log;

import com.fasterxml.jackson.core.JsonProcessingException;

import org.jetbrains.annotations.NotNull;

import java.util.HashMap;

import io.qonversion.sandwich.AutomationsSandwich;

public class AutomationsDelegate implements com.qonversion.android.sdk.automations.AutomationsDelegate {
    private static final String EVENT_SCREEN_SHOWN = "OnAutomationsScreenShown";
    private static final String EVENT_ACTION_STARTED = "OnAutomationsActionStarted";
    private static final String EVENT_ACTION_FAILED = "OnAutomationsActionFailed";
    private static final String EVENT_ACTION_FINISHED = "OnAutomationsActionFinished";
    private static final String EVENT_AUTOMATIONS_FINISHED = "OnAutomationsFinished";

    public static String TAG = "AutomationsDelegate";
    private final MessageSender messageSender;
    private final AutomationsSandwich automationsSandwich;

    public AutomationsDelegate(MessageSender messageSender) {
        this.messageSender = messageSender;
        automationsSandwich = new AutomationsSandwich();
        automationsSandwich.subscribe((event, data) -> {
            String methodName = "";
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
        });
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
