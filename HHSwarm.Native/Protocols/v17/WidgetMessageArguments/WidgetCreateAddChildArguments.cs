using HHSwarm.Native.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.v17.WidgetMessageArguments
{
    /// <summary>
    /// 'pargs'
    /// </summary>
    class WidgetCreateAddChildArguments
    {
        /// <summary>
        /// 'relpos(spec, ...)'
        /// </summary>
        public string Specification;

        /// <summary>
        /// 'relpos(.., args)'
        /// </summary>
        public object[] Arguments;

        /// <summary>
        /// '(Coord)args[0]'
        /// </summary>
        /// <param name="position"></param>
        public WidgetCreateAddChildArguments(Coord2i position)
        {
            this.Specification = "!";
            this.Arguments = new object[]
            {
                position
            };
        }

        /// <summary>
        /// '((Coord2d)args[0]).mul(new Coord2d(this.sz.sub(child.sz))).round()'
        /// </summary>
        /// <param name="position"></param>
        public WidgetCreateAddChildArguments(Coord2d position)
        {
            this.Specification = "!@s$s-*"; // <position>, (<parent-id> -> <size>, <child-id> -> <size>, <subtract>), <multiply>
            this.Arguments = new object[]
            {
                position
            };
        }

        public WidgetCreateAddChildArguments(string specification, object[] arguments)
        {
            this.Specification = specification;
            this.Arguments = arguments;
        }
    }
}
