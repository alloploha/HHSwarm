using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.Tests.Extensions
{
    class TestResultsTraceListener : TraceListener
    {
        TestContext Context;
        StringBuilder Strings = new StringBuilder();

        public TestResultsTraceListener(TestContext context)
        {
            this.Context = context;
        }

        public void Clear()
        {
            Strings.Clear();
        }

        public string Result => Strings.ToString();

        public override void Write(string message)
        {
            Strings.Append(message);
            this.WriteLine(message);
        }

        public override void WriteLine(string message)
        {
            Strings.AppendLine(message);
            Context.WriteLine("{0}", message);
        }
    }
}
