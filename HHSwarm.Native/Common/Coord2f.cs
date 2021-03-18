using HHSwarm.Model.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Common
{
    [Serializable]
    public struct Coord2f : IEquatable<Coord2f>
    {
        public float X;
        public float Y;

        public Coord2f(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public Coord2f(float value) : this(value, value)
        {
        }

        public Coord2f(Coord2f other) : this(other.X, other.Y)
        {
        }

        public Coord2f(PointF other) : this(other.X, other.Y)
        {
        }

        public static implicit operator Coord2d(Coord2f @this)
        {
            return new Coord2d(@this.X, @this.Y);
        }

        public static implicit operator PointF(Coord2f @this)
        {
            return new PointF(@this.X, @this.Y);
        }

        public static implicit operator Coordinate(Coord2f @this)
        {
            return new Coordinate(right: @this.X, up: @this.Y, backwards: 0);
        }

        #region Equals
        public override bool Equals(object obj)
        {
            return obj is Coord2f ? Equals((Coord2f)obj) : false;
        }

        public bool Equals(Coord2f other)
        {
            return other != null ? this == other : false;
        }

        public static bool operator ==(Coord2f left, Coord2f right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(Coord2f left, Coord2f right)
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
        public static Coord2f operator +(Coord2f left, Coord2f right)
        {
            return new Coord2f(left.X + right.X, left.Y + right.Y);
        }

        public static Coord2f operator -(Coord2f left, Coord2f right)
        {
            return new Coord2f(left.X - right.X, left.Y - right.Y);
        }

        public static Coord2f operator *(Coord2f left, Coord2f right)
        {
            return new Coord2f(left.X * right.X, left.Y * right.Y);
        }

        public static Coord2f operator /(Coord2f left, Coord2f right)
        {
            return new Coord2f(left.X / right.X, left.Y / right.Y);
        }

        public static Coord2f operator %(Coord2f left, Coord2f right)
        {
            return new Coord2f(left.X % right.X, left.Y % right.Y);
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
