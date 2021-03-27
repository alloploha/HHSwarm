using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Diagnostics
{
    public static class TraceSourceExtensions
    {
        [Conditional("DEBUG")]
        public static void Dump<T>(this TraceSource @this, TraceEventType eventType, string message, T data)
        {
            List<object> traceData = new List<object>();
            traceData.Add(message);

            Type type = typeof(T);

            if (data != null)
            {
                if (type.IsClass)
                {
                    if (type.IsArray)
                    {
                        traceData.Add($"Length: {(data as Array).Length}");
                        traceData.Add(Format(data));
                    }
                    else if (type is IEnumerable)
                    {
                        traceData.Add($"Count: {(data as IEnumerable).Cast<object>().Count()}");
                        traceData.Add(Format(data));
                    }
                    else
                    {
                        traceData.Add(String.Join(".", GetDeclaringTypesChain(type).Select(c => c.Name)));
                        traceData.Add(Format(data));
                        /*
                        foreach (var m in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField))
                        {
                            traceData.Add($"{m.Name}='{Format(m.GetValue(data))}'");
                        }

                        foreach (var m in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty))
                        {
                            traceData.Add($"{m.Name}='{Format(m.GetValue(data))}'");
                        }*/
                    }
                }
                else
                    traceData.Add(Format(data));
            }
            else
                traceData.Add("null");

            @this.TraceData(eventType, 0, traceData.ToArray());
        }

        private static List<Type> GetDeclaringTypesChain(Type @this)
        {
            if (!@this.IsNested)
                return new List<Type>(new Type[] { @this });
            else
            {
                var chain = GetDeclaringTypesChain(@this.DeclaringType);
                chain.Add(@this);
                return chain;
            }
        }

        private static object Format(object value)
        {
            if(value == null)
            {
                return "null";
            }
            else if (value is string)
            {
                return value;
            }
            else if (value is byte[])
            {
                return BitConverter.ToString((byte[])value);
            }
            else if (value is IEnumerable)
            {
                return "[" + String.Join(",", (value as IEnumerable).Cast<object>().Select(v => Format(v))) + "]";
            }
            else if (value.GetType().IsClass)
            {
                Type type = value.GetType();
                object data = value;
                List<string> result = new List<string>();

                foreach (var m in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField))
                {
                    result.Add($"{m.Name}='{Format(m.GetValue(data))}'");
                }

                foreach (var m in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty))
                {
                    result.Add($"{m.Name}='{Format(m.GetValue(data))}'");
                }

                return "{" + String.Join(", ", result) + "}";
            }
            else
                return value;
        }

        [Conditional("DEBUG")]
        public static void TraceDebug(this TraceSource @this, string message)
        {
            @this.TraceEvent(TraceEventType.Verbose, 0, message, null);
        }

        [Conditional("DEBUG")]
        public static void TraceDebug(this TraceSource @this, string format, params object[] args)
        {
            @this.TraceEvent(TraceEventType.Verbose, 0, format, args);
        }

        public static void TraceWarning(this TraceSource @this, string message)
        {
            @this.TraceEvent(TraceEventType.Warning, 0, message, null);
        }

        public static void TraceWarning(this TraceSource @this, string format, params object[] args)
        {
            @this.TraceEvent(TraceEventType.Warning, 0, format, args);
        }

        public static void TraceError(this TraceSource @this, string format, params object[] args)
        {
            @this.TraceEvent(TraceEventType.Error, 0, format, args);
        }

        public static void TraceCritical(this TraceSource @this, string format, params object[] args)
        {
            @this.TraceEvent(TraceEventType.Critical, 0, format, args);
        }

        public class DebugTraceScope : IDisposable
        {
            private TraceSource Trace;
            public string Message { get; private set; }
            public string Name { get; private set; }

            private const char MARKER_BEGIN = '<';
            private const char MARKER_END = '>';
            private const byte MARKER_LENGTH = 8;
            private const char MARKER_SPACE = ' ';

            public DebugTraceScope(TraceSource trace, string name, string message)
            {
                this.Trace = trace;
                this.Name = name;
                this.Message = message;

                Write(MARKER_BEGIN, Message);

            }

            public void Dispose()
            {
                if (Trace != null)
                {
                    Write(MARKER_END, Message);
                    Trace = null;
                }
            }

            public void Write(string message)
            {
                Write(MARKER_SPACE, message);
            }

            private void Write(char marker, string message)
            {
                Trace.TraceDebug(message != null ? "{0} {1}, \"{2}\"" : "{0} {1}", new string(marker, MARKER_LENGTH), Name, message);
            }
        }

        public static DebugTraceScope Scope(this TraceSource @this, string name, string message = null)
        {
            return new DebugTraceScope(@this, name, message);
        }
    }
}
