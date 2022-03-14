﻿using System;
using System.Collections.Generic;

namespace QonversionUnity
{
    public class ActionResult
    {
        public readonly ActionResultType Type;
        public readonly Dictionary<string, object> Parameters;
        public readonly QonversionError Error;

        public ActionResult(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("error", out object rawError)) Error = new QonversionError((Dictionary<string, object>)rawError);
            if (dict.TryGetValue("value", out object rawParams)) Parameters = (Dictionary<string, object>)rawParams;
            if (dict.TryGetValue("type", out object ActionType)) Type = FormatActionResultType(ActionType);
        }

        public override string ToString()
        {
            return $"{nameof(Type)}: {Type}, " +
                   $"{nameof(Parameters)}: {Parameters}, " +
                   $"{nameof(Error)}: {Error}";
        }

        private ActionResultType FormatActionResultType(object type) {
            string value = type as string;
            ActionResultType result;
            switch (value)
            {
                case "url":
                    result = ActionResultType.URL;
                    break;
                case "deeplink":
                    result = ActionResultType.Deeplink;
                    break;
                case "navigate":
                    result = ActionResultType.Navigation;
                    break;
                case "purchase":
                    result = ActionResultType.Purchase;
                    break;
                case "restore":
                    result = ActionResultType.Restore;
                    break;
                case "close":
                    result = ActionResultType.Close;
                    break;
                default:
                    result = ActionResultType.Unknown;
                    break;
            }

            return result;
        }
    }
}
