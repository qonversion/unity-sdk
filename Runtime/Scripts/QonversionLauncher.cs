using UnityEngine;

namespace QonversionUnity
{
    public class QonversionLauncher : MonoBehaviour
    {
        [Tooltip("Your Qonversion Application Access Key. Get from https://dash.qonversion.io/")]
        [SerializeField]
        protected string m_ApplicationAccessKey;

        [Tooltip("Debug Mode: https://documentation.qonversion.io/docs/debug-mode")]
        [SerializeField]
        protected bool m_DebugMode;

        [Tooltip("Observer Mode: https://documentation.qonversion.io/docs/observer-mode")]
        [SerializeField]
        protected bool m_ObserverMode;

        private void Start()
        {
            if (m_DebugMode)
            {
                Qonversion.SetDebugMode();
            }

            Qonversion.Launch(m_ApplicationAccessKey, m_ObserverMode);
        }
    }
}