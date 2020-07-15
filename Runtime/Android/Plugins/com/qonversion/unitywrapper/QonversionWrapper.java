package com.qonversion.unitywrapper;

import android.app.Activity;
import android.content.Context;
import android.os.AsyncTask;
import android.util.Log;

import com.android.billingclient.api.Purchase;
import com.android.billingclient.api.SkuDetails;
import com.fasterxml.jackson.core.JsonGenerationException;
import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.qonversion.android.sdk.AttributionSource;
import com.qonversion.android.sdk.Qonversion;
import com.qonversion.android.sdk.QonversionCallback;
import com.qonversion.unitywrapper.internal.Validate;
import com.unity3d.player.UnityPlayer;

import org.jetbrains.annotations.NotNull;
import org.json.JSONException;

import java.io.IOException;
import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.Executor;

public class QonversionWrapper {
    public static String TAG = "QonversionWrapper";

    private static Executor executor;

    private static final Object LOCK = new Object();

    private static QonversionWrapper INSTANCE;

    private static Context applicationContext;

    private static Boolean sdkInitialized = false;

    private static Class<?> unityPlayer;

    private QonversionWrapper(String projectKey, String userID) {
        Log.d(TAG, "Qonversion Initialize starting with userID: " + userID);

        Activity unityActivity = UnityPlayer.currentActivity;

        applicationContext = UnityPlayer.currentActivity.getApplicationContext();

        Qonversion.initialize(unityActivity.getApplication(), projectKey, userID, new QonversionCallback() {
            @Override
            public void onSuccess(@NotNull String uid) {
                Log.d(TAG, "Qonversion initialized. UID: " + uid);

                sdkInitialized = true;

                QonversionWrapper.getExecutor()
                        .execute(
                                new Runnable() {
                                    @Override
                                    public void run() {
                                        // Automatically log In App Purchase events
                                        InAppPurchaseActivityLifecycleTracker.update();
                                    }
                                });
            }

            @Override
            public void onError(@NotNull Throwable t) {
                Log.d(TAG, "Qonversion initializing error: " + t.getLocalizedMessage());
            }
        });
    }

    public static synchronized boolean isInitialized() {
        return sdkInitialized;
    }

    public static synchronized void initialize(String projectKey, String userID) {
        if (INSTANCE != null){
            Log.w(TAG, "Qonversion SDK is already initialized");
            return;
        }

        INSTANCE = new QonversionWrapper(projectKey, userID);
    }

    public static synchronized void attribution(String conversionData, String attributionSource, String conversionUid) {
        Validate.sdkInitialized();

        try {
            Qonversion qonversion = Qonversion.getInstance();
            if (qonversion == null) {
                Log.w(TAG, "Qonversion isn't initialized");
                return;
            }
            AttributionSource source = AttributionSource.valueOf(attributionSource);

            ObjectMapper mapper = new ObjectMapper();

            TypeReference<HashMap<String, Object>> typeRef
                    = new TypeReference<HashMap<String, Object>>() {
            };
            Map<String, Object> conversionInfo = mapper.readValue(conversionData, typeRef);

            qonversion.attribution(conversionInfo, source, conversionUid);
            Log.e(TAG, "Attribution sent");
            
        } catch (Exception e) {
            Log.e(TAG, "Purchases. " + "pushAttribution error: " + e.getLocalizedMessage());
        }
    }

    public static synchronized void trackPurchase(String jsonSkuDetails, String jsonPurchaseInfo, String signature){
        Validate.sdkInitialized();

        try {
            Qonversion qonversion = Qonversion.getInstance();
            if (qonversion == null){
                Log.w(TAG, "Qonversion isn't initialized");
                return;
            }

            SkuDetails s = new SkuDetails(jsonSkuDetails);
            Purchase p = new Purchase(jsonPurchaseInfo, signature);

            qonversion.purchase(s, p, new QonversionCallback() {
                @Override
                public void onSuccess(@NotNull String s) {
                    Log.d(TAG, "Purchase tracked: " + s);
                }

                @Override
                public void onError(@NotNull Throwable throwable) {
                    Log.e(TAG, "Purchase track error: " + throwable.getLocalizedMessage());
                }
            });
        } catch (JSONException e) {
            logJSONException(e);
        }
    }

    public static Executor getExecutor() {
        synchronized (LOCK) {
            if (QonversionWrapper.executor == null) {
                QonversionWrapper.executor = AsyncTask.THREAD_POOL_EXECUTOR;
            }
        }
        return QonversionWrapper.executor;
    }

    public static Context getApplicationContext() {
        Validate.sdkInitialized();
        return applicationContext;
    }

    public static boolean isImplicitPurchaseLoggingEnabled() {
        return true;
    }

    private static void logJSONException(JSONException e) {
        Log.e(TAG, "Purchases. " + "JSON Error: " + e.getLocalizedMessage());
    }
}