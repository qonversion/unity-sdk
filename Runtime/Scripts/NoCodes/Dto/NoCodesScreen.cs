using System.Collections.Generic;
using JetBrains.Annotations;
using QonversionUnity.MiniJSON;

namespace QonversionUnity
{
    /// <summary>
    /// Kind of a No-Codes screen default variable — what it was configured as in the builder.
    /// The set may grow in future backend versions; values this SDK version does not know
    /// are mapped to <see cref="Unknown"/> instead of failing.
    /// </summary>
    public enum NoCodesScreenVariableKind
    {
        /// <summary>A Screen Variable authored in the builder's Variables section.</summary>
        Custom,

        /// <summary>
        /// A product slot: the variable key is the slot name and the value is the default
        /// Qonversion product id assigned to it.
        /// </summary>
        Product,

        /// <summary>
        /// The screen's Default Product configured in the builder: the value is
        /// the Qonversion product id selected by default.
        /// </summary>
        SelectedProduct,

        /// <summary>A kind introduced on the backend after this SDK version was released.</summary>
        Unknown
    }

    /// <summary>
    /// A typed default variable of a No-Codes screen, configured in the builder and delivered
    /// at screen load so it can be read by key. The value keeps its authored type
    /// (bool / string / number) rather than being coerced to a string.
    /// </summary>
    public class NoCodesScreenVariable
    {
        /// <summary>What the variable represents — see <see cref="NoCodesScreenVariableKind"/>.</summary>
        public readonly NoCodesScreenVariableKind Kind;

        /// <summary>
        /// Variable name it is addressed by (`variable.&lt;key&gt;` in the builder for custom
        /// variables, the slot name for product slots). May contain spaces.
        /// </summary>
        public readonly string Key;

        /// <summary>Authored value type: "boolean", "string" or "number".</summary>
        public readonly string Type;

        /// <summary>
        /// The configured default value, preserving its native type (bool, string, long or double).
        /// Null when no default value was authored.
        /// </summary>
        [CanBeNull] public readonly object Value;

        /// <summary>
        /// The value rendered as a plain string regardless of its native type: "true"/"false"
        /// for booleans, the string itself, a number without a trailing ".0" when integral,
        /// or an empty string when no value was authored.
        /// </summary>
        public readonly string StringValue;

        public NoCodesScreenVariable(Dictionary<string, object> dict)
        {
            Kind = ParseKind(dict.GetString("kind"));
            Key = dict.GetString("key");
            Type = dict.GetString("type");
            dict.TryGetValue("value", out Value);
            StringValue = dict.GetString("stringValue");
        }

        private static NoCodesScreenVariableKind ParseKind([CanBeNull] string kind)
        {
            switch (kind)
            {
                case "custom": return NoCodesScreenVariableKind.Custom;
                case "product": return NoCodesScreenVariableKind.Product;
                case "selected_product": return NoCodesScreenVariableKind.SelectedProduct;
                default: return NoCodesScreenVariableKind.Unknown;
            }
        }

        public override string ToString()
        {
            return $"{nameof(Kind)}: {Kind}, " +
                   $"{nameof(Key)}: {Key}, " +
                   $"{nameof(Type)}: {Type}, " +
                   $"{nameof(Value)}: {Value}, " +
                   $"{nameof(StringValue)}: {StringValue}";
        }
    }

    /// <summary>
    /// A loaded No-Codes screen returned from <see cref="INoCodes.LoadScreen"/>.
    ///
    /// Exposes the screen identifiers and the typed default variables configured in the builder —
    /// the screen content stays internal, as rendering remains the SDK's job via
    /// <see cref="INoCodes.ShowScreen"/>.
    /// </summary>
    public class NoCodesScreen
    {
        /// <summary>Identifier of the screen.</summary>
        public readonly string Id;

        /// <summary>The context key of the screen set in the No-Codes builder.</summary>
        public readonly string ContextKey;

        /// <summary>
        /// The Qonversion product id selected by default when the screen opens (the builder's
        /// Default Product), or null when none is configured.
        /// </summary>
        [CanBeNull] public readonly string DefaultSelectedProductId;

        /// <summary>
        /// Typed default variables of the screen configured in the builder: authored custom
        /// variables and product slots. Read them by <see cref="NoCodesScreenVariable.Key"/> (may be empty).
        /// </summary>
        public readonly List<NoCodesScreenVariable> DefaultVariables = new List<NoCodesScreenVariable>();

        public NoCodesScreen(Dictionary<string, object> dict)
        {
            Id = dict.GetString("id");
            ContextKey = dict.GetString("contextKey");
            DefaultSelectedProductId = dict.TryGetValue("defaultSelectedProductId", out object productId)
                ? productId as string
                : null;

            if (dict.TryGetValue("defaultVariables", out object variablesValue) && variablesValue is List<object> rawVariables)
            {
                foreach (object rawVariable in rawVariables)
                {
                    if (rawVariable is Dictionary<string, object> variableDict)
                    {
                        DefaultVariables.Add(new NoCodesScreenVariable(variableDict));
                    }
                }
            }
        }

        /// <summary>
        /// Returns the default variable configured under the given key, or null when the screen
        /// has no variable with that exact (case-sensitive) key.
        ///
        /// Keys are only unique within a kind — a custom variable and a product slot may share
        /// a name — so pass <paramref name="kind"/> to disambiguate; without it the first match
        /// in payload order (custom variables, then product slots, then the selected product)
        /// is returned.
        ///
        /// For the default selected product prefer <see cref="DefaultSelectedProductId"/> — it needs no key.
        /// </summary>
        [CanBeNull]
        public NoCodesScreenVariable DefaultVariable(string key, NoCodesScreenVariableKind? kind = null)
        {
            foreach (NoCodesScreenVariable variable in DefaultVariables)
            {
                if (variable.Key == key && (kind == null || variable.Kind == kind))
                {
                    return variable;
                }
            }

            return null;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, " +
                   $"{nameof(ContextKey)}: {ContextKey}, " +
                   $"{nameof(DefaultSelectedProductId)}: {DefaultSelectedProductId}, " +
                   $"{nameof(DefaultVariables)}: {string.Join("; ", DefaultVariables)}";
        }
    }
}
