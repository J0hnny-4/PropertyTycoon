using System.Collections.Generic;
using UI.Game;

namespace UI
{
    /// <summary>
    /// Enum used to identify screens belonging to the game scene.
    /// Useful for testing.
    /// </summary>
    public enum GameScreen {
        MainGame,
        GameOver
    }

    /// <summary>
    /// Implementation of BaseUIManager used in the game.
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
    }
}