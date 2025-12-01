using UnityEngine;

namespace QonversionUnity
{
    /// <summary>
    /// Delegate fires each time a NoCodes event happens
    /// </summary>
    public abstract class NoCodesDelegate : MonoBehaviour
    {
        /// <summary>
        /// Called when NoCodes screen is shown
        /// </summary>
        /// <param name="screenId">shown screen Id</param>
        public abstract void OnNoCodesScreenShown(string screenId);

        /// <summary>
        /// Called when NoCodes flow starts executing an action
        /// </summary>
        /// <param name="jsonPayload">executed action as JSON string</param>
        public abstract void OnNoCodesActionStarted(string jsonPayload);

        /// <summary>
        /// Called when NoCodes flow fails executing an action
        /// </summary>
        /// <param name="jsonPayload">executed action as JSON string</param>
        public abstract void OnNoCodesActionFailed(string jsonPayload);

        /// <summary>
        /// Called when NoCodes flow finishes executing an action
        /// For instance, if the user made a purchase then action.type == purchase
        /// Then you can use the <see cref="IQonversion.CheckEntitlements"/> method to get available permissions
        /// </summary>
        /// <param name="jsonPayload">executed action as JSON string</param>
        public abstract void OnNoCodesActionFinished(string jsonPayload);

        /// <summary>
        /// Called when NoCodes flow is finished and the NoCodes screen is closed
        /// </summary>
        /// <param name="jsonPayload">event payload as JSON string</param>
        public abstract void OnNoCodesFinished(string jsonPayload);

        /// <summary>
        /// Called when NoCodes screen loading failed
        /// </summary>
        /// <param name="jsonPayload">error payload as JSON string</param>
        public abstract void OnNoCodesScreenFailedToLoad(string jsonPayload);
    }
}

