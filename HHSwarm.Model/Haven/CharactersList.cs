using HHSwarm.Model.Widgets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSwarm.Model.Haven
{
    public class CharactersList : Widget<CharactersListWidget>
    {
        private List<Character> Characters = new List<Character>();

        internal CharactersList(Task<CharactersListWidget> widget) : base(widget)
        {
        }

        protected override void InitializeWidget(bool finalize)
        {
            Debug.WriteLine("===>", "**CharactersList::InitializeWidget");

            base.InitializeWidget(false);

            WidgetInfo.Add += (models, characterName, desc, map) =>
            {
                Character character = models.ConstructCharacter(WidgetInfo, characterName);

                Characters.Add(character);

                Debug.WriteLine("CharacterAdded?.Invoke =>", "**CharactersList::InitializeWidget");
                CharacterAdded?.Invoke(this, character);
                Debug.WriteLine("CharacterAdded?.Invoke <=", "**CharactersList::InitializeWidget");

                if (Characters.Count >= WidgetInfo.Capacity)
                    ListIsFull?.Invoke(this);
            };


            WidgetInfo.Ava += (models, characterName, desc, map) =>
            {

            };

            FinalizeInitialization(finalize);

            Debug.WriteLine("<===", "**CharactersList::InitializeWidget");
        }

        public delegate void CharacterAddedDelegate(CharactersList charactersList, Character character);
        public event CharacterAddedDelegate CharacterAdded;

        public delegate void ListIsFullDelegate(CharactersList charactersList);
        public event ListIsFullDelegate ListIsFull;
    }
}
