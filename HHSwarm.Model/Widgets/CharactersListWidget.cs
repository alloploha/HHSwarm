using HHSwarm.Model.Common;
using HHSwarm.Model.Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Model.Widgets
{
    public class CharactersListWidget : Widget
    {
        /// <summary>
        /// 'height'
        /// </summary>
        public int Capacity;

        public Image BackgroundImage;

        public Image ScrollUpButtonUpImage;
        public Image ScrollUpButtonDownImage;
        public Image ScrollUpButtonHoverImage;
        public Image ScrollDownButtonUpImage;
        public Image ScrollDownButtonDownImage;
        public Image ScrollDownButtonHoverImage;

        public delegate void AddDelegate(IModelFactory models, string characterName, object desc, object map);
        public event AddDelegate Add;
        public virtual void OnAdd(IModelFactory models, string characterName, object desc, object map)
        {
            Add?.Invoke(models, characterName, desc, map);
        }

        public delegate void AvaDelegate(IModelFactory models, string characterName, object desc, object map);
        public event AvaDelegate Ava;
        public virtual void OnAva(IModelFactory models, string characterName, object desc, object map)
        {
            Ava?.Invoke(models, characterName, desc, map);
        }
    }
}
