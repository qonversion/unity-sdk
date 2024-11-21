using System.Collections.Generic;

namespace QonversionUnity
{
    public class SKPaymentDiscount {
        public readonly string Identifier;
        public readonly string KeyIdentifier;
        public readonly string Nonce;
        public readonly string Signature;
        public readonly long Timestamp;

        public SKPaymentDiscount(Dictionary<string, object> dict) {
            if (dict.TryGetValue("identifier", out object value)) Identifier = value as string;
            if (dict.TryGetValue("keyIdentifier", out value)) KeyIdentifier = value as string;
            if (dict.TryGetValue("nonce", out value)) Nonce = value as string;
            if (dict.TryGetValue("signature", out value)) Signature = value as string;
            if (dict.TryGetValue("timestamp", out object timestamp)) Timestamp = (long)timestamp;
        }

        public override string ToString() {
            return $"{nameof(Identifier)}: {Identifier}, " +
                   $"{nameof(KeyIdentifier)}: {KeyIdentifier}, " +
                   $"{nameof(Nonce)}: {Nonce}, " +
                   $"{nameof(Signature)}: {Signature}, " +
                   $"{nameof(Timestamp)}: {Timestamp}";
        }
    }
}