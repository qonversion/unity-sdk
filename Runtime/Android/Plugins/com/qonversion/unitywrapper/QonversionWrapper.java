package com.qonversion.unitywrapper;

import android.app.Activity;
import android.content.Context;
import android.os.AsyncTask;
import android.util.Log;
import com.unity3d.player.UnityPlayer;

import org.jetbrains.annotations.NotNull;
import org.json.JSONException;

import java.io.IOException;
import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.Executor;
import com.fasterxml.jackson.core.JsonGenerationException;
import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;

import com.qonversion.android.sdk.AttributionSource;
import com.qonversion.android.sdk.QUserProperties;
import com.qonversion.android.sdk.Qonversion;
import com.qonversion.android.sdk.QonversionEligibilityCallback;
import com.qonversion.android.sdk.QonversionError;
import com.qonversion.android.sdk.QonversionExperimentsCallback;
import com.qonversion.android.sdk.QonversionLaunchCallback;
import com.qonversion.android.sdk.QonversionOfferingsCallback;
import com.qonversion.android.sdk.QonversionPermissionsCallback;
import com.qonversion.android.sdk.QonversionProductsCallback;
import com.qonversion.android.sdk.dto.QLaunchResult;
import com.qonversion.android.sdk.dto.experiments.QExperimentInfo;
import com.qonversion.android.sdk.dto.offerings.QOfferings;
import com.qonversion.android.sdk.dto.QPermission;
import com.qonversion.android.sdk.dto.products.QProduct;

import android.os.Handler;

public class QonversionWrapper {

    public static String TAG = "QonversionWrapper";

    private static Executor executor;

    private static final Object LOCK = new Object();

    private static Handler mUnityMainThreadHandler;

    public static synchronized void launch(String projectKey, boolean observerMode) {
        Log.d(TAG, "Qonversion Launch starting. Project key: " + projectKey);

        Activity unityActivity = UnityPlayer.currentActivity;

        Qonversion.launch(unityActivity.getApplication(), projectKey, observerMode, new QonversionLaunchCallback()
        {
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
            Log.e(TAG, "Purchases. " + "pushAttribution error: " + e.getLocalizedMessage());
        }
    }

    public static synchronized void setUserId(String value) {
        Qonversion.setUserID(value);
    }

    public void runOnUnityThread(Runnable runnable) {
        if (mUnityMainThreadHandler != null && runnable != null) {
            mUnityMainThreadHandler.post(runnable);
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
}