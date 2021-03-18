using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Drawing
{
    public static class PointExtensions
    {
        public static Point Mul(this Point point, int value)
        {
            return new Point(x: point.X * value, y: point.Y * value);
        }

        public static Point Mul(this Point point, Point value)
        {
            return new Point(x: point.X * value.X, y: point.Y * value.Y);
        }

        public static Point Mul(this Point point, Size value)
        {
            return new Point(x: point.X * value.Width, y: point.Y * value.Height);
        }

        public static Point Div(this Point point, int value)
        {
            return new Point(x: point.X / value, y: point.Y / value);
        }

        public static Point Div(this Point point, Point value)
        {
            return new Point(x: point.X / value.X, y: point.Y / value.Y);
        }

        public static Point Div(this Point point, Size value)
        {
            return new Point(x: point.X / value.Width, y: point.Y / value.Height);
        }
    }
}
