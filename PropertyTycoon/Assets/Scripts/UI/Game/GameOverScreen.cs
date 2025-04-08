using BackEnd;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI.Game
{
    /// <summary>
    /// Shows the winner's name, and allows the user to return to the menu or quit the game.
    /// </summary>
    public class GameOverScreen : BaseScreen
    {
        private Button _menuButton;
        private Button _quitButton;
        private Label _winnerLabel;

        public override void Initialise()
        {
            // sets screen type
            Type = ScreenType.GameOver;
            
            // get reference to UI elements
            _menuButton = Root.Q<Button>("menu-button");
            _quitButton = Root.Q<Button>("quit-button");
            _winnerLabel = Root.Q<Label>("winner-label");
            
            // register button actions
            _menuButton.RegisterCallback<ClickEvent>(OnMenuClicked);
            _quitButton.RegisterCallback<ClickEvent>(OnQuitClicked);
            
            // listens for game over event
            GameState.OnGameOver += HandleGameOver;
        }

        /// <summary>
        /// Sets the winner's name, then shows itself (the game over screen).
        /// </summary>
        /// <param name="winner">Data about the winning player.</param>
        private void HandleGameOver(PlayerData winner)
        {
            _winnerLabel.text = $"{winner.Name} won!";
            Show();
        }

        protected override void CleanUp()
        {
            _menuButton.UnregisterCallback<ClickEvent>(OnMenuClicked);
            _quitButton.UnregisterCallback<ClickEvent>(OnQuitClicked);
        }

        /// <summary>
        /// Method triggered by the "menu" button. It takes the user to the main menu screen.
        /// </summary>
        /// <param name="e">Click event -- not used.</param>
        private void OnMenuClicked(ClickEvent e) => SceneManager.LoadScene("MenuScene");
        
        /// <summary>
        /// Method triggered by the "quit" button. It closes the game.
        /// </summary>
        /// <param name="e">Click event -- not used.</param>
        private void OnQuitClicked(ClickEvent e) => Application.Quit();
    }
}