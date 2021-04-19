using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Native.Protocols.Hafen.WidgetMessageArguments
{
    class ResourceNamesWidgetCreateArguments<T> where T : Enum
    {
        private readonly Dictionary<T, string> ResourceName = new Dictionary<T, string>();

        public string this[T key]
        {
            set => ResourceName[key] = value;

            get => ResourceName[key];
        }

        public IEnumerable<string> ResourceNames => ResourceName.Values.AsEnumerable();
    }
}
