using HHSwarm.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Common
{
    [Serializable]
    public struct Coord3f : IEquatable<Coord3f>
    {
        public float X;
        public float Y;
        public float Z;

        public Coord3f(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Coord3f(float value) : this(value, value, value)
        {
        }

        public Coord3f(Coord3f other) : this(other.X, other.Y, other.Z)
        {
        }

        public static implicit operator Coord3d(Coord3f @this)
        {
            return new Coord3d(@this.X, @this.Y, @this.Z);
        }

        public static implicit operator Coordinate(Coord3f @this)
        {
            // TODO: Check 3D orientation
            return new Coordinate(right: @this.X, up: @this.Y, backwards: @this.Z);
        }

        #region Equals
        public override bool Equals(object obj)
        {
            return obj is Coord3f ? Equals((Coord3f)obj) : false;
        }

        public bool Equals(Coord3f other)
        {
            return other != null ? this == other : false;
        }

        public static bool operator ==(Coord3f left, Coord3f right)
        {
            return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
        }

        public static bool operator !=(Coord3f left, Coord3f right)
        {
            return left.X != right.X || left.Y != right.Y || left.Z != right.Z;
        }

        public override int GetHashCode()
        {
            int hash = 23;
            hash *= 31 + X.GetHashCode();
            hash *= 31 + Y.GetHashCode();
            hash *= 31 + Z.GetHashCode();
            return hash;
        }
        #endregion

        #region Arithmetics
        public static Coord3f operator +(Coord3f left, Coord3f right)
        {
            return new Coord3f(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public static Coord3f operator -(Coord3f left, Coord3f right)
        {
            return new Coord3f(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static Coord3f operator *(Coord3f left, Coord3f right)
        {
            return new Coord3f(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
        }

        public static Coord3f operator /(Coord3f left, Coord3f right)
        {
            return new Coord3f(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
        }

        public static Coord3f operator %(Coord3f left, Coord3f right)
        {
            return new Coord3f(left.X % right.X, left.Y % right.Y, left.Z % right.Z);
        }
        #endregion

        #region Round
        public Coord3i Rounded => new Coord3i((int)Math.Round(X), (int)Math.Round(Y), (int)Math.Round(Z));

        public Coord3i Truncated => new Coord3i((int)Math.Truncate(X), (int)Math.Truncate(Y), (int)Math.Round(Z));

        public Coord3i Ceiling => new Coord3i((int)Math.Ceiling(X), (int)Math.Ceiling(Y), (int)Math.Round(Z));

        public Coord3i Floor => new Coord3i((int)Math.Floor(X), (int)Math.Floor(Y), (int)Math.Round(Z));
        #endregion

        public override string ToString()
        {
            return $"X={X}, Y={Y}, Z={Z}";
        }
    }
}
