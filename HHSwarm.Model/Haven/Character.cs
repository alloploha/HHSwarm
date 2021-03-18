using HHSwarm.Model;
using HHSwarm.Model.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Model.Haven
{
    public class Character
    {
        public string Name { get; private set; }

        public Character(string name, ICommandAsync play)
        {
            this.Name = name;
            this.Play = play;
        }

        public ICommandAsync Play { get; set; }

        public Func<Inventory> OpenInventory { get; set; }
    }
}
