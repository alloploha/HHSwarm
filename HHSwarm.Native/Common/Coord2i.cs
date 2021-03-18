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
    public struct Coord2i : IEquatable<Coord2i>
    {
        public int X;
        public int Y;

        public Coord2i(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Coord2i(int value) : this(value, value)
        {
        }

        public Coord2i(Coord2i other) : this(other.X, other.Y)
        {
        }

        public Coord2i(Point other) : this(other.X, other.Y)
        {
        }

        public Coord2i(Coordinate other) : this(new Coord2f(x: other.Right, y: other.Up).Rounded)
        {
        }

        public static implicit operator Coord2f(Coord2i @this)
        {
            return new Coord2f(@this.X, @this.Y);
        }

        public static implicit operator Point(Coord2i @this)
        {
            return new Point(@this.X, @this.Y);
        }

        public static implicit operator PointF(Coord2i @this)
        {
            return new PointF(@this.X, @this.Y);
        }

        public static implicit operator Coordinate(Coord2i @this)
        {
            return new Coordinate(right: @this.X, up: @this.Y, backwards: 0);
        }

        #region Equals
        public override bool Equals(object obj)
        {
            return obj is Coord2i ? Equals((Coord2i)obj) : false;
        }

        public bool Equals(Coord2i other)
        {
            return other != null ? this == other : false;
        }

        public static bool operator ==(Coord2i left, Coord2i right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(Coord2i left, Coord2i right)
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
        public static Coord2i operator +(Coord2i left, Coord2i right)
        {
            return new Coord2i(left.X + right.X, left.Y + right.Y);
        }

        public static Coord2i operator -(Coord2i left, Coord2i right)
        {
            return new Coord2i(left.X - right.X, left.Y - right.Y);
        }

        public static Coord2i operator *(Coord2i left, Coord2i right)
        {
            return new Coord2i(left.X * right.X, left.Y * right.Y);
        }

        public static Coord2i operator /(Coord2i left, Coord2i right)
        {
            return new Coord2i(left.X / right.X, left.Y / right.Y);
        }

        public static Coord2i operator %(Coord2i left, Coord2i right)
        {
            return new Coord2i(left.X % right.X, left.Y % right.Y);
        }
        #endregion

        #region Round
        public Coord2i Rounded => new Coord2i((int)Math.Round((decimal)X), (int)Math.Round((decimal)Y));

        public Coord2i Truncated => new Coord2i((int)Math.Truncate((decimal)X), (int)Math.Truncate((decimal)Y));

        public Coord2i Ceiling => new Coord2i((int)Math.Ceiling((decimal)X), (int)Math.Ceiling((decimal)Y));

        public Coord2i Floor => new Coord2i((int)Math.Floor((decimal)X), (int)Math.Floor((decimal)Y));
        #endregion

        public override string ToString()
        {
            return $"X={X}, Y={Y}";
        }
    }
}
