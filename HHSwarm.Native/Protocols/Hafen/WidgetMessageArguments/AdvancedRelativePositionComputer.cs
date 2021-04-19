using HHSwarm.Native.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments
{
    /// <summary>
    /// Improved version of 'relpos' computer, which can be used to unified parsing of <see cref="WidgetCreateAddChildArguments"/>.
    /// </summary>
    class AdvancedRelativePositionComputer : RelativePositionComputer
    {
        public AdvancedRelativePositionComputer
        (
            GetWidgetPositionRelativeToParentDelegate getWidgetPositionRelativeToParent,
            GetWidgetPositionRelativeToAnotherWidgetDelegate getWidgetPositionRelativeToAnotherWidget,
            GetWidgetSizeDelegate getWidgetSize
        )
        : base
        (
              getWidgetPositionRelativeToParent,
              getWidgetPositionRelativeToAnotherWidget,
              getWidgetSize
        )
        {
        }

        protected override void ExecuteOperator(char @operator, Queue<object> operands, ushort? parentWidgetID, ushort? childWidgetID)
        {
            switch (@operator)
            {
                case '+':
                    {
                        object b = Stack.Pop();
                        object a = Stack.Pop();

                        if (a is Coord2i && b is int)
                        {
                            Coord2i value = (Coord2i)a + new Coord2i((int)b);
                            Stack.Push(value);
                        }
                        else if (a is Coord2d && b is int)
                        {
                            Coord2i value = ((Coord2d)a + new Coord2d((int)b)).Rounded;
                            Stack.Push(value);
                        }
                        else if (a is Coord2d && b is Coord2i)
                        {
                            Coord2i value = ((Coord2d)a + new Coord2f((Coord2i)b)).Rounded;
                            Stack.Push(value);
                        }
                        else if (a is Coord2d && b is Coord2d)
                        {
                            Coord2d value = (Coord2d)a + (Coord2d)b;
                            Stack.Push(value);
                        }
                        else
                        {
                            Stack.Push(a);
                            Stack.Push(b);
                            base.ExecuteOperator(@operator, operands, parentWidgetID, childWidgetID);
                        }
                    }
                    break;
                case '-':
                    {
                        object b = Stack.Pop();
                        object a = Stack.Pop();

                        if (a is Coord2i && b is int)
                        {
                            Coord2i value = (Coord2i)a - new Coord2i((int)b);
                            Stack.Push(value);
                        }
                        else if (a is Coord2d && b is int)
                        {
                            Coord2i value = ((Coord2d)a - new Coord2d((int)b)).Rounded;
                            Stack.Push(value);
                        }
                        else if (a is Coord2d && b is Coord2i)
                        {
                            Coord2i value = ((Coord2d)a - new Coord2f((Coord2i)b)).Rounded;
                            Stack.Push(value);
                        }
                        else if (a is Coord2d && b is Coord2d)
                        {
                            Coord2d value = (Coord2d)a - (Coord2d)b;
                            Stack.Push(value);
                        }
                        else
                        {
                            Stack.Push(a);
                            Stack.Push(b);
                            base.ExecuteOperator(@operator, operands, parentWidgetID, childWidgetID);
                        }
                    }
                    break;
                case '*':
                    {
                        object b = Stack.Pop();
                        object a = Stack.Pop();

                        if (a is Coord2d && b is int)
                        {
                            Coord2i value = ((Coord2d)a * new Coord2d((int)b)).Rounded;
                            Stack.Push(value);
                        }
                        else if (a is Coord2d && b is Coord2i)
                        {
                            Coord2i value = ((Coord2d)a * new Coord2f((Coord2i)b)).Rounded;
                            Stack.Push(value);
                        }
                        else if (a is Coord2d && b is Coord2d)
                        {
                            Coord2d value = (Coord2d)a * (Coord2d)b;
                            Stack.Push(value);
                        }
                        else
                        {
                            Stack.Push(a);
                            Stack.Push(b);
                            base.ExecuteOperator(@operator, operands, parentWidgetID, childWidgetID);
                        }
                    }
                    break;
                case '/':
                    {
                        object b = Stack.Pop();
                        object a = Stack.Pop();

                        if (a is Coord2d && b is int)
                        {
                            Coord2i value = ((Coord2d)a / new Coord2d((int)b)).Rounded;
                            Stack.Push(value);
                        }
                        else if (a is Coord2d && b is Coord2i)
                        {
                            Coord2i value = ((Coord2d)a / new Coord2f((Coord2i)b)).Rounded;
                            Stack.Push(value);
                        }
                        else if (a is Coord2d && b is Coord2d)
                        {
                            Coord2d value = (Coord2d)a / (Coord2d)b;
                            Stack.Push(value);
                        }
                        else
                        {
                            Stack.Push(a);
                            Stack.Push(b);
                            base.ExecuteOperator(@operator, operands, parentWidgetID, childWidgetID);
                        }
                    }
                    break;
                default:
                    base.ExecuteOperator(@operator, operands, parentWidgetID, childWidgetID);
                    break;
            }
        }
    }
}
