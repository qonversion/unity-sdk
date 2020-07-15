using UnityEngine;

namespace QonversionUnity
{
    public class QonversionPurchases : MonoBehaviour
    {
        [Tooltip("Your Qonversion Application Access Key. Get from https://dash.qonversion.io/")]
        [SerializeField]
        protected string m_ApplicationAccessKey;

        [Tooltip("Debug Mode: https://docs.qonversion.io/getting-started/debug-mode")]
        [SerializeField]
        protected bool m_DebugMode;

        private void Start()
        {
            Qonversion.Initialize(m_ApplicationAccessKey, m_DebugMode);
        }
    }
}