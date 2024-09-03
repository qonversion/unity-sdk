package com.qonversion.unitywrapper;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.node.ObjectNode;

import org.jetbrains.annotations.NotNull;
import org.jetbrains.annotations.Nullable;

import io.qonversion.sandwich.SandwichError;

public class Utils {

    public static ObjectNode createErrorNode(@NotNull SandwichError error) {
        return createErrorNode(error.getCode(), error.getDescription(), error.getAdditionalMessage());
    }

    public static ObjectNode createErrorNode(String code, String description, @Nullable String additionalMessage) {
        ObjectMapper mapper = new ObjectMapper();
        ObjectNode errorNode = mapper.createObjectNode();
        errorNode.put("code", code);
        errorNode.put("description", description);
        if (additionalMessage != null) {
            errorNode.put("additionalMessage", additionalMessage);
        }

        ObjectNode rootNode = mapper.createObjectNode();
        rootNode.set("error", errorNode);
        return rootNode;
    }
}
