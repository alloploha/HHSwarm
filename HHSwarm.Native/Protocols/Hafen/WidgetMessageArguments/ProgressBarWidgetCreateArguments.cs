using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments
{
    /// <summary>
    /// <c>java: @RName("im")</c>
    /// 'IMeter'
    /// </summary>
    /// <remarks>
    /// </remarks>
    class ProgressBarWidgetCreateArguments : ResourceNamesWidgetCreateArguments<ProgressBarWidgetCreateArguments.RESOURCE_NAME>
    {
        public enum RESOURCE_NAME
        {
            BackgroundImage
        }

        /// <remarks>
        /// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/IMeter.java#L42
        /// </remarks>
        public ushort BackgroundImageResourceID;

        /// <summary>
        /// Секция полоски прогресс-бара.
        /// <c>java: IMeter.Meter</c>
        /// </summary>
        public class Meter
        {
            /// <summary>
            /// <c>java: double IMeter.Meter.c</c>
            /// </summary>
            public Color Color;

            /// <summary>
            /// <c>java: double IMeter.Meter.a</c>
            /// </summary>
            /// <remarks>
            /// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/IMeter.java#L56
            /// </remarks>
            public double Value;
        }

        /// <remarks>
        /// https://github.com/dolda2000/hafen-client/blob/f72eff8c3a3a5e22da71c45ceea1ceebd43a68e0/src/haven/IMeter.java#L37
        /// </remarks>
        public Meter[] Meters;
    }
}
