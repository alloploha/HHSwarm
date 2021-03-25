using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments;
using HHSwarm.Native.Common;
using System.Collections.Generic;

namespace HHSwarm.Native.Tests.Protocols.v17.WidgetMessageArguments
{
    [TestClass]
    public class AdvancedRelativePositionComputerTests
    {
        [TestMethod]
        public void AddNumbers()
        {
            AdvancedRelativePositionComputer comp = new AdvancedRelativePositionComputer(null, null, null);
            comp.Run("!!+", new object[] { 13, 29 }, null, null);
            Assert.AreEqual(13 + 29, comp.Result);
        }

        [TestMethod]
        public void WidgetCreateAddChildArguments_Coord2i()
        {
            Coord2i position = new Coord2i(13, 27);
            AdvancedRelativePositionComputer comp = new AdvancedRelativePositionComputer(null, null, null);

            WidgetCreateAddChildArguments prog = new WidgetCreateAddChildArguments(position);

            comp.Run(prog.Specification, prog.Arguments, null, null);
            Assert.AreEqual(position, comp.Result);
        }

        [TestMethod]
        public void WidgetCreateAddChildArguments_Coord2d()
        {
            Coord2d position = new Coord2d(29, 37);

            Dictionary<ushort, Coord2i> size = new Dictionary<ushort, Coord2i>();
            const ushort PARENT_ID = 7;
            const ushort CHILD_ID = 5;
            size.Add(PARENT_ID, new Coord2i(-17, 31));
            size.Add(CHILD_ID, new Coord2i(23, 3));

            AdvancedRelativePositionComputer comp = new AdvancedRelativePositionComputer
            (
                getWidgetPositionRelativeToAnotherWidget: null,
                getWidgetPositionRelativeToParent: null,
                getWidgetSize: (id) => size[id]
            );

            WidgetCreateAddChildArguments prog = new WidgetCreateAddChildArguments(position);

            comp.Run(prog.Specification, prog.Arguments, PARENT_ID, CHILD_ID);

            Coord2i expected = (position * (Coord2f)(size[PARENT_ID] - size[CHILD_ID])).Rounded;

            Assert.AreEqual(expected, comp.Result);
        }
    }
}
