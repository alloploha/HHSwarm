using HHSwarm.Native.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    /// <summary>
    /// @Resource.LayerName("skan"), class ResPose, 'ResPos', 'skan'
    /// </summary>
    [Serializable]
    public class SkeletonAnimationResourceLayer : ResourceLayer
    {
        /// <summary>
        /// 'id'
        /// </summary>
        public short ID;

        [Flags]
        public enum FLAGS : byte
        {
            SpeedSpecified = 0x01
        }

        /// <summary>
        /// 'len', time
        /// </summary>
        public double Diration;

        /// <summary>
        /// 'nspeed'
        /// </summary>
        public double? Speed;

        public enum AnimationMode : byte
        {
            /// <summary>
            /// 'WrapMode.ONCE'
            /// </summary>
            Once = 0,

            /// <summary>
            /// 'WrapMode.LOOP'
            /// </summary>
            Loop = 1,

            /// <summary>
            /// 'WrapMode.PONG'
            /// </summary>
            Pong = 2,

            /// <summary>
            /// 'WrapMode.PONGLOOP'
            /// </summary>
            PongLoop = 3
        }

        /// <summary>
        /// 'mode', 'defmode', 'animation mode', 'WrapMode'
        /// </summary>
        public AnimationMode Mode;

        /// <summary>
        /// 'FxTrack', 'fx'
        /// </summary>
        [Serializable]
        public class Effect
        {
            /// <summary>
            /// 'FxTrack.Event'
            /// </summary>
            [Serializable]
            public abstract class Event
            {
                /// <summary>
                /// 'tm'
                /// </summary>
                public double Time;

                public enum TYPE : byte
                {
                    SpawnSprite = 0,
                    Trigger = 1
                }

                [Serializable]
                public class SpawnSprite : Event
                {
                    /// <summary>
                    /// 'resnm'
                    /// </summary>
                    public string ResourceName;

                    /// <summary>
                    /// 'resver'
                    /// </summary>
                    public ushort ResourceVersion;

                    /// <summary>
                    /// 'sdt'
                    /// </summary>
                    public byte[] Data = Array.Empty<byte>();
                }

                [Serializable]
                public class Trigger : Event
                {
                    /// <summary>
                    /// 'id'
                    /// </summary>
                    public string ID;
                }
            }

            public Event[] Events = Array.Empty<Event>();
        }

        public readonly IList<Effect> Effects = new List<Effect>();

        /// <summary>
        /// 'Track'
        /// </summary>
        [Serializable]
        public class Track
        {
            /// <summary>
            /// 'bnm', 'bone'
            /// </summary>
            public string BoneName;

            /// <summary>
            /// 'Track.Frame'
            /// </summary>
            [Serializable]
            public class Frame
            {
                /// <summary>
                /// 'tm', 'time'
                /// </summary>
                public double Time;

                /// <summary>
                /// 'trans'
                /// </summary>
                public Coord3d trans;

                /// <summary>
                /// 'rang'
                /// </summary>
                public Angle RotationAngle;

                /// <summary>
                /// 'rax'
                /// </summary>
                public Coord3d RotationAxis;
            }

            /// <summary>
            /// 'frames'
            /// </summary>
            public Frame[] Frames = Array.Empty<Frame>();
        }

        public readonly IList<Track> Tracks = new List<Track>();
    }
}
