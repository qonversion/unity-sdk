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
#if UNITY_ANDROID && !UNITY_EDITOR
        _iapManagerConfigurator.PurchaseClick("com.qonversion.sdktestproduct1");
#elif UNITY_IPHONE && !UNITY_EDITOR
        _iapManagerConfigurator.PurchaseClick("q.test.coach.inapp.lifetimeaccess.19");
#endif
            Debug.Log("PurchaseClicked");
        });

        TestSubscription.onClick.AddListener(() =>
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        _iapManagerConfigurator.PurchaseClick("com.qonversion.testsubscription1");
#elif UNITY_IPHONE && !UNITY_EDITOR
        _iapManagerConfigurator.PurchaseClick("coach.direct_subs.weekly.3.feb.19");
#endif
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
#elif UNITY_IPHONE && !UNITY_EDITOR
        builder.AddProduct("q.test.coach.inapp.lifetimeaccess.19", ProductType.Consumable);
        builder.AddProduct("coach.direct_subs.weekly.3.feb.19", ProductType.Subscription);
#endif
        
        _iapManagerConfigurator = new IAPManagerConfigurator(() =>
        {
            IAPBtnsContainer.SetActive(true);
            LoadingIAPText.SetActive(false);
        }, builder, QonversionPurchases);
    }
}