package com.qonversion.unitywrapper;

import android.util.Log;

import androidx.annotation.Nullable;

import com.android.billingclient.api.BillingClient;
import com.android.billingclient.api.BillingResult;
import com.android.billingclient.api.Purchase;
import com.android.billingclient.api.PurchasesUpdatedListener;
import com.qonversion.android.sdk.BuildConfig;
import com.qonversion.android.sdk.Qonversion;
import com.qonversion.android.sdk.QonversionBillingBuilder;
import com.qonversion.unitywrapper.utils.MappersKt;
import com.unity3d.player.UnityPlayer;
import com.qonversion.android.sdk.Qonversion;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.List;

public class QonversionWrapper {
    //unity methods
    private static final String MAKE_PURCHASE = "_makePurchase";


    private static String gameObject;
    private static String TAG = "QonversionWrapper";

    public static void setup(String gameObject_, String projectKey, String userID, boolean autoTracking, boolean useQonversionBilling) {
        //TODO: add logic checks
        gameObject = gameObject_;
        Log.d(TAG, "Initialize starting with projectKey: " + projectKey +
                "; userID: " + userID +
                "; autoTracking" + autoTracking +
                "; useQonversionBilling: " + useQonversionBilling);

        QonversionBillingBuilder billingBuilder = buildBilling();
        Log.d(TAG, "Billing built successfully");
        Qonversion.initialize(UnityPlayer.currentActivity.getApplication(), projectKey, userID, billingBuilder, autoTracking);
        Log.d(TAG, "Qonversion initialized");
    }

    private static QonversionBillingBuilder buildBilling() {
        return new QonversionBillingBuilder()
                .enablePendingPurchases()
                .setChildDirected(BillingClient.ChildDirected.CHILD_DIRECTED)
                .setUnderAgeOfConsent(BillingClient.UnderAgeOfConsent.UNDER_AGE_OF_CONSENT)
                .setListener(new PurchasesUpdatedListener() {
                    @Override
                    public void onPurchasesUpdated(BillingResult billingResult, @Nullable List<Purchase> purchases) {
                        try {
                            JSONObject object = new JSONObject();
                            object.put("responseCode", billingResult.getResponseCode());
                            if (purchases != null) {
                                object.put("purchases", MappersKt.convertToJsonArray(purchases));
                            }
                            sendJSONObject(object, MAKE_PURCHASE);
                        } catch (JSONException e) {
                            logJSONException(e);
                        } catch (Exception e) {
                            Log.wtf(TAG, e);
                        }
                    }
                });
    }

    private static void sendJSONObject(JSONObject object, String method) {
        Log.e(TAG, "Purchases. Result: " + object.toString());
        UnityPlayer.UnitySendMessage(gameObject, method, object.toString());
    }

    private static void logJSONException(JSONException e) {
        Log.e(TAG, "Purchases. " + "JSON Error: " + e.getLocalizedMessage());
    }
}
