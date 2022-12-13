package com.qonversion.unitywrapper;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.node.ObjectNode;

import org.jetbrains.annotations.NotNull;
import io.qonversion.sandwich.SandwichError;

public class Utils {

    public static ObjectNode createErrorNode(@NotNull SandwichError error) {
        ObjectMapper mapper = new ObjectMapper();
        ObjectNode errorNode = mapper.createObjectNode();
        errorNode.put("code", error.getCode());
        errorNode.put("description", error.getDescription());
        errorNode.put("additionalMessage", error.getAdditionalMessage());

        ObjectNode rootNode = mapper.createObjectNode();
        rootNode.set("error", errorNode);
        return rootNode;
    }
}
