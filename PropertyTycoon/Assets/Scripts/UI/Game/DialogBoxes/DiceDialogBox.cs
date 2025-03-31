using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Game.DialogBoxes
{
    public class DiceDialogBox : BaseDialogBox<bool>
    {
        private VisualElement[] _dices;
        private Texture2D[] _diceIcons;
        private int[] _result;
        
        /// <summary>
        /// Initialise the dialog box by setting values and callbacks.
        /// </summary>
        /// <param name="rollResult"> The values of the two dice. </param>
        public void Initialise(int[] rollResult)
        {
            base.Initialise();
            SetTitle("Roll Dice");
            SetConfirmButton("Roll");
            
            // get reference to dices
            _result = rollResult;
            _dices = new VisualElement[2];
            _dices[0] = LeftPanel.Q<VisualElement>("dice-1");
            _dices[1] = LeftPanel.Q<VisualElement>("dice-2");
            
            // get dice icons
            _diceIcons = Resources.LoadAll<Texture2D>("Images/Icons/Dice");
        }

        private async Task RollDice()
        {
            // shuffles dices around for a bit
            for (var i = 0; i < 20; i++) { 
                SetDicesValues(new [] {Random.Range(0, 6), Random.Range(0, 6)});
                await Task.Delay(100);
            }
            
            // finally, set them to the correct values
            SetDicesValues(_result);
            await Task.Delay(1500);
        }

        private void SetDicesValues(int[] values)
        {
            _dices[0].style.backgroundImage = _diceIcons[values[0]]; 
            _dices[1].style.backgroundImage = _diceIcons[values[1]]; 
        }

        protected override void HandleCancelClicked() { /* unused */ }

        protected override async void HandleConfirmClicked()
        {
            // prevents from pressing it more than once
            ConfirmBtn.SetEnabled(false);
            // waits for animation
            await RollDice();
            
            RaiseOnChoiceMade(true);
            Close();
        }
    }
}