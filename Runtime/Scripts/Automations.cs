using QonversionUnity.Automations;
using UnityEngine;

namespace QonversionUnity
{
    public partial class Automations : MonoBehaviour
    {
        private const string GameObjectName = "AutomationsRuntimeGameObject";
        private static IQonversionWrapper _Instance;
        private static AutomationsDelegate automationsDelegate;

        private static IQonversionWrapper getFinalInstance()
        {
            if (_Instance == null)
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.Android:
                        _Instance = new QonversionWrapperAndroid();
                        break;
                    case RuntimePlatform.IPhonePlayer:
                        _Instance = new QonversionWrapperIOS();
                        break;
                    default:
                        _Instance = new QonversionWrapperNoop();
                        break;
                }
            }

            GameObject go = new GameObject(GameObjectName);
            go.AddComponent<Automations>();
            DontDestroyOnLoad(go);

            return _Instance;
        }

        /// <summary>
        /// The Automations delegate is responsible for handling in-app screens and actions when push notification is received.
        /// Make sure the method is called before Qonversion.handleNotification.
        /// </summary>
        /// <param name="automationsDelegate">The delegate to handle automations events</param>
        public static void SetDelegate(AutomationsDelegate automationsDelegate)
        {
            Qonversion.SetAutomationsDelegate(automationsDelegate);
        }
    }
}
