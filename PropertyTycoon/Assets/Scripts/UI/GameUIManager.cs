using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd.Squares;
using UI.Game;

namespace UI
{
    public enum GameScreen {
        MainGame,
    }

    /// <summary>
    /// Implementation of BaseUIManager used in the menu.
    /// </summary>
    public class GameUIManager : BaseUIManager<GameScreen>
    {
        private MainGameScreen _mainGameScreen;
        
        protected override void Awake()
        {
            base.Awake();
            if (!Screens.TryGetValue(GameScreen.MainGame, out var baseScreen))
            {
                throw new KeyNotFoundException("Cannot find GameScreen.");
            };
            _mainGameScreen = (MainGameScreen)baseScreen;
            _mainGameScreen.Show();
        }
        
        /// <summary>
        /// Shows an dialog box prompting the player to pay.
        /// </summary>
        /// <param name="square">The type of Square associated with this interaction</param>
        /// <param name="amount">Amount to pay.</param>
        /// <returns>True once the player presses the pay button.</returns>
        public static Task<bool> PayFeeDialogBox(Square square, int amount)
        {
            return DialogBoxFactory.MakePaymentDialogBox(square, amount).AsTask();
        }

        /// <summary>
        /// Shows a dialog box prompting the player to choose between staying in prison or pay their bail. 
        /// </summary>
        /// <returns>True if the player decides to pay, False if they decide to stay in prison.</returns>
        public static Task<bool> JailLandingDialogBox()
        {
            // todo add option for card?
            // todo disable pay button if insufficient funds?
            return DialogBoxFactory.MakeJailLandingDialogBox().AsTask();
        }
        
        
    }
}