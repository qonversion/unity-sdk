using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleJSON {
    public class ParseError : Exception {
        public readonly int Position;

        public ParseError(string message, int position) : base(message) {
            Position = position;
        }
    }

    public static class JSONDecoder {
        private const char ObjectStart = '{';
        private const char ObjectEnd = '}';
        private const char ObjectPairSeparator = ',';
        private const char ObjectSeparator = ':';
        private const char ArrayStart = '[';
        private const char ArrayEnd = ']';
        private const char ArraySeparator = ',';
        private const char StringStart = '"';
        private const char NullStart = 'n';
        private const char TrueStart = 't';
        private const char FalseStart = 'f';

        public static JObject Decode(string json) {
            var data = Scan(json, 0);
            return data.Result;
        }

        private struct ScannerData {
            public readonly JObject Result;
            public readonly int Index;

            public ScannerData(JObject result, int index) {
                Result = result;
                Index = index;
            }
        }

        private static readonly Dictionary<char, string> EscapeChars =
            new Dictionary<char, string>
                {
                    { '"', "\"" },
                    { '\\', "\\" },
                    { 'b', "\b" },
                    { 'f', "\f" },
                    { 'n', "\n" },
                    { 'r', "\r" },
                    { 't', "\t" },
                };

        private static ScannerData Scan(string json, int index) {
            index = SkipWhitespace(json, index);
            var nextChar = json[index];

            switch (nextChar) {
            case ObjectStart:
                return ScanObject(json, index);
            case ArrayStart:
                return ScanArray(json, index);
            case StringStart:
                return ScanString(json, index + 1);
            case TrueStart:
                return ScanTrue(json, index);
            case FalseStart:
                return ScanFalse(json, index);
            case NullStart:
                return ScanNull(json, index);
            default:
                if (IsNumberStart(nextChar)) {
                    return ScanNumber(json, index);
                }
                throw new ParseError("Unexpected token " + nextChar, index);
            }
        }

        private static ScannerData ScanString(string json, int index) {
            string s;
            index = ScanBareString(json, index, out s);

            return new ScannerData(JObject.CreateString(s), index + 1);
        }

        private static ScannerData ScanTrue(string json, int index) {
            return new ScannerData(JObject.CreateBoolean(true), ExpectConstant(json, index, "true"));
        }

        private static ScannerData ScanFalse(string json, int index) {
            return new ScannerData(JObject.CreateBoolean(false), ExpectConstant(json, index, "false"));
        }

        private static ScannerData ScanNull(string json, int index) {
            return new ScannerData(JObject.CreateNull(), ExpectConstant(json, index, "null"));
        }

        private static ScannerData ScanNumber(string json, int index) {
            var negative = false;
            var fractional = false;
            var negativeExponent = false;

            if (json[index] == '-') {
                negative = true;
                ++index;
            }

            ulong integerPart = 0;
            if (json[index] != '0') {
                while (json.Length > index && char.IsNumber(json[index])) {
                    integerPart = (integerPart * 10) + (ulong)(json[index] - '0');
                    ++index;
                }
            } else {
                ++index;
            }

            ulong fractionalPart = 0;
            int fractionalPartLength = 0;
            if (json.Length > index && json[index] == '.') {
                fractional = true;

                ++index;
                while (json.Length > index && char.IsNumber(json[index])) {
                    fractionalPart = (fractionalPart * 10) + (ulong)(json[index] - '0');
                    ++index;
                    ++fractionalPartLength;
                }
            }

            ulong exponent = 0;
            if (json.Length > index && (json[index] == 'e' || json[index] == 'E')) {
                fractional = true;
                ++index;

                if (json[index] == '-') {
                    negativeExponent = true;
                    ++index;
                } else if (json[index] == '+') {
                    ++index;
                }

                while (json.Length > index && char.IsNumber(json[index])) {
                    exponent = (exponent * 10) + (ulong)(json[index] - '0');
                    ++index;
                }
            }

            return new ScannerData(JObject.CreateNumber(negative, fractional, negativeExponent, integerPart, fractionalPart, fractionalPartLength, exponent), index);
        }

        private static ScannerData ScanArray(string json, int index) {
            var list = new List<JObject>();

            var nextTokenIndex = SkipWhitespace(json, index + 1);
            if (json[nextTokenIndex] == ArrayEnd) return new ScannerData(JObject.CreateArray(list), nextTokenIndex + 1);

            while (json[index] != ArrayEnd) {
                ++index;
                var result = Scan(json, index);
                index = SkipWhitespace(json, result.Index);
                if (json[index] != ArraySeparator && json[index] != ArrayEnd) {
                    throw new ParseError("Expecting array separator (,) or array end (])", index);
                }
                list.Add(result.Result);
            }
            return new ScannerData(JObject.CreateArray(list), index + 1);
        }

        private static ScannerData ScanObject(string json, int index) {
            var dict = new Dictionary<string, JObject>();

            var nextTokenIndex = SkipWhitespace(json, index + 1);
            if (json[nextTokenIndex] == ObjectEnd) return new ScannerData(JObject.CreateObject(dict), nextTokenIndex + 1);

            while (json[index] != ObjectEnd) {
                index = SkipWhitespace(json, index + 1);
                if (json[index] != '"') {
                    throw new ParseError("Object keys must be strings", index);
                }
                string key;
                index = ScanBareString(json, index + 1, out key) + 1;
                index = SkipWhitespace(json, index);
                if (json[index] != ObjectSeparator) {
                    throw new ParseError("Expecting object separator (:)", index);
                }
                ++index;
                var valueResult = Scan(json, index);
                index = SkipWhitespace(json, valueResult.Index);
                if (json[index] != ObjectEnd && json[index] != ObjectPairSeparator) {
                    throw new ParseError("Expecting object pair separator (,) or object end (})", index);
                }
                dict[key] = valueResult.Result;
            }
            return new ScannerData(JObject.CreateObject(dict), index + 1);
        }

        private static int SkipWhitespace(string json, int index) {
            while (char.IsWhiteSpace(json[index])) {
                ++index;
            }
            return index;
        }

        private static int ExpectConstant(string json, int index, string expected) {
            if (json.Substring(index, expected.Length) != expected) {
                throw new ParseError(string.Format("Expected '{0}' got '{1}'",
                                                   expected,
                                                   json.Substring(index, expected.Length)),
                                     index);
            }
            return index + expected.Length;
        }

        private static bool IsNumberStart(char b) {
            return b == '-' || (b >= '0' && b <= '9');
        }

        private static int ScanBareString(string json, int index, out string result) {
            // First determine length
            var lengthIndex = index;
            var foundEscape = false;
            while (json[lengthIndex] != '"') {
                if (json[lengthIndex] == '\\') {
                    foundEscape = true;
                    ++lengthIndex;
                    if (EscapeChars.ContainsKey(json[lengthIndex]) || json[lengthIndex] == 'u') {
                        ++lengthIndex;
                    } else if (json[lengthIndex] == 'u') {
                        lengthIndex += 5;
                    }
                } else {
                    ++lengthIndex;
                }
            }

            if (!foundEscape) {
                result = json.Substring(index, lengthIndex - index);
                return lengthIndex;
            }

            var strBuilder = new StringBuilder(lengthIndex - index);

            var lastIndex = index;
            while (json[index] != '"') {
                if (json[index] == '\\') {
                    if (index > lastIndex) {
                        strBuilder.Append(json, lastIndex, index - lastIndex);
                    }
                    ++index;
                    if (EscapeChars.ContainsKey(json[index])) {
                        strBuilder.Append(EscapeChars[json[index]]);
                        ++index;
                    } else if (json[index] == 'u') {
                        ++index;
                        var unicodeSequence = Convert.ToInt32(json.Substring(index, 4), 16);
                        strBuilder.Append((char)unicodeSequence);
                        index += 4;
                    }
                    lastIndex = index;
                } else {
                    ++index;
                }
            }

            if (lastIndex != index) {
                strBuilder.Append(json, lastIndex, index - lastIndex);
            }

            result = strBuilder.ToString();
            return index;
        }
    }
}