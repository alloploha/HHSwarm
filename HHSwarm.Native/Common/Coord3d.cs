using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace HHSwarm.Native.Common
{
    [Serializable]
    public struct Coord3d : IEquatable<Coord3d>
    {
        public double X;
        public double Y;
        public double Z;

        public Coord3d(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Coord3d(double value) : this(value, value, value)
        {
        }

        public Coord3d(Coord3d other) : this(other.X, other.Y, other.Z)
        {
        }

        #region Equals
        public override bool Equals(object obj)
        {
            return obj is Coord3d ? Equals((Coord3d)obj) : false;
        }

        public bool Equals(Coord3d other)
        {
            return other != null ? this == other : false;
        }

        public static bool operator ==(Coord3d left, Coord3d right)
        {
            return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
        }

        public static bool operator !=(Coord3d left, Coord3d right)
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
        public static Coord3d operator +(Coord3d left, Coord3d right)
        {
            return new Coord3d(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public static Coord3d operator -(Coord3d left, Coord3d right)
        {
            return new Coord3d(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static Coord3d operator *(Coord3d left, Coord3d right)
        {
            return new Coord3d(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
        }

        public static Coord3d operator /(Coord3d left, Coord3d right)
        {
            return new Coord3d(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
        }

        public static Coord3d operator %(Coord3d left, Coord3d right)
        {
            return new Coord3d(left.X % right.X, left.Y % right.Y, left.Z % right.Z);
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
