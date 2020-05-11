using System;
using System.Globalization;
using System.IO;

namespace SimpleJSON {
    public class JSONStreamEncoder {
        private struct EncoderContext {
            public bool IsObject;
            public bool IsEmpty;

            public EncoderContext(bool isObject, bool isEmpty) {
                IsObject = isObject;
                IsEmpty = isEmpty;
            }
        }

        private TextWriter _writer;
        private EncoderContext[] _contextStack;
        private int _contextStackPointer = -1;
        private bool _newlineInserted = false;

        public JSONStreamEncoder(TextWriter writer, int expectedNesting = 20) {
            _writer = writer;
            _contextStack = new EncoderContext[expectedNesting];
        }

        public void BeginArray() {
            WriteSeparator();
            PushContext(new EncoderContext(false, true));
            _writer.Write('[');
        }

        public void EndArray() {
            if (_contextStackPointer == -1) {
                throw new InvalidOperationException("EndArray called without BeginArray");
            } else if (_contextStack[_contextStackPointer].IsObject) {
                throw new InvalidOperationException("EndArray called after BeginObject");
            }

            PopContext();
            WriteNewline();
            _writer.Write(']');
        }

        public void BeginObject() {
            WriteSeparator();
            PushContext(new EncoderContext(true, true));
            _writer.Write('{');
        }

        public void EndObject() {
            if (_contextStackPointer == -1) {
                throw new InvalidOperationException("EndObject called without BeginObject");
            } else if (!_contextStack[_contextStackPointer].IsObject) {
                throw new InvalidOperationException("EndObject called after BeginArray");
            }

            PopContext();
            WriteNewline();
            _writer.Write('}');
        }

        public void WriteString(string str) {
            WriteSeparator();
            WriteBareString(str);
        }

        public void WriteKey(string str) {
            if (_contextStackPointer == -1) {
                throw new InvalidOperationException("WriteKey called without BeginObject");
            } else if (!_contextStack[_contextStackPointer].IsObject) {
                throw new InvalidOperationException("WriteKey called after BeginArray");
            }

            WriteSeparator();
            WriteBareString(str);
            _writer.Write(':');

            _contextStack[_contextStackPointer].IsEmpty = true;
        }

        public void WriteNumber(long l) {
            WriteSeparator();
            _writer.Write(l);
        }

        public void WriteNumber(ulong l) {
            WriteSeparator();
            _writer.Write(l);
        }

        public void WriteNumber(double d) {
            WriteSeparator();
            WriteFractionalNumber(d, 0.00000000000000001);
        }

        public void WriteNumber(float f) {
            WriteSeparator();
            WriteFractionalNumber(f, 0.000000001);
        }

        public void WriteNull() {
            WriteSeparator();
            _writer.Write("null");
        }

        public void WriteBool(bool b) {
            WriteSeparator();
            _writer.Write(b ? "true" : "false");
        }

        public void WriteJObject(JObject obj) {
            switch (obj.Kind) {
            case JObjectKind.Array:
                BeginArray();
                foreach (var elem in obj.ArrayValue) {
                    WriteJObject(elem);
                }
                EndArray();
                break;
            case JObjectKind.Boolean:
                WriteBool(obj.BooleanValue);
                break;
            case JObjectKind.Null:
                WriteNull();
                break;
            case JObjectKind.Number:
                if (obj.IsFractional) {
                    WriteNumber(obj.DoubleValue);
                } else if (obj.IsNegative) {
                    WriteNumber(obj.LongValue);
                } else {
                    WriteNumber(obj.ULongValue);
                }
                break;
            case JObjectKind.Object:
                BeginObject();
                foreach (var pair in obj.ObjectValue) {
                    WriteKey(pair.Key);
                    WriteJObject(pair.Value);
                }
                EndObject();
                break;
            case JObjectKind.String:
                WriteString(obj.StringValue);
                break;
            }
        }

        public void InsertNewline() {
            _newlineInserted = true;
        }

        private void WriteBareString(string str) {
            _writer.Write('"');

            int len = str.Length;
            int lastIndex = 0;
            int i = 0;

            for (; i < len; ++i) {
                char c = str[i];

                if (c > 0x80 || c < 0x20 || c == '"' || c == '\\') {
                    if (i > lastIndex) {
                        _writer.Write(str.Substring(lastIndex, i - lastIndex));
                    }
                    if (JSONEncoder.EscapeChars.ContainsKey(c)) {
                        _writer.Write(JSONEncoder.EscapeChars[c]);
                    } else {
                        _writer.Write("\\u" + Convert.ToString(c, 16)
                                                .ToUpper(CultureInfo.InvariantCulture)
                                                .PadLeft(4, '0'));
                    }
                    lastIndex = i + 1;
                }
            }

            if (lastIndex == 0 && i > lastIndex) {
                _writer.Write(str);
            } else if (i > lastIndex) {
                _writer.Write(str.Substring(lastIndex, i - lastIndex));
            }

            _writer.Write('"');
        }

        private void WriteFractionalNumber(double d, double tolerance) {
            if (d < 0) {
                _writer.Write('-');
                d = -d;
            } else if (d == 0) {
                _writer.Write('0');
                return;
            }

            var magnitude = (int)Math.Log10(d);

            if (magnitude < 0) {
                _writer.Write("0.");
                for (int i = 0; i > magnitude + 1; --i) {
                    _writer.Write('0');
                }
            }

            while (d > tolerance || magnitude >= 0) {
                var weight = Math.Pow(10, magnitude);
                var digit = (int)Math.Floor(d / weight);
                d -= digit * weight;
                _writer.Write((char)('0' + digit));
                if (magnitude == 0 && (d > tolerance || magnitude > 0)) {
                    _writer.Write('.');
                }
                --magnitude;
            }
        }

        private void WriteSeparator() {
            if (_contextStackPointer == -1) return;

            if (!_contextStack[_contextStackPointer].IsEmpty) {
                _writer.Write(',');
            }

            _contextStack[_contextStackPointer].IsEmpty = false;

            WriteNewline();
        }

        private void WriteNewline() {
            if (_newlineInserted) {
                _writer.Write('\n');
                for (var i = 0; i < _contextStackPointer + 1; ++i) {
                    _writer.Write(' ');
                }

                _newlineInserted = false;
            }
        }

        private void PushContext(EncoderContext ctx) {
            if (_contextStackPointer + 1 == _contextStack.Length) {
                throw new StackOverflowException("Too much nesting for context stack, increase expected nesting when creating the encoder");
            }

            _contextStack[++_contextStackPointer] = ctx;
        }

        private void PopContext() {
            if (_contextStackPointer == -1) {
                throw new InvalidOperationException("Stack underflow");
            }

            --_contextStackPointer;
        }
    }
}
