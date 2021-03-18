using HHSwarm.Native.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.GameResources
{
    /// <summary>
    /// '@Resource.LayerName("mat2")', 'class NewMat'
    /// </summary>
    [Serializable]
    public class MaterialResourceLayer2 : ResourceLayer, IMaterialResourceLayer2PartsReceiver
    {
        public ushort MaterialID;
        public bool Linear;
        public bool Mipmap;

        /// <summary>
        /// '@ResName("nofacecull")', 'GL.GL_CULL_FACE'
        /// </summary>
        public bool FaceCulling;

        [Serializable]
        public abstract class Part
        {
        }

        [Serializable]
        public class CelShadePart : Part
        {
            public bool Diffuse;
            public bool Specular;
        }

        [Serializable]
        public class ColorsPart : Part
        {
            /// <summary>
            /// 'amb'
            /// </summary>
            public Color Ambient;

            /// <summary>
            /// 'dif'
            /// </summary>
            public Color Diffuse;

            /// <summary>
            /// 'spc'
            /// </summary>
            public Color Specular;

            /// <summary>
            /// 'emi'
            /// </summary>
            public Color Emission;

            /// <summary>
            /// 'shine'
            /// </summary>
            public float Shine;
        }

        [Serializable]
        public class LightPart : Part
        {
            public enum ShaderAlgorithm
            {
                /// <summary>
                /// "n"
                /// </summary>
                None,

                /// <summary>
                /// "def"
                /// </summary>
                Default,

                /// <summary>
                /// "pv"
                /// </summary>
                PhongVertex,

                /// <summary>
                /// "pp"
                /// </summary>
                PhongFragment
            }

            public ShaderAlgorithm Algorithm;
        }

        [Serializable]
        public class OrderPart : Part
        {
            public enum DrawOrderType : int
            {
                /// <summary>
                /// Order deflt = new Order.Default(0);
                /// </summary>
                Default = 0,

                /// <summary>
                /// Order first = new Order.Default(Integer.MIN_VALUE);
                /// </summary>
                First = int.MinValue,

                /// <summary>
                /// Order last = new Order.Default(Integer.MAX_VALUE);
                /// </summary>
                Last = int.MaxValue,

                /// <summary>
                /// Order postfx = new Order.Default(5000);
                /// </summary>
                PostFx = 5000,

                /// <summary>
                /// Order postpfx = new Order.Default(5500);
                /// </summary>
                PostPfx = 5500,

                /// <summary>
                /// Order eyesort = new EyeOrder(10000);
                /// </summary>
                EyeSort = 10000,

                /// <summary>
                /// Order eeyesort = new EyeOrder(4500);
                /// </summary>
                EarlyEyeSort = 4500,

                /// <summary>
                /// Order premap = new Order.Default(990);
                /// </summary>
                PreMap = 990,

                /// <summary>
                /// Order postmap = new Order.Default(1010)
                /// </summary>
                PostMap = 1010
            }

            public DrawOrderType DrawOrder;
        }

        [Serializable]
        public class TexPart : Part
        {
            public string ResourceName;
            public ushort ResourceVersion;
            public ushort TextureID;
            public bool Clip;
        }

        [Serializable]
        public class TexPalPart : Part
        {
            public string ResourceName;
            public ushort ResourceVersion;
            public ushort TextureID;
        }

        /// <summary>
        /// 'mlink'
        /// </summary>
        [Serializable]
        public class MaterialLink : Part
        {
            public string ResourceName;
            public ushort ResourceVersion;
            public ushort? ResourceLayerID;
        }

        /// <summary>
        /// 'OverTex', 'otex'
        /// </summary>
        [Serializable]
        public class OverTex : Part
        {
            public string ResourceName;
            public ushort ResourceVersion;
            public ushort TextureID;

            public enum BlendMode
            {
                /// <summary>
                /// 'cpblend', 'cp'
                /// </summary>
                cpblend,

                /// <summary>
                /// 'olblend', 'ol'
                /// </summary>
                olblend,

                /// <summary>
                /// 'colblend', 'a'
                /// </summary>
                colblend
            }

            /// <summary>
            /// 'blend'
            /// </summary>
            public BlendMode Blend;
        }

        public readonly IList<ColorsPart> Colors = new List<ColorsPart>();

        public void Receive(ColorsPart part)
        {
            Colors.Add(part);
        }

        public readonly IList<TexPalPart> TexPals = new List<TexPalPart>();

        public void Receive(TexPalPart part)
        {
            TexPals.Add(part);
        }

        public readonly IList<LightPart> Lights = new List<LightPart>();

        public void Receive(LightPart part)
        {
            Lights.Add(part);
        }

        public readonly IList<OrderPart> Orders = new List<OrderPart>();

        public void Receive(OrderPart part)
        {
            Orders.Add(part);
        }

        public readonly IList<TexPart> Texs = new List<TexPart>();

        public void Receive(TexPart part)
        {
            Texs.Add(part);
        }

        public readonly IList<CelShadePart> CelShades = new List<CelShadePart>();

        public void Receive(CelShadePart part)
        {
            CelShades.Add(part);
        }

        public readonly IList<MaterialLink> MaterialLinks = new List<MaterialLink>();

        public void Receive(MaterialLink part)
        {
            MaterialLinks.Add(part);
        }

        public readonly IList<OverTex> OverTexes = new List<OverTex>();

        public void Receive(OverTex part)
        {
            OverTexes.Add(part);
        }

        /// <summary>
        /// '@Material.ResName("vcol")', 'vcol', 'States.ColState', 'ColState', 'class ColState extends GLState'
        /// </summary>
        [Serializable]
        public class ColorState : Part
        {
            /// <summary>
            /// 'c'
            /// </summary>
            public Color Color;
        }

        public readonly IList<ColorState> ColorStates = new List<ColorState>();

        public void Receive(ColorState part)
        {
            ColorStates.Add(part);
        }

        /// <summary>
        /// '@Material.ResName("texrot")', 'texrot', 'class TexAnim'
        /// </summary>
        [Serializable]
        public class TexAnim : Part
        {
            /// <summary>
            /// 'ax'
            /// </summary>
            public Coord3f ax;
        }

        public readonly IList<TexAnim> TextureAnimations = new List<TexAnim>();

        public void Receive(TexAnim part)
        {
            TextureAnimations.Add(part);
        }

        /// <summary>
        /// '@Material.ResName("maskcol")', 'class $colmask', glColorMask
        /// glColorMask specify whether the individual color components in the frame buffer can or cannot be written.
        /// </summary>
        [Flags]
        public enum MaskColorPart
        {
            /// <summary>
            /// 'GLboolean red'
            /// </summary>
            Red = 0x01,

            /// <summary>
            /// 'GLboolean green'
            /// </summary>
            Green = 0x02,

            /// <summary>
            /// 'GLboolean blue'
            /// </summary>
            Blue = 0x04,

            /// <summary>
            /// 'GLboolean alpha'
            /// </summary>
            Alpha = 0x08,

            RGBA = 0x0F
        }

        public MaskColorPart MaskColor;
    }
}
