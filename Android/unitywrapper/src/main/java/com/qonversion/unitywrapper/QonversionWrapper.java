package com.qonversion.unitywrapper;

import android.util.Log;
import android.util.Pair;

import androidx.annotation.Nullable;

import com.android.billingclient.api.BillingClient;
import com.android.billingclient.api.BillingResult;
import com.android.billingclient.api.Purchase;
import com.android.billingclient.api.PurchasesUpdatedListener;
import com.android.billingclient.api.SkuDetails;
import com.qonversion.android.sdk.AttributionSource;
import com.qonversion.android.sdk.GooglePurchaseConverter;
import com.qonversion.android.sdk.Qonversion;
import com.qonversion.android.sdk.QonversionBillingBuilder;
import com.qonversion.android.sdk.QonversionCallback;
import com.qonversion.unitywrapper.utils.Helper;
import com.unity3d.player.UnityPlayer;

import org.jetbrains.annotations.NotNull;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class QonversionWrapper {
    //unity methods
    private static QonversionWrapper INSTANCE;

    private String gameObject;

    public static String TAG = "QonversionWrapper";

    private QonversionWrapper(String gameObject_, String projectKey, String userID){
        //TODO: add logic checks
        gameObject = gameObject_;
        Log.d(TAG, "Initialize starting with projectKey: " + projectKey +
                "; userID: " + userID);

        Qonversion.initialize(UnityPlayer.currentActivity.getApplication(), projectKey, userID, new QonversionCallback() {
            @Override
            public void onSuccess(@NotNull String uid) {
                Log.d(TAG, "Qonversion initialized. UID: " + uid);
            }

            @Override
            public void onError(@NotNull Throwable t) {
                Log.d(TAG, "Qonversion initializing error: " + t.getLocalizedMessage());
            }
        });
    }


    public static synchronized void setup(String gameObject_, String projectKey, String userID) {
        if (INSTANCE != null){
            Log.w(TAG, "Qonversion SDK is already initialized");
            return;
        }

        INSTANCE = new QonversionWrapper(gameObject_, projectKey, userID);
    }

    public static synchronized void pushAttribution(HashMap<Object, Object> conversionData, String attributionSource, String conversionUid) {
        try {
            Qonversion q = Qonversion.getInstance();
            if (q == null){
                Log.w(TAG, "Qonversion isn't initialized");
                return;
            }
            q.attribution(Helper.convertToMap(conversionData), AttributionSource.APPS_FLYER, conversionUid);
            Log.e(TAG, "Attribution sent");
        } catch (Exception e) {
            Log.e(TAG, "Purchases. " + "pushAttribution error: " + e.getLocalizedMessage());
        }
    }



    public static synchronized void trackPurchase(String jsonSkuDetails, String jsonPurchaseInfo, String signature){
        try {
            Qonversion q = Qonversion.getInstance();
            if (q == null){
                Log.w(TAG, "Qonversion isn't initialized");
                return;
            }

            GooglePurchaseConverter converter = new GooglePurchaseConverter();
            SkuDetails s = new SkuDetails(jsonSkuDetails);
            Purchase p = new Purchase(jsonPurchaseInfo, signature);
            com.qonversion.android.sdk.entity.Purchase qqwe = converter.convert(new Pair<SkuDetails, Purchase>(s, p));
            Log.w(TAG, "Converted purchase: " + qqwe.toString());

            q.purchase(s, p, new QonversionCallback() {
                @Override
                public void onSuccess(@NotNull String s) {
                    Log.d(TAG, "Purchase tracked: " + s);
                }

                @Override
                public void onError(@NotNull Throwable throwable) {
                    Log.e(TAG, "Purchase error: " + throwable.getLocalizedMessage());
                }
            });
        } catch (JSONException e) {
            logJSONException(e);
        }
    }

    private static void logJSONException(JSONException e) {
        Log.e(TAG, "Purchases. " + "JSON Error: " + e.getLocalizedMessage());
    }
}
