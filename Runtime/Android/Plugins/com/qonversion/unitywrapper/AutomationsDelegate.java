package com.qonversion.unitywrapper;

import android.util.Log;

import androidx.annotation.NonNull;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.qonversion.android.sdk.automations.Automations;
import com.qonversion.android.sdk.automations.QActionResult;

import org.jetbrains.annotations.NotNull;

import java.util.HashMap;
import java.util.Map;

public class AutomationsDelegate implements com.qonversion.android.sdk.automations.AutomationsDelegate {
    private static final String EVENT_SCREEN_SHOWN = "OnAutomationsScreenShown";
    private static final String EVENT_ACTION_STARTED = "OnAutomationsActionStarted";
    private static final String EVENT_ACTION_FAILED = "OnAutomationsActionFailed";
    private static final String EVENT_ACTION_FINISHED = "OnAutomationsActionFinished";
    private static final String EVENT_AUTOMATIONS_FINISHED = "OnAutomationsFinished";

    public static String TAG = "AutomationsDelegate";
    private final MessageSender messageSender;

    public AutomationsDelegate(MessageSender messageSender) {
        this.messageSender = messageSender;
        Automations.setDelegate(this);
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

    @Override
    public void automationsDidShowScreen(@NonNull String screenId) {
        Map<String, Object> result = new HashMap<>();
        result.put("screenID", screenId);
        sendMessageToUnity(result, EVENT_SCREEN_SHOWN);
    }

    @Override
    public void automationsDidStartExecuting(@NonNull QActionResult actionResult) {
        Map<String, Object> result = Mapper.mapActionResult(actionResult);
        sendMessageToUnity(result, EVENT_ACTION_STARTED);
    }

    @Override
    public void automationsDidFailExecuting(@NonNull QActionResult actionResult) {
        Map<String, Object> result = Mapper.mapActionResult(actionResult);
        sendMessageToUnity(result, EVENT_ACTION_FAILED);
    }

    @Override
    public void automationsDidFinishExecuting(@NonNull QActionResult actionResult) {
        Map<String, Object> result = Mapper.mapActionResult(actionResult);
        sendMessageToUnity(result, EVENT_ACTION_FINISHED);
    }

    @Override
    public void automationsFinished() {
        Map<String, Object> result = new HashMap<>();
        sendMessageToUnity(result, EVENT_AUTOMATIONS_FINISHED);
    }
}
