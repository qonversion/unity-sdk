using System.Collections.Generic;
using NUnit.Framework;

namespace QonversionUnity.Tests
{
    [TestFixture]
    public class MapperTests
    {
        [Test]
        public void PurchaseResultFromJson_ValidJson_ReturnsPurchaseResult()
        {
            const string json = "{" +
                "\"status\":\"Success\"," +
                "\"entitlements\":{" +
                    "\"premium\":{\"id\":\"premium\",\"isActive\":true}" +
                "}," +
                "\"isFallbackGenerated\":false," +
                "\"source\":\"Api\"," +
                "\"storeTransaction\":{" +
                    "\"transactionId\":\"tx_123\"," +
                    "\"originalTransactionId\":\"orig_tx_123\"," +
                    "\"productId\":\"com.test.product\"," +
                    "\"quantity\":1" +
                "}" +
            "}";

            var result = Mapper.PurchaseResultFromJson(json);

            Assert.IsNotNull(result);
            Assert.AreEqual(PurchaseResultStatus.Success, result.Status);
            Assert.IsNotNull(result.Entitlements);
            Assert.IsTrue(result.Entitlements.ContainsKey("premium"));
            Assert.IsFalse(result.IsFallbackGenerated);
            Assert.AreEqual(PurchaseResultSource.Api, result.Source);
            Assert.IsNotNull(result.StoreTransaction);
            Assert.AreEqual("tx_123", result.StoreTransaction.TransactionId);
            Assert.AreEqual("com.test.product", result.StoreTransaction.ProductId);
        }

        [Test]
        public void PurchaseResultFromJson_InvalidJson_ReturnsNull()
        {
            const string json = "invalid json";

            var result = Mapper.PurchaseResultFromJson(json);

            Assert.IsNull(result);
        }

        [Test]
        public void PurchaseResultFromJson_PendingStatus_ReturnsPendingResult()
        {
            const string json = "{" +
                "\"status\":\"Pending\"," +
                "\"isFallbackGenerated\":false," +
                "\"source\":\"Local\"" +
            "}";

            var result = Mapper.PurchaseResultFromJson(json);

            Assert.IsNotNull(result);
            Assert.AreEqual(PurchaseResultStatus.Pending, result.Status);
            Assert.IsTrue(result.IsPending);
            Assert.IsNull(result.Entitlements);
            Assert.IsNull(result.StoreTransaction);
            Assert.AreEqual(PurchaseResultSource.Local, result.Source);
        }

        [Test]
        public void PurchaseResultFromJson_WithError_ReturnsResultWithError()
        {
            const string json = "{" +
                "\"status\":\"Error\"," +
                "\"error\":{" +
                    "\"code\":\"PurchaseInvalid\"," +
                    "\"description\":\"Purchase failed\"" +
                "}," +
                "\"isFallbackGenerated\":false," +
                "\"source\":\"Api\"" +
            "}";

            var result = Mapper.PurchaseResultFromJson(json);

            Assert.IsNotNull(result);
            Assert.AreEqual(PurchaseResultStatus.Error, result.Status);
            Assert.IsTrue(result.IsError);
            Assert.IsNotNull(result.Error);
        }

        [Test]
        public void PurchaseResultFromJson_DeferredPurchaseScenario_ParsesCorrectly()
        {
            // Simulates the JSON a deferred purchase completion sends from the Sandwich SDK
            const string json = "{" +
                "\"status\":\"Success\"," +
                "\"entitlements\":{" +
                    "\"premium\":{\"id\":\"premium\",\"isActive\":true}" +
                "}," +
                "\"isFallbackGenerated\":false," +
                "\"source\":\"Api\"," +
                "\"storeTransaction\":{" +
                    "\"transactionId\":\"deferred_tx_456\"," +
                    "\"originalTransactionId\":\"deferred_orig_456\"," +
                    "\"productId\":\"com.test.annual\"," +
                    "\"quantity\":1" +
                "}" +
            "}";

            var result = Mapper.PurchaseResultFromJson(json);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.StoreTransaction);
            Assert.AreEqual("deferred_tx_456", result.StoreTransaction.TransactionId);
            Assert.AreEqual("com.test.annual", result.StoreTransaction.ProductId);
        }
    }
}
