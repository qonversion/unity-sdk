package com.qonversion.unitywrapper;

import android.util.Log;

import androidx.annotation.Nullable;

import com.android.billingclient.api.BillingClient;
import com.android.billingclient.api.BillingResult;
import com.android.billingclient.api.Purchase;
import com.android.billingclient.api.PurchasesUpdatedListener;
import com.android.billingclient.api.SkuDetails;
import com.qonversion.android.sdk.Qonversion;
import com.qonversion.android.sdk.QonversionBillingBuilder;
import com.qonversion.android.sdk.QonversionCallback;
import com.unity3d.player.UnityPlayer;

import org.jetbrains.annotations.NotNull;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.List;

public class QonversionWrapper {
    //unity methods
    private static QonversionWrapper INSTANCE;

    private String gameObject;

    private static String TAG = "QonversionWrapper";

    private QonversionWrapper(String gameObject_, String projectKey, String userID){
        //TODO: add logic checks
        gameObject = gameObject_;
        Log.d(TAG, "Initialize starting with projectKey: " + projectKey +
                "; userID: " + userID);

        Qonversion.initialize(UnityPlayer.currentActivity.getApplication(), projectKey, userID, buildBilling(), true);
        Log.d(TAG, "Qonversion initialized");
    }

    private static QonversionBillingBuilder buildBilling() {
        return new QonversionBillingBuilder()
                .enablePendingPurchases()
                .setListener(new PurchasesUpdatedListener() {
                    @Override
                    public void onPurchasesUpdated(BillingResult billingResult, @Nullable List<Purchase> purchases) {
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

    public static void trackPurchase(String jsonSkuDetails, String jsonPurchaseInfo, String signature){
        try {
            Qonversion q = Qonversion.getInstance();
            if (q == null){
                Log.w(TAG, "Qonversion isn't initialized");
                return;
            }
            q.purchase(new SkuDetails(jsonSkuDetails), new Purchase(jsonPurchaseInfo, signature), new QonversionCallback() {
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
