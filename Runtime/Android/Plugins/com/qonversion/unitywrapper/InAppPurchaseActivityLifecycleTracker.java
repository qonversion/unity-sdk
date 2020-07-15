package com.qonversion.unitywrapper;

import android.app.Activity;
import android.app.Application;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.os.Bundle;
import android.os.IBinder;
import android.util.Log;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.atomic.AtomicBoolean;
import org.json.JSONException;
import org.json.JSONObject;

public class InAppPurchaseActivityLifecycleTracker {

    private static final String TAG = InAppPurchaseActivityLifecycleTracker.class.getCanonicalName();

    private static final String SERVICE_INTERFACE_NAME =
            "com.android.vending.billing.IInAppBillingService$Stub";
    private static final String BILLING_ACTIVITY_NAME =
            "com.android.billingclient.api.ProxyBillingActivity";

    private static final AtomicBoolean isTracking = new AtomicBoolean(false);

    private static Boolean hasBillingService = null;
    private static Boolean hasBiillingActivity = null;
    private static ServiceConnection serviceConnection;
    private static Application.ActivityLifecycleCallbacks callbacks;
    private static Intent intent;
    private static Object inAppBillingObj;

    public static void update() {
        initializeIfNotInitialized();
        if (!hasBillingService) {
            return;
        }
        if (QonversionWrapper.isImplicitPurchaseLoggingEnabled()) {
            startTracking();
        }
    }

    private static void initializeIfNotInitialized() {
        if (hasBillingService != null) {
            return;
        }

        try {
            Class.forName(SERVICE_INTERFACE_NAME);
            hasBillingService = true;
        } catch (ClassNotFoundException ignored) {
            hasBillingService = false;
            return;
        }

        try {
            Class.forName(BILLING_ACTIVITY_NAME);
            hasBiillingActivity = true;
        } catch (ClassNotFoundException ignored) {
            hasBiillingActivity = false;
        }

        InAppPurchaseEventManager.clearSkuDetailsCache();

        intent =
                new Intent("com.android.vending.billing.InAppBillingService.BIND")
                        .setPackage("com.android.vending");

        serviceConnection =
                new ServiceConnection() {
                    @Override
                    public void onServiceConnected(ComponentName name, IBinder service) {

                        inAppBillingObj =
                                InAppPurchaseEventManager.asInterface(QonversionWrapper.getApplicationContext(), service);
                    }

                    @Override
                    public void onServiceDisconnected(ComponentName name) {}
                };

        callbacks =
                new Application.ActivityLifecycleCallbacks() {
                    @Override
                    public void onActivityResumed(Activity activity) {
                        try {
                            QonversionWrapper.getExecutor()
                                    .execute(
                                            new Runnable() {
                                                @Override
                                                public void run() {
                                                    final Context context = QonversionWrapper.getApplicationContext();
                                                    ArrayList<String> purchasesInapp =
                                                            InAppPurchaseEventManager.getPurchasesInapp(context, inAppBillingObj);
                                                    logPurchase(context, purchasesInapp, false);

                                                    ArrayList<String> purchasesSubs =
                                                            InAppPurchaseEventManager.getPurchasesSubs(context, inAppBillingObj);
                                                    logPurchase(context, purchasesSubs, true);
                                                }
                                            });
                        } catch (Exception ep) {
                            /*no op*/
                        }
                    }

                    @Override
                    public void onActivityCreated(Activity activity, Bundle savedInstanceState) {}

                    @Override
                    public void onActivityStarted(Activity activity) {}

                    @Override
                    public void onActivityPaused(Activity activity) {}

                    @Override
                    public void onActivityStopped(Activity activity) {
                        try {
                            if (hasBiillingActivity
                                    && activity.getLocalClassName().equals(BILLING_ACTIVITY_NAME)) {
                                QonversionWrapper.getExecutor()
                                        .execute(
                                                new Runnable() {
                                                    @Override
                                                    public void run() {
                                                        final Context context = QonversionWrapper.getApplicationContext();
                                                        ArrayList<String> purchases =
                                                                InAppPurchaseEventManager.getPurchasesInapp(
                                                                        context, inAppBillingObj);
                                                        if (purchases.isEmpty()) {
                                                            purchases =
                                                                    InAppPurchaseEventManager.getPurchaseHistoryInapp(
                                                                            context, inAppBillingObj);
                                                        }
                                                        logPurchase(context, purchases, false);
                                                    }
                                                });
                            }
                        } catch (Exception ep) {
                            /*no op*/
                        }
                    }

                    @Override
                    public void onActivitySaveInstanceState(Activity activity, Bundle outState) {}

                    @Override
                    public void onActivityDestroyed(Activity activity) {}
                };
    }

    private static void startTracking() {
        if (!isTracking.compareAndSet(false, true)) {
            return;
        }
        Context context = QonversionWrapper.getApplicationContext();
        if (context instanceof Application) {
            Application application = (Application) context;
            application.registerActivityLifecycleCallbacks(callbacks);
            context.bindService(intent, serviceConnection, Context.BIND_AUTO_CREATE);
        }
    }

    private static void logPurchase(
            final Context context, ArrayList<String> purchases, boolean isSubscription) {
        if (purchases.isEmpty()) {
            return;
        }

        final Map<String, String> purchaseMap = new HashMap<>();
        ArrayList<String> skuList = new ArrayList<>();
        for (String purchase : purchases) {
            try {
                JSONObject purchaseJson = new JSONObject(purchase);
                String sku = purchaseJson.getString("productId");
                purchaseMap.put(sku, purchase);

                skuList.add(sku);
            } catch (JSONException e) {
                Log.e(TAG, "Error parsing in-app purchase data.", e);
            }
        }

        final Map<String, String> skuDetailsMap =
                InAppPurchaseEventManager.getSkuDetails(context, skuList, inAppBillingObj, isSubscription);

        for (Map.Entry<String, String> pair : skuDetailsMap.entrySet()) {

            String skuDetailsForProduct = pair.getValue();
            String jsonPurchaseInfo  = purchaseMap.get(pair.getKey());
			
            QonversionWrapper.trackPurchase(skuDetailsForProduct, jsonPurchaseInfo, null);
        }
    }
}
