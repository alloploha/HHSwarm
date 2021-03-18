using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    /// <summary>
    /// '@Resource.LayerName("boneoff")'
    /// </summary>
    [Serializable]
    public class BoneOffsetResourceLayer : ResourceLayer
    {
        public enum COMMAND_TYPE : byte
        {
            Translate = 0,
            Rotate = 1,
            TranslateBoneByName = 2,
            AlignBone = 3
        }

        [Serializable]
        public abstract class BoneOffsetCommand
        {
        }

        [Serializable]
        public class Xlate : BoneOffsetCommand
        {
            public double X;
            public double Y;
            public double Z;
        }

        [Serializable]
        public class Rot : BoneOffsetCommand
        {
            public double Angle;
            public double AxisX;
            public double AxisY;
            public double AxisZ;
        }

        [Serializable]
        public class Bonetrans : BoneOffsetCommand
        {
            public string BoneName;
        }

        [Serializable]
        public class BoneAlign : BoneOffsetCommand
        {
            public double RefX;
            public double RefY;
            public double RefZ;
            public string OriginBoneName;
            public string TargetBoneName;
        }

        public string Name;
        public IList<BoneOffsetCommand> Commands = new List<BoneOffsetCommand>();
    }
}
