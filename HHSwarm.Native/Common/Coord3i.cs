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
    public struct Coord3i : IEquatable<Coord3i>
    {
        public int X;
        public int Y;
        public int Z;

        public Coord3i(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Coord3i(int value) : this(value, value, value)
        {
        }

        public Coord3i(Coord3i other) : this(other.X, other.Y, other.Z)
        {
        }

        public static implicit operator Coord3f(Coord3i @this)
        {
            return new Coord3f(@this.X, @this.Y, @this.Z);
        }

        public static implicit operator Coordinate(Coord3i @this)
        {
            // TODO: Check 3D orientation
            return new Coordinate(right: @this.X, up: @this.Y, backwards: @this.Z);
        }

        #region Equals
        public override bool Equals(object obj)
        {
            return obj is Coord3i ? Equals((Coord3i)obj) : false;
        }

        public bool Equals(Coord3i other)
        {
            return other != null ? this == other : false;
        }

        public static bool operator ==(Coord3i left, Coord3i right)
        {
            return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
        }

        public static bool operator !=(Coord3i left, Coord3i right)
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
        public static Coord3i operator +(Coord3i left, Coord3i right)
        {
            return new Coord3i(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public static Coord3i operator -(Coord3i left, Coord3i right)
        {
            return new Coord3i(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static Coord3i operator *(Coord3i left, Coord3i right)
        {
            return new Coord3i(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
        }

        public static Coord3i operator /(Coord3i left, Coord3i right)
        {
            return new Coord3i(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
        }

        public static Coord3i operator %(Coord3i left, Coord3i right)
        {
            return new Coord3i(left.X % right.X, left.Y % right.Y, left.Z % right.Z);
        }
        #endregion

        #region Round
        public Coord3i Rounded => new Coord3i((int)Math.Round((decimal)X), (int)Math.Round((decimal)Y), (int)Math.Round((decimal)Z));

        public Coord3i Truncated => new Coord3i((int)Math.Truncate((decimal)X), (int)Math.Truncate((decimal)Y), (int)Math.Round((decimal)Z));

        public Coord3i Ceiling => new Coord3i((int)Math.Ceiling((decimal)X), (int)Math.Ceiling((decimal)Y), (int)Math.Round((decimal)Z));

        public Coord3i Floor => new Coord3i((int)Math.Floor((decimal)X), (int)Math.Floor((decimal)Y), (int)Math.Round((decimal)Z));
        #endregion

        public override string ToString()
        {
            return $"X={X}, Y={Y}, Z={Z}";
        }
    }
}
