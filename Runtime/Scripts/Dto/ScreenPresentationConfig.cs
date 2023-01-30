using System;
using System.Collections.Generic;
using QonversionUnity.MiniJSON;
using UnityEngine;

namespace QonversionUnity
{
    public class ScreenPresentationConfig
    {
        /// Describes how screens will be displayed.
        [Tooltip("For mode details see the enum description.")]
        public readonly ScreenPresentationStyle PresentationStyle;

        /// iOS only. For Android consider using <see cref="ScreenPresentationStyle.NoAnimation"/>.
        /// Describes whether should transaction be animated or not.
        /// Default value is true.
        public readonly bool Animated;

        public ScreenPresentationConfig(ScreenPresentationStyle presentationStyle)
        {
            PresentationStyle = presentationStyle;
            Animated = true;
        }

        public ScreenPresentationConfig(ScreenPresentationStyle presentationStyle, bool animated)
        {
            PresentationStyle = presentationStyle;
            Animated = animated;
        }

        public string ToJson()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            
            string presentationStyleName = Enum.GetName(typeof(ScreenPresentationStyle), PresentationStyle);
            data["presentationStyle"] = presentationStyleName;
            data["animated"] = Animated;

            return data.toJson();
        }
    }
}
