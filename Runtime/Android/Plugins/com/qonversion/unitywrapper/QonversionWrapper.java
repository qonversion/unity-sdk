package com.qonversion.unitywrapper;

import android.app.Activity;
import android.os.AsyncTask;
import android.util.Log;

import com.fasterxml.jackson.databind.node.ObjectNode;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.qonversion.android.sdk.QonversionOfferingsCallback;
import com.qonversion.android.sdk.QonversionProductsCallback;
import com.qonversion.android.sdk.dto.offerings.QOfferings;
import com.qonversion.android.sdk.dto.products.QProduct;
import com.unity3d.player.UnityPlayer;

import org.jetbrains.annotations.NotNull;

import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.Executor;
import java.util.List;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;

import com.qonversion.android.sdk.AttributionSource;
import com.qonversion.android.sdk.Qonversion;
import com.qonversion.android.sdk.QonversionError;
import com.qonversion.android.sdk.QonversionLaunchCallback;
import com.qonversion.android.sdk.QonversionPermissionsCallback;
import com.qonversion.android.sdk.dto.QLaunchResult;
import com.qonversion.android.sdk.dto.QPermission;

import android.os.Handler;

public class QonversionWrapper {
    private static final String ON_CHECK_PERMISSIONS_METHOD = "OnCheckPermissions";
    private static final String ON_PURCHASE_METHOD = "OnPurchase";
    private static final String ON_UPDATE_PURCHASE_METHOD = "OnUpdatePurchase";
    private static final String ON_RESTORE_METHOD = "OnRestore";
    private static final String ON_PRODUCTS_METHOD = "OnProducts";
    private static final String ON_OFFERINGS_METHOD = "OnOfferings";

    public static String TAG = "QonversionWrapper";

    private static String unityListenerName;

    private static Executor executor;

    private static final Object LOCK = new Object();

    private static Handler mUnityMainThreadHandler;

    public static synchronized void launch(String unityListener, String projectKey, boolean observerMode) {
        Log.d(TAG, "Qonversion Launch starting. Project key: " + projectKey);

        unityListenerName = unityListener;

        Activity unityActivity = UnityPlayer.currentActivity;

        Qonversion.launch(unityActivity.getApplication(), projectKey, observerMode, new QonversionLaunchCallback() {
            @Override
            public void onSuccess(@NotNull QLaunchResult launchResult) {
                Log.d(TAG, "Qonversion initialized. UID: " + launchResult.getUid());
            }

            @Override
            public void onError(@NotNull QonversionError qonversionError) {
                Log.d(TAG, "Qonversion initializing error: " + qonversionError.getCode() + ", " + qonversionError.getDescription() + ", " + qonversionError.getAdditionalMessage());
            }
        });
    }

    public static synchronized void syncPurchases() {
        Qonversion.syncPurchases();
    }

    public static synchronized void setDebugMode() {
        Qonversion.setDebugMode();
    }

    public static synchronized void setUserID(String userID) {
        Qonversion.setUserID(userID);
    }

    public static synchronized void attribution(String conversionData, String attributionSource) {
        try {
            AttributionSource source = AttributionSource.valueOf(attributionSource);

            ObjectMapper mapper = new ObjectMapper();

            TypeReference<HashMap<String, Object>> typeRef
                    = new TypeReference<HashMap<String, Object>>() {
            };
            Map<String, Object> conversionInfo = mapper.readValue(conversionData, typeRef);

            Qonversion.attribution(conversionInfo, source);
            Log.d(TAG, "Attribution sent");
        } catch (Exception e) {
            Log.e(TAG, "pushAttribution error: " + e.getLocalizedMessage());
        }
    }

    public static synchronized void setUserId(String value) {
        Qonversion.setUserID(value);
    }

    public static synchronized void checkPermissions() {
        Qonversion.checkPermissions(new QonversionPermissionsCallback() {
            @Override
            public void onSuccess(@NotNull Map<String, QPermission> permissions) {
                handlePermissionsResponse(permissions, ON_CHECK_PERMISSIONS_METHOD);
            }

            @Override
            public void onError(@NotNull QonversionError error) {
                handleErrorResponse(error, ON_CHECK_PERMISSIONS_METHOD);
            }
        });
    }

    public static synchronized void purchase(String productId) {
        Qonversion.purchase(UnityPlayer.currentActivity, productId, new QonversionPermissionsCallback() {
            @Override
            public void onSuccess(@NotNull Map<String, QPermission> permissions) {
                handlePermissionsResponse(permissions, ON_PURCHASE_METHOD);
            }

            @Override
            public void onError(@NotNull QonversionError error) {
                handleErrorResponse(error, ON_PURCHASE_METHOD);
            }
        });
    }

    public static synchronized void updatePurchase(String productId, String oldProductId, int prorationMode) {
        Qonversion.updatePurchase(UnityPlayer.currentActivity, productId, oldProductId, prorationMode, new QonversionPermissionsCallback() {
            @Override
            public void onSuccess(@NotNull Map<String, QPermission> permissions) {
                handlePermissionsResponse(permissions, ON_UPDATE_PURCHASE_METHOD);
            }

            @Override
            public void onError(@NotNull QonversionError error) {
                handleErrorResponse(error, ON_UPDATE_PURCHASE_METHOD);
            }
        });
    }

    public static synchronized void restore() {
        Qonversion.restore(new QonversionPermissionsCallback() {
            @Override
            public void onSuccess(@NotNull Map<String, QPermission> permissions) {
                handlePermissionsResponse(permissions, ON_RESTORE_METHOD);
            }

            @Override
            public void onError(@NotNull QonversionError error) {
                handleErrorResponse(error, ON_RESTORE_METHOD);
            }
        });
    }

    public static synchronized void products() {
        Qonversion.products(new QonversionProductsCallback() {
            @Override
            public void onSuccess(@NotNull Map<String, QProduct> products) {
                List<Map<String, Object>> mappedProducts = Mapper.mapProducts(products);
                sendMessageToUnity(mappedProducts, ON_PRODUCTS_METHOD);
            }

            @Override
            public void onError(@NotNull QonversionError error) {
                handleErrorResponse(error, ON_PRODUCTS_METHOD);
            }
        });
    }

    public static synchronized void offerings() {
        Qonversion.offerings(new QonversionOfferingsCallback() {
            @Override
            public void onSuccess(@NotNull QOfferings qOfferings) {
                Map<String, Object> mappedOfferings = Mapper.mapOfferings(qOfferings);
                sendMessageToUnity(mappedOfferings, ON_OFFERINGS_METHOD);
            }

            @Override
            public void onError(@NotNull QonversionError error) {
                handleErrorResponse(error, ON_OFFERINGS_METHOD);
            }
        });
    }

    private static void handlePermissionsResponse(@NotNull Map<String, QPermission> permissions, @NotNull String methodName) {
        List<Map<String, Object>> mappedPermissions = Mapper.mapPermissions(permissions);
        sendMessageToUnity(mappedPermissions, methodName);
    }

    private static void handleErrorResponse(@NotNull QonversionError error, @NotNull String methodName) {
        ObjectMapper mapper = new ObjectMapper();
        ObjectNode rootNode = mapper.createObjectNode();
        String message = String.format("%s. %s", error.getDescription(), error.getAdditionalMessage());

        ObjectNode errorNode = mapper.createObjectNode();
        errorNode.put("message", message);
        errorNode.put("code", error.getCode().name());

        rootNode.set("error", errorNode);

        sendMessageToUnity(rootNode, methodName);
    }

    private static void sendMessageToUnity(@NotNull Object objectToConvert, @NotNull String methodName) {
        try {
            ObjectMapper mapper = new ObjectMapper();
            String json = mapper.writeValueAsString(objectToConvert);
            UnityPlayer.UnitySendMessage(unityListenerName, methodName, json);
        } catch (JsonProcessingException e) {
            handleJsonProcessingException(e);
        }
    }

    private static void handleJsonProcessingException(JsonProcessingException e) {
        Log.e(TAG, "An error occurred while serializing data: " + e.getLocalizedMessage());
    }

    private void runOnUnityThread(Runnable runnable) {
        if (mUnityMainThreadHandler != null && runnable != null) {
            mUnityMainThreadHandler.post(runnable);
        }
    }

    private static Executor getExecutor() {
        synchronized (LOCK) {
            if (QonversionWrapper.executor == null) {
                QonversionWrapper.executor = AsyncTask.THREAD_POOL_EXECUTOR;
            }
        }
        return QonversionWrapper.executor;
    }
}