using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Native.Tests.Extensions;

namespace System.Diagnostics.Tests
{
    [TestClass()]
    public class TraceSourceExtensionsTests
    {
        public TestContext TestContext { get; set; }

        class ClassToTrace
        {
            public string StringField = "A String";
            public bool BooleanField = true;
            public Int64 BigIntegerField = Int64.MinValue;
            public double BigRealField = double.PositiveInfinity;
            public byte[] BytesArrayField = new byte[] { 0x10, 0x20, 0x30 };
            public List<char> ListOfCharsField = new List<char>("abcdef".ToArray());
            public object ObjectProperty { get; set; } = "B String";
            public int SmallNumberProperty = 123456;
        }

        TraceSource TraceSource = new TraceSource(nameof(TraceSourceExtensionsTests), SourceLevels.Verbose);
        TestResultsTraceListener TraceListener = null;
        ClassToTrace TestData = null;

        [TestInitialize]
        public void ResetTestData()
        {
            TestData = new ClassToTrace();
            TraceListener = new TestResultsTraceListener(TestContext);
            TraceSource.Listeners.Clear();
            TraceSource.Listeners.Add(TraceListener);
        }

        [TestCleanup]
        public void Cleanup()
        {
            TraceSource.Listeners.Clear();
            TestData = null;
        }

        [TestMethod()]
        public void DumpTest1()
        {
            TraceSource.Dump(TraceEventType.Information, nameof(DumpTest1), TestData);
#if DEBUG
            StringAssert.Contains(TraceListener.Result, nameof(ClassToTrace));
            StringAssert.Contains(TraceListener.Result, Convert.ToString(TestData.StringField));
            StringAssert.Contains(TraceListener.Result, Convert.ToString(TestData.BigRealField));
#endif
        }

        [TestMethod()]
        public void DumpTest2()
        {
            TraceSource.Dump(TraceEventType.Information, nameof(DumpTest2), TestData.BytesArrayField);
#if DEBUG
            foreach (byte b in TestData.BytesArrayField)
            {
                StringAssert.Contains(TraceListener.Result, b.ToString("X"));
            }
#endif
        }

        [TestMethod()]
        public void DumpTest3()
        {
            TraceSource.Dump(TraceEventType.Information, nameof(DumpTest3), (object)null);
#if DEBUG
            StringAssert.Contains(TraceListener.Result, "null");
#endif
        }

        [TestMethod()]
        public void DumpTest4()
        {            
            TraceSource.Dump(TraceEventType.Verbose, nameof(DumpTest4), TestData.SmallNumberProperty);
#if DEBUG
            StringAssert.Contains(TraceListener.Result, Convert.ToString(TestData.SmallNumberProperty));
#endif
        }
    }
}