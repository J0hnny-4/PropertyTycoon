using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UI.Game;

namespace BackEnd
{
    /// <summary>
    /// A class representing a card in the game, potluck, hard knocks etc.
    /// Functionally of the card is defined by a lambda function.
    /// </summary>
    public class Card
    {
        public string Name { get; }
        public string Description { get; }
        public Func<Task> Effect { get; }

        private object _value;

        public Card(string name, string description, String effect, object value = null)
        {
            this.Name = name;
            this.Description = description;
            this._value = value;

            switch (effect)
            {
                case "GoToJail":
                    Effect = async () =>
                    {
                        await DialogBoxFactory.AIDialogBox(name, description).AsTask();
                        await GameState.ActivePlayer.GoToJail();
                    };
                    break;
                case "PayPlayer":
                    Effect = async () =>
                    {
                        await DialogBoxFactory.AIDialogBox(name, description).AsTask();
                        GameState.ActivePlayer.AddMoney((int)_value);
                    };
                    break;
                default:
                    Effect = async () => { };
                    break;
            };
        }
    }
}