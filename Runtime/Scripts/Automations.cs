using UnityEngine;

namespace QonversionUnity
{
    public partial class Automations : MonoBehaviour
    {
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
