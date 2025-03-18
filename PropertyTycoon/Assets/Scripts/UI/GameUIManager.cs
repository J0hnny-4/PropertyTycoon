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
    }
}