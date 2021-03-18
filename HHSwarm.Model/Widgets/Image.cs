using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Model.Widgets
{
    public class Image
    {
        public short ID;
        public byte[] Data;

        public Image(short id, byte[] data)
        {
            this.ID = id;
            this.Data = data;
        }
    }
}
