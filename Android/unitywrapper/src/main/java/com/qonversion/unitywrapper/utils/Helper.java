package com.qonversion.unitywrapper.utils;

import java.util.HashMap;
import java.util.Map;

public class Helper {
    public static Map<String, Object> convertToMap(Map<Object, Object> raw) {
        final Map<String, Object> map = new HashMap<String, Object>(raw.size());
        for (Map.Entry<Object, Object> en : raw.entrySet())
        {
            map.put(en.getKey().toString(), en.getValue());
        }
        return map;
    }
}
