using HHSwarm.Native.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments
{
    /// <summary>
    /// 'Widget::relpos'
    /// </summary>
    public abstract class RelativePositionComputer
    {
        protected Stack<object> Stack = new Stack<object>();

        public delegate Coord2i GetWidgetPositionRelativeToParentDelegate(ushort widgetID);
        private GetWidgetPositionRelativeToParentDelegate GetWidgetPositionRelativeToParent;

        public delegate Coord2i GetWidgetPositionRelativeToAnotherWidgetDelegate(ushort widgetID, ushort anotherWidgetID);
        private GetWidgetPositionRelativeToAnotherWidgetDelegate GetWidgetPositionRelativeToAnotherWidget;

        public delegate Coord2i GetWidgetSizeDelegate(ushort widgetID);
        private GetWidgetSizeDelegate GetWidgetSize;

        public RelativePositionComputer
        (
            GetWidgetPositionRelativeToParentDelegate getWidgetPositionRelativeToParent,
            GetWidgetPositionRelativeToAnotherWidgetDelegate getWidgetPositionRelativeToAnotherWidget,
            GetWidgetSizeDelegate getWidgetSize
        )
        {
            this.GetWidgetPositionRelativeToParent = getWidgetPositionRelativeToParent;
            this.GetWidgetPositionRelativeToAnotherWidget = getWidgetPositionRelativeToAnotherWidget;
            this.GetWidgetSize = getWidgetSize;
        }

        /// <summary>
        /// 'relpos'
        /// </summary>
        /// <param name="specification">'spec'</param>
        /// <param name="arguments">'args'</param>
        public void Run(IEnumerable<char> specification, IEnumerable<object> arguments, ushort? parentWidgetID, ushort? childWidgetID)
        {
            Queue<char> programm = new Queue<char>(specification);
            Queue<object> operands = new Queue<object>(arguments);

            while (programm.Count > 0)
            {
                if(char.IsDigit(programm.Peek()))
                {
                    StringBuilder sb = new StringBuilder();

                    while (programm.Count > 0 && char.IsDigit(programm.Peek()))
                    {
                        sb.Append(programm.Dequeue());
                    }

                    int value = int.Parse(sb.ToString());
                    Stack.Push(value);
                }

                char @operator = programm.Dequeue();

                if (char.IsWhiteSpace(@operator)) continue;

                ExecuteOperator(@operator, operands, parentWidgetID, childWidgetID);
            }
        }

        protected virtual void ExecuteOperator(char @operator, Queue<object> operands, ushort? parentWidgetID, ushort? childWidgetID)
        {
            switch (@operator)
            {
                case '!':
                    Stack.Push(operands.Dequeue());
                    break;
                case '$':
                    {
                        if (!childWidgetID.HasValue) throw new InvalidOperationException($"{nameof(childWidgetID)} is {childWidgetID} during '{@operator}'!");

                        Stack.Push(childWidgetID.Value);
                    }
                    break;
                case '@':
                    {
                        if (!parentWidgetID.HasValue) throw new InvalidOperationException($"{nameof(parentWidgetID)} is {parentWidgetID} during '{@operator}'!");

                        Stack.Push(parentWidgetID.Value);
                    }
                    break;
                case '_':
                    Stack.Push(Stack.Peek()); // duplicate top item
                    break;
                case '.':
                    Stack.Pop();
                    break;
                case '^':
                    {
                        object a = Stack.Pop();
                        object b = Stack.Pop();
                        Stack.Push(a);
                        Stack.Push(b);
                    }
                    break;
                case 'c':
                    {
                        int x = (int)Stack.Pop();
                        int y = (int)Stack.Pop();
                        Coord2i value = new Coord2i(x, y);
                        Stack.Push(value);
                    }
                    break;
                case 'o':
                    {
                        ushort id = (ushort)Stack.Pop();
                        Coord2i position = GetWidgetPositionRelativeToParent(widgetID: id); // 'c'
                        Coord2i size = GetWidgetSize(widgetID: id); // 'sz'
                        Coord2i value = position + size;
                        Stack.Push(value);
                    }
                    break;
                case 'p':
                    {
                        ushort id = (ushort)Stack.Pop();
                        Coord2i position = GetWidgetPositionRelativeToParent(widgetID: id);
                        Stack.Push(position);
                    }
                    break;
                case 'P':
                    {
                        ushort second_id = (ushort)Stack.Pop(); // 'parent'
                        ushort first_id = (ushort)Stack.Pop();
                        Coord2i position = GetWidgetPositionRelativeToAnotherWidget(widgetID: first_id, anotherWidgetID: second_id);
                        Stack.Push(position);
                    }
                    break;
                case 's':
                    {
                        ushort id = (ushort)Stack.Pop();
                        Coord2i size = GetWidgetSize(widgetID: id);
                        Stack.Push(size);
                    }
                    break;
                case 'w':
                    {
                        Stack.Push((ushort)Stack.Pop()); // convert a number (int) to WindgetID (ushort)
                    }
                    break;
                case 'x':
                    {
                        Coord2i position = (Coord2i)Stack.Pop();
                        Stack.Push(position.X);
                    }
                    break;
                case 'y':
                    {
                        Coord2i position = (Coord2i)Stack.Pop();
                        Stack.Push(position.Y);
                    }
                    break;
                case '+':
                    {
                        object b = Stack.Pop();
                        object a = Stack.Pop();

                        if (a is int && b is int)
                        {
                            int value = (int)a + (int)b;
                            Stack.Push(value);
                        }
                        else if (a is Coord2i && b is Coord2i)
                        {
                            Coord2i value = (Coord2i)a + (Coord2i)b;
                            Stack.Push(value);
                        }
                        else
                            throw new InvalidOperationException($"{a} {@operator} {b}");
                    }
                    break;
                case '-':
                    {
                        object b = Stack.Pop();
                        object a = Stack.Pop();

                        if (a is int && b is int)
                        {
                            int value = (int)a - (int)b;
                            Stack.Push(value);
                        }
                        else if (a is Coord2i && b is Coord2i)
                        {
                            Coord2i value = (Coord2i)a - (Coord2i)b;
                            Stack.Push(value);
                        }
                        else
                            throw new InvalidOperationException($"{a} {@operator} {b}");
                    }
                    break;
                case '*':
                    {
                        object b = Stack.Pop();
                        object a = Stack.Pop();

                        if (a is int && b is int)
                        {
                            int value = (int)a * (int)b;
                            Stack.Push(value);
                        }
                        else if (a is Coord2i && b is int)
                        {
                            Coord2i value = (Coord2i)a * new Coord2i((int)b);
                            Stack.Push(value);
                        }
                        else if (a is Coord2i && b is Coord2i)
                        {
                            Coord2i value = (Coord2i)a * (Coord2i)b;
                            Stack.Push(value);
                        }                        
                        else
                            throw new InvalidOperationException($"{a} {@operator} {b}");
                    }
                    break;
                case '/':
                    {
                        object b = Stack.Pop();
                        object a = Stack.Pop();

                        if (a is int && b is int)
                        {
                            int value = (int)a / (int)b;
                            Stack.Push(value);
                        }
                        else if (a is Coord2i && b is int)
                        {
                            Coord2i value = (Coord2i)a / new Coord2i((int)b);
                            Stack.Push(value);
                        }
                        else if (a is Coord2i && b is Coord2i)
                        {
                            Coord2i value = (Coord2i)a / (Coord2i)b;
                            Stack.Push(value);
                        }                        
                        else
                            throw new InvalidOperationException($"{a} {@operator} {b}");
                    }
                    break;
                default:
                    throw new NotImplementedException($"Unexpected operator {@operator} in the {nameof(RelativePositionComputer)} during run of a programm!");
            }
        }

        public object Result => Stack.Count > 0 ? Stack.Peek() : null;

        public void Reset()
        {
            Stack.Clear();
        }
    }
}
