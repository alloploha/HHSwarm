using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Common
{
    [Serializable]
    public struct Angle
    {
        private double Value;

        private Angle(double radians)
        {
            this.Value = radians;
        }

        #region radians
        public static Angle FromRadians(double radians)
        {
            return new Angle(radians);
        }

        /// <summary>
        /// rad
        /// </summary>
        public double Radians => this.Value;
        #endregion

        #region milliradians
        public static Angle FromMilliradians(double mil)
        {
            return new Angle(mil / 1000);
        }

        /// <summary>
        /// mil, mrad
        /// </summary>
        public double Milliradians => Radians * 1000;
        #endregion

        #region grad
        public static Angle FromGradian(double grad)
        {
            return new Angle(grad * Math.PI / 200);
        }

        /// <summary>
        /// gon, grad, grade
        /// </summary>
        public double Gradian => Radians * 200 / Math.PI;
        #endregion

        #region degrees
        public static Angle FromDegrees(double degrees)
        {
            return new Angle(degrees * Math.PI / 180);
        }

        public double Degrees => Radians * 180 / Math.PI;
        #endregion

        #region turns
        public static Angle FromTurns(double turns)
        {
            return new Angle(turns * Math.PI * 2);
        }

        public double Turns => Radians / Math.PI / 2;
        #endregion

        /// <summary>
        /// Sine
        /// </summary>
        public Angle Sin => new Angle(Math.Sin(Radians));

        /// <summary>
        /// Cosine
        /// </summary>
        public Angle Cos => new Angle(Math.Cos(Radians));

        /// <summary>
        /// Tangent
        /// </summary>
        public Angle Tan => new Angle(Math.Tan(Radians));

        /// <summary>
        /// Cotangent
        /// </summary>
        public Angle Cot => new Angle(1 / Math.Tan(Radians));

        /// <summary>
        /// Secant
        /// </summary>
        public Angle Sec => new Angle(1 / Math.Cos(Radians));

        /// <summary>
        /// Cosecant
        /// </summary>
        public Angle Csc => new Angle(1 / Math.Sin(Radians));

        public static Angle operator +(Angle @this, Angle other)
        {
            return new Angle(@this.Radians + other.Radians);
        }

        public static Angle operator -(Angle @this, Angle other)
        {
            return new Angle(@this.Radians - other.Radians);
        }

        public static Angle operator -(Angle @this)
        {
            return new Angle(-@this.Radians);
        }

        public static Angle operator *(Angle @this, double value)
        {
            return new Angle(@this.Radians * value);
        }

        public static Angle operator /(Angle @this, double value)
        {
            return new Angle(@this.Radians / value);
        }

        public Angle Reduced => new Angle(Radians % (Math.PI * 2));

        public static bool operator ==(Angle @this, Angle other)
        {
            return @this.Radians == other.Radians || @this.Reduced == other.Reduced;
        }

        public static bool operator !=(Angle @this, Angle other)
        {
            return @this.Radians != other.Radians && @this.Reduced != other.Reduced;
        }

        public override bool Equals(object obj)
        {
            return Equals((Angle)obj);
        }

        public bool Equals(Angle other, float epsilonDegrees = float.MinValue * 2)
        {
            return this == other || Math.Abs(this.Reduced.Degrees - other.Reduced.Degrees) <= epsilonDegrees;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }
    }
}
