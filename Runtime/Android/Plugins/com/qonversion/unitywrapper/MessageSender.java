package com.qonversion.unitywrapper;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.unity3d.player.UnityPlayer;

import org.jetbrains.annotations.NotNull;

public class MessageSender {
    private final String unityListenerName;

    public MessageSender(String unityListenerName) {
        this.unityListenerName = unityListenerName;
    }

    void sendMessageToUnity(@NotNull Object objectToConvert, @NotNull String methodName) throws JsonProcessingException {
        ObjectMapper mapper = new ObjectMapper();
        String json = mapper.writeValueAsString(objectToConvert);
        UnityPlayer.UnitySendMessage(unityListenerName, methodName, json);
    }
}
