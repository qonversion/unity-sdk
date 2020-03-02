package com.qonversion.unitywrapper.utils

import org.json.JSONArray
import org.json.JSONObject

fun Map<String, *>.convertToJson(): JSONObject {
    val jsonObject = JSONObject()
    for ((key, value) in this) {
        when (value) {
            null -> jsonObject.put(key, JSONObject.NULL)
            is Map<*, *> -> jsonObject.put(key, (value as Map<String, *>).convertToJson())
            is List<*> -> jsonObject.put(key, value.convertToJsonArray())
            is Array<*> -> jsonObject.put(key, value.toList().convertToJsonArray())
            else -> jsonObject.put(key, value)
        }
    }
    return jsonObject
}

fun List<*>.convertToJsonArray(): JSONArray {
    val writableArray = JSONArray()
    for (item in this) {
        when (item) {
            null -> writableArray.put(JSONObject.NULL)
            is Map<*, *> -> writableArray.put((item as Map<String, *>).convertToJson())
            is Array<*> -> writableArray.put(item.asList().convertToJsonArray())
            is List<*> -> writableArray.put(item.convertToJsonArray())
            else -> writableArray.put(item)
        }
    }
    return writableArray
}
