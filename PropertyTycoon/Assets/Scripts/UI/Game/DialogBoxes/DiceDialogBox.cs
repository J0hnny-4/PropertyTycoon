using System;
using System.Threading.Tasks;
using Data;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace UI.Game.DialogBoxes
{
    /// <summary>
    /// A dialog box prompting the player to roll the dice. 
    /// </summary>
    public class DiceDialogBox : BaseDialogBox<bool>
    {
        private VisualElement[] _dices;
        private Texture2D[] _diceIcons;
        private Tuple<int, int> _result;

        /// <summary>
        /// Initialise the dialog box by setting values and callbacks.
        /// </summary>
        /// <param name="playerName">Name of the player performing the dice roll.</param>
        /// <param name="rollResult"> The values of the two dice.</param>
        public void Initialise(string playerName, Tuple<int, int> rollResult)
        {
            base.Initialise();
            SetTitle($"{playerName}'s turn");
            SetConfirmButton("Roll");
            
            // get reference to dices
            _result = rollResult;
            _dices = new VisualElement[2];
            _dices[0] = LeftPanel.Q<VisualElement>("dice-1");
            _dices[1] = LeftPanel.Q<VisualElement>("dice-2");
            
            // get dice icons
            _diceIcons = Resources.LoadAll<Texture2D>("Images/Icons/Dice");
        }

        /// <summary>
        /// Shuffles the dice for a short time, then stops them on the correct (pre-calculated) result.
        /// </summary>
        private async Task RollDice()
        {
            // shuffles dices around for a bit
            for (var i = 0; i < 20; i++) { 
                SetDicesValues(new Tuple<int, int>(Random.Range(1, 7), Random.Range(1, 7)));
                await AsyncDelayHelper.DelayAsync(100);
            }
            SetDicesValues(_result); // finally, set them to the correct values
            await AsyncDelayHelper.DelayAsync(Cons.AIDialogBoxDelay);
        }

        /// <summary>
        /// Set the values shown by the dice.
        /// </summary>
        /// <param name="values">A tuple representing the values of both dice.</param>
        private void SetDicesValues(Tuple<int, int> values)
        {
            // adjust values to line up with 0 indexed array
            _dices[0].style.backgroundImage = _diceIcons[values.Item1 - 1]; 
            _dices[1].style.backgroundImage = _diceIcons[values.Item2 - 1]; 
        }

        /// <summary>
        /// Unused in this case.
        /// </summary>
        protected override void HandleCancelClicked() { /* unused */ }

        /// <summary>
        /// Plays the rolling dice animation, then returns (a throw-away value) and closes itself.
        /// </summary>
        protected override async void HandleConfirmClicked()
        {
            ConfirmBtn.SetEnabled(false); // prevents from pressing it more than once
            await RollDice(); // waits for animation
            RaiseOnChoiceMade(true);
            Close();
        }
    }
}