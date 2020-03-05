using System.Collections;
using System.Collections.Generic;
using Qonversion.Scripts;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class QonversionUsage : MonoBehaviour
{
    public QonversionPurchases QonversionPurchases;
    private IAPManagerConfigurator _iapManagerConfigurator;

    public GameObject LoadingIAPText;
    public GameObject IAPBtnsContainer;

    public Button TestPurchase;
    public Button TestSubscription;

    void Start()
    {
        LoadingIAPText.SetActive(true);
        IAPBtnsContainer.SetActive(false);

        InitializeBilling();

        TestPurchase.onClick.AddListener(() =>
        {
            _iapManagerConfigurator.PurchaseClick("com.qonversion.sdktestproduct1");
            Debug.Log("PurchaseClicked");
        });

        TestSubscription.onClick.AddListener(() =>
        {
            _iapManagerConfigurator.PurchaseClick("com.qonversion.testsubscription1");
            Debug.Log("SubscriptionClicked");
        });

        QonversionPurchases.Initialize("test3");
        Debug.Log("Qonversion initialized");
    }

    private void InitializeBilling()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
#if UNITY_ANDROID && !UNITY_EDITOR
        builder.AddProduct("com.qonversion.sdktestproduct1", ProductType.Consumable);
        builder.AddProduct("com.qonversion.testsubscription1", ProductType.Subscription);
        //#elif UNITY_IPHONE && !UNITY_EDITOR
#endif
        
        _iapManagerConfigurator = new IAPManagerConfigurator(() =>
        {
            IAPBtnsContainer.SetActive(true);
            LoadingIAPText.SetActive(false);
        }, builder, QonversionPurchases);
    }
}