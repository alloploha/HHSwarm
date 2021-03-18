using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Common
{
    [Serializable]
    public struct Coord2d : IEquatable<Coord2d>
    {
        public double X;
        public double Y;

        public Coord2d(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public Coord2d(double value) : this(value, value)
        {
        }

        public Coord2d(Coord2d other) : this(other.X, other.Y)
        {
        }

        #region Equals
        public override bool Equals(object obj)
        {
            return obj is Coord2d ? Equals((Coord2d)obj) : false;
        }

        public bool Equals(Coord2d other)
        {
            return other != null ? this == other : false;
        }

        public static bool operator ==(Coord2d left, Coord2d right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(Coord2d left, Coord2d right)
        {
            return left.X != right.X || left.Y != right.Y;
        }

        public override int GetHashCode()
        {
            int hash = 23;
            hash *= 31 + X.GetHashCode();
            hash *= 31 + Y.GetHashCode();
            return hash;
        }
        #endregion

        #region Arithmetics
        public static Coord2d operator +(Coord2d left, Coord2d right)
        {
            return new Coord2d(left.X + right.X, left.Y + right.Y);
        }

        public static Coord2d operator -(Coord2d left, Coord2d right)
        {
            return new Coord2d(left.X - right.X, left.Y - right.Y);
        }

        public static Coord2d operator *(Coord2d left, Coord2d right)
        {
            return new Coord2d(left.X * right.X, left.Y * right.Y);
        }

        public static Coord2d operator /(Coord2d left, Coord2d right)
        {
            return new Coord2d(left.X / right.X, left.Y / right.Y);
        }

        public static Coord2d operator %(Coord2d left, Coord2d right)
        {
            return new Coord2d(left.X % right.X, left.Y % right.Y);
        }
        #endregion

        #region Round
        public Coord2i Rounded => new Coord2i((int)Math.Round(X), (int)Math.Round(Y));

        public Coord2i Truncated => new Coord2i((int)Math.Truncate(X), (int)Math.Truncate(Y));

        public Coord2i Ceiling => new Coord2i((int)Math.Ceiling(X), (int)Math.Ceiling(Y));

        public Coord2i Floor => new Coord2i((int)Math.Floor(X), (int)Math.Floor(Y));
        #endregion

        public override string ToString()
        {
            return $"X={X}, Y={Y}";
        }
    }
}
