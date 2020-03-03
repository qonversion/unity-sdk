using System;
using System.Collections.Generic;
using System.Linq;
using Qonversion.Scripts;
using Qonversion.Scripts.Utils;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManagerConfigurator : IStoreListener
{
    private IStoreController controller;
    private IExtensionProvider extensions;

    private Action _onInitialized;

    private QonversionPurchases _qonversionPurchases;
    
    public IAPManagerConfigurator(Action onInitialized, ConfigurationBuilder configurationBuilder, QonversionPurchases qonversionPurchases)
    {
        _qonversionPurchases = qonversionPurchases;
        _onInitialized = onInitialized;
        UnityPurchasing.Initialize(this, configurationBuilder);
    }

    /// <summary>
    /// Called when Unity IAP is ready to make purchases.
    /// </summary>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
        this.extensions = extensions;
        _onInitialized();
    }

    public void PurchaseClick(string productId) {
        if (controller == null)
        {
            Debug.LogWarning("UnityPurchasing isn't initialized");
            return;
        }
        
        controller.InitiatePurchase(productId);
    }
    
    /// <summary>
    /// Called when Unity IAP encounters an unrecoverable initialization error.
    ///
    /// Note that this will not be called if Internet is unavailable; Unity IAP
    /// will attempt initialization until it becomes available.
    /// </summary>
    public void OnInitializeFailed(InitializationFailureReason error)
    {
    }

    /// <summary>
    /// Called when a purchase completes.
    ///
    /// May be called at any time after OnInitialized().
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        var skuDetails = extensions.GetExtension<IGooglePlayStoreExtensions>().GetProductJSONDictionary();
        Debug.Log($"IAPReceipt: {e.purchasedProduct.receipt}");
        Debug.Log($"SkuDetails: {string.Join(";", skuDetails.Select(x => x.Key + "=" + x.Value).ToArray())}");

        Debug.Log($"e.purchasedProduct.definition.storeSpecificId: {e.purchasedProduct.definition.storeSpecificId}");
        var skuDetailsForProduct = skuDetails[e.purchasedProduct.definition.storeSpecificId];

        var payload = JSON.Parse(e.purchasedProduct.receipt)["Payload"];
        var parsedReceiptPayload = JSON.Parse(payload);
        Debug.Log($"Payload: {payload}");
        var jsonPurchaseInfo = parsedReceiptPayload["json"];
        var signature = parsedReceiptPayload["signature"];
        
        _qonversionPurchases.TrackPurchase(skuDetailsForProduct, jsonPurchaseInfo,signature);
        return PurchaseProcessingResult.Complete;
    }

    /// <summary>
    /// Called when a purchase fails.
    /// </summary>
    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        Debug.Log($"OnPurchaseFailed({i.definition.id}, {i.definition.storeSpecificId}): {p}");
    }
}