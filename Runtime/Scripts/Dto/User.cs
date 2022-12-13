using System.Collections.Generic;
using JetBrains.Annotations;

namespace QonversionUnity
{
    public class User
    {
        public readonly string QonversionId;
        [CanBeNull] public readonly string IdentityId;

        public User(string qonversionId, [CanBeNull] string identityId)
        {
            QonversionId = qonversionId;
            IdentityId = identityId;
        }

        public User(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("qonversionId", out var qonversionId) && qonversionId != null)
            {
                QonversionId = qonversionId as string;
            }

            if (dict.TryGetValue("identityId", out var identityId) && identityId != null)
            {
                IdentityId = identityId as string;
            }
        }

        public override string ToString()
        {
            return $"{nameof(QonversionId)}: {QonversionId}, " +
                   $"{nameof(IdentityId)}: {IdentityId}";
        }
    }
}
