using System;
using System.Collections.Generic;

namespace QonversionUnity
{
    /// <summary>
    /// Base class for all NoCodes events.
    /// </summary>
    public class NoCodesEvent
    {
        /// <summary>
        /// Type of the event.
        /// </summary>
        public readonly NoCodesEventType Type;

        /// <summary>
        /// Event payload data.
        /// </summary>
        public readonly Dictionary<string, object> Payload;

        /// <summary>
        /// Creates a new instance of NoCodesEvent from dictionary.
        /// </summary>
        /// <param name="dict">Dictionary containing event data.</param>
        public NoCodesEvent(Dictionary<string, object> dict)
        {
            Payload = dict ?? new Dictionary<string, object>();
            
            if (dict != null && dict.TryGetValue("type", out object eventType))
            {
                Type = FormatNoCodesEventType(eventType);
            }
            else
            {
                Type = NoCodesEventType.Unknown;
            }
        }

        /// <summary>
        /// Creates a new instance of NoCodesEvent with specified type and payload.
        /// </summary>
        /// <param name="type">Event type.</param>
        /// <param name="payload">Event payload data.</param>
        public NoCodesEvent(NoCodesEventType type, Dictionary<string, object> payload = null)
        {
            Type = type;
            Payload = payload ?? new Dictionary<string, object>();
        }

        public override string ToString()
        {
            return $"{nameof(Type)}: {Type}, " +
                   $"{nameof(Payload)}: {Payload.Count} items";
        }

        private NoCodesEventType FormatNoCodesEventType(object type)
        {
            string value = type as string;
            NoCodesEventType result;
            
            switch (value)
            {
                case "nocodes_screen_shown":
                case "screen_shown":
                    result = NoCodesEventType.ScreenShown;
                    break;
                case "nocodes_finished":
                case "finished":
                    result = NoCodesEventType.Finished;
                    break;
                case "nocodes_action_started":
                case "action_started":
                    result = NoCodesEventType.ActionStarted;
                    break;
                case "nocodes_action_failed":
                case "action_failed":
                    result = NoCodesEventType.ActionFailed;
                    break;
                case "nocodes_action_finished":
                case "action_finished":
                    result = NoCodesEventType.ActionFinished;
                    break;
                case "nocodes_screen_failed_to_load":
                case "screen_failed_to_load":
                    result = NoCodesEventType.ScreenFailedToLoad;
                    break;
                default:
                    result = NoCodesEventType.Unknown;
                    break;
            }

            return result;
        }
    }
}
