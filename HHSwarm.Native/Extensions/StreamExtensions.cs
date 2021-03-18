using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO
{
    public delegate BinaryReader CreateReaderDelegate(Stream stream, bool keepOpen);
    public delegate BinaryWriter CreateWriterDelegate(Stream stream, bool keepOpen);

    public static class StreamExtensions
    {
        public static void Enqueue(this Stream @this, byte[] data)
        {
            long position = @this.Position;
            @this.Position = @this.Length - 1;
            @this.Write(data, 0, data.Length);
            @this.Position = position;
        }

        public static byte[] Dequeue(this Stream @this)
        {
            long position = @this.Position;
            byte[] result = new byte[position];
            byte[] buffer = new byte[@this.Length - position];
            @this.Position = 0;
            @this.Read(result, 0, result.Length);
            @this.Read(buffer, 0, buffer.Length);
            @this.SetLength(buffer.Length);
            @this.Position = 0;
            @this.Write(buffer, 0, buffer.Length);
            @this.Position = position - result.Length;
            return result;
        }
    }
}
