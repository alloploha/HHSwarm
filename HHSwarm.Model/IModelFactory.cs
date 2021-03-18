using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HHSwarm.Model.Haven;
using HHSwarm.Model.Widgets;

namespace HHSwarm.Model
{
    public interface IModelFactory
    {
        Character ConstructCharacter(CharactersListWidget widget, string characterName);
    }
}
