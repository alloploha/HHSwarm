using HHSwarm.Native.Common;
using System.Collections.Generic;
using System.Drawing;

namespace System.IO
{
    public interface IMessageBinaryReader
    {
        /// <summary>
        /// Position within the current stream
        /// </summary>
        long Position { get; set; }

        /// <summary>
        /// Length in bytes of the stream
        /// </summary>
        long Length { get; }

        byte[] ReadBytes(int count);
        short ReadInt16();
        int ReadInt32();
        long ReadInt64();

        /// <summary>
        /// Рекомендуется использовать <see cref="ReadString(int)"/>, т.к. иногда завершающий ноль '\0' может отсутствовать.
        /// </summary>
        /// <returns></returns>
        [Obsolete("Используйте метод ReadString(int) вместо ReadString().")]
        string ReadString();

        /// <summary>
        /// Считывает 'C-style' строку из потока.
        /// </summary>
        /// <example>
        /// <code>
        /// string text = reader.ReadString((int)(nextLayerPosition - reader.Position))
        /// </code>
        /// </example>
        /// <param name="maxLength">Максимальная длина данных строки, в байтах. Используется в случае если завершающий ноль не указан.</param>
        /// <returns></returns>
        string ReadString(int maxLength);
        ushort ReadUInt16();
        uint ReadUInt32();
        ulong ReadUInt64();
        ushort ReadUInt16BigEndian();
        short ReadInt16BigEndian();
        uint ReadUInt32BigEndian();
        int ReadInt32BigEndian();
        ulong ReadUInt64BigEndian();
        long ReadInt64BigEndian();
        Color ReadColor();

        /// <summary>
        /// Считывает два 32-х разрядных числа.
        /// <para>
        /// <see cref="Coord2i"/> может быть присвоен <see cref="Point"/>.
        /// <code>java: new MessageBuf(data).coord()</code>
        /// </para>
        /// </summary>
        Coord2i ReadCoord2i();
        Coord2f ReadCoord2f();
        Coord2d ReadCoord2d();
        Coord3i ReadCoord3i();
        Coord3f ReadCoord3f();
        Coord3d ReadCoord3d();

        /// <summary>
        /// <c>java: Message.uint8()</c>
        /// </summary>
        /// <remarks>
        /// https://github.com/dolda2000/hafen-client/blob/019f9dbcc1813a6bec0a13a0b7a3157177750ad2/src/haven/Message.java#L109
        /// </remarks>
        byte ReadByte();

        int PeekChar();
        int Read();
        int Read(byte[] buffer, int index, int count);
        int Read(char[] buffer, int index, int count);
        bool ReadBoolean();
        char ReadChar();
        char[] ReadChars(int count);
        decimal ReadDecimal();

        /// <summary>
        /// <c>java: double Message.float32()</c>
        /// </summary>
        /// <returns></returns>
        double ReadDouble();

        sbyte ReadSByte();
        float ReadSingle();

        /// <summary>
        /// <c>java: double Message.cpfloat()</c>
        /// </summary>
        /// <returns></returns>
        double ReadDouble20bit();

        /// <summary>
        /// <c>java: float Message.float16()</c>
        /// </summary>
        /// <returns></returns>
        float ReadSingle16bit();

        IEnumerable<object> ReadList();
        Coord3f ReadCoord3f32bit();
        Coord3f ReadCoord3f20bit();
        Color ReadColor20bit();
        Coord3f ReadCoord3f16bit();
    }
}