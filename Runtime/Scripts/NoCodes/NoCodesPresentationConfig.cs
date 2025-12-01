using System.Collections.Generic;
using QonversionUnity.MiniJSON;

namespace QonversionUnity
{
    /// <summary>
    /// Presentation style for NoCodes screens.
    /// </summary>
    public enum NoCodesPresentationStyle
    {
        /// <summary>
        /// Push presentation style (iOS only).
        /// </summary>
        Push,

        /// <summary>
        /// Full screen presentation style.
        /// </summary>
        FullScreen,

        /// <summary>
        /// Popover presentation style (iOS only).
        /// </summary>
        Popover
    }

    /// <summary>
    /// Configuration for NoCodes screen presentation.
    /// </summary>
    public class NoCodesPresentationConfig
    {
        /// <summary>
        /// Whether the presentation should be animated.
        /// </summary>
        public bool Animated { get; }

        /// <summary>
        /// Presentation style for the screen.
        /// </summary>
        public NoCodesPresentationStyle PresentationStyle { get; }

        /// <summary>
        /// Creates a new instance of NoCodesPresentationConfig.
        /// </summary>
        /// <param name="animated">Whether the presentation should be animated. Default is true.</param>
        /// <param name="presentationStyle">Presentation style. Default is FullScreen.</param>
        public NoCodesPresentationConfig(
            bool animated = true,
            NoCodesPresentationStyle presentationStyle = NoCodesPresentationStyle.FullScreen)
        {
            Animated = animated;
            PresentationStyle = presentationStyle;
        }

        /// <summary>
        /// Converts the configuration to a dictionary for native code.
        /// </summary>
        /// <returns>Dictionary representation of the configuration.</returns>
        public Dictionary<string, object> ToDictionary()
        {
            string styleString = PresentationStyle switch
            {
                NoCodesPresentationStyle.Push => "Push",
                NoCodesPresentationStyle.FullScreen => "FullScreen",
                NoCodesPresentationStyle.Popover => "Popover",
                _ => "FullScreen"
            };

            return new Dictionary<string, object>
            {
                { "animated", Animated },
                { "presentationStyle", styleString }
            };
        }
    }
}




