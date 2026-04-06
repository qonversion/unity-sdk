using System.Collections.Generic;
using NUnit.Framework;

namespace QonversionUnity.Tests
{
    [TestFixture]
    public class DeferredPurchasesListenerTests
    {
        [Test]
        public void OnDeferredPurchaseCompleted_DelegateType_Exists()
        {
            // Verify the delegate type exists and has the correct signature
            Qonversion.OnDeferredPurchaseCompleted handler = (PurchaseResult result) => { };
            Assert.IsNotNull(handler);
        }

        [Test]
        public void OnDeferredPurchaseCompleted_DelegateInvocation_ReceivesPurchaseResult()
        {
            PurchaseResult receivedResult = null;
            Qonversion.OnDeferredPurchaseCompleted handler = (PurchaseResult result) =>
            {
                receivedResult = result;
            };

            var json = "{" +
                "\"status\":\"Success\"," +
                "\"entitlements\":{}," +
                "\"isFallbackGenerated\":false," +
                "\"source\":\"Api\"," +
                "\"storeTransaction\":{" +
                    "\"transactionId\":\"deferred_123\"," +
                    "\"productId\":\"com.test.product\"" +
                "}" +
            "}";

            var purchaseResult = Mapper.PurchaseResultFromJson(json);
            handler.Invoke(purchaseResult);

            Assert.IsNotNull(receivedResult);
            Assert.AreEqual(PurchaseResultStatus.Success, receivedResult.Status);
            Assert.AreEqual("deferred_123", receivedResult.StoreTransaction.TransactionId);
        }

        [Test]
        public void OnDeferredPurchaseCompleted_MultipleSubscribers_AllReceiveEvent()
        {
            int callCount = 0;
            Qonversion.OnDeferredPurchaseCompleted handler1 = (PurchaseResult result) => { callCount++; };
            Qonversion.OnDeferredPurchaseCompleted handler2 = (PurchaseResult result) => { callCount++; };

            var combined = handler1 + handler2;

            var json = "{\"status\":\"Success\",\"isFallbackGenerated\":false,\"source\":\"Api\"}";
            var purchaseResult = Mapper.PurchaseResultFromJson(json);
            combined.Invoke(purchaseResult);

            Assert.AreEqual(2, callCount);
        }

        [Test]
        public void OnUpdatedEntitlementsReceived_DelegateType_StillExists()
        {
            // Verify the existing delegate still works (backward compatibility)
            Qonversion.OnUpdatedEntitlementsReceived handler = (Dictionary<string, Entitlement> entitlements) => { };
            Assert.IsNotNull(handler);
        }

        [Test]
        public void BothDelegateTypes_CanCoexist()
        {
            // Verify both event delegate types can be instantiated simultaneously
            bool entitlementsHandled = false;
            bool deferredPurchaseHandled = false;

            Qonversion.OnUpdatedEntitlementsReceived entitlementsHandler =
                (Dictionary<string, Entitlement> entitlements) => { entitlementsHandled = true; };

            Qonversion.OnDeferredPurchaseCompleted deferredHandler =
                (PurchaseResult result) => { deferredPurchaseHandled = true; };

            entitlementsHandler.Invoke(new Dictionary<string, Entitlement>());

            var json = "{\"status\":\"Success\",\"isFallbackGenerated\":false,\"source\":\"Api\"}";
            deferredHandler.Invoke(Mapper.PurchaseResultFromJson(json));

            Assert.IsTrue(entitlementsHandled);
            Assert.IsTrue(deferredPurchaseHandled);
        }
    }
}
