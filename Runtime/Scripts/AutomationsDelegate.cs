using UnityEngine;

namespace QonversionUnity
{
    public partial class Automations
    {
        /// <summary>
        /// Delegate fires each time an automations event happens
        /// </summary>
        public abstract class AutomationsDelegate : MonoBehaviour
        {
            /// <summary>
            /// Called when Automations screen is shown
            /// </summary>
            /// <param name="screenId">shown screen Id</param>
            public abstract void OnAutomationsScreenShown(string screenId);

            /// <summary>
            /// Called when Automations flow starts executing an action
            /// </summary>
            /// <param name="actionResult">executed action</param>
            public abstract void OnAutomationsActionStarted(ActionResult actionResult);

            /// <summary>
            /// Called when Automations flow fails executing an action
            /// </summary>
            /// <param name="actionResult">executed action</param>
            public abstract void OnAutomationsActionFailed(ActionResult actionResult);

            /// <summary>
            /// Called when Automations flow finishes executing an action
            /// For instance, if the user made a purchase then action.type == QONActionResultTypePurchase
            /// Then you can use the Qonversion.checkPermissions() method to get available permissions
            /// </summary>
            /// <param name="actionResult">executed action</param>
            public abstract void OnAutomationsActionFinished(ActionResult actionResult);

            /// <summary>
            /// Called when Automations flow is finished and the Automations screen is closed
            /// </summary>
            public abstract void OnAutomationsFinished();
        }
    }
}