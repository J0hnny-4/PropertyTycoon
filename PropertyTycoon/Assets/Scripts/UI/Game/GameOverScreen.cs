using BackEnd;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI.Game
{
    public class GameOverScreen : BaseScreen<GameScreen>
    {
        private Button _menuButton;
        private Button _quitButton;
        private Label _winnerLabel;

        public override void Initialise()
        {
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

        private void HandleGameOver(PlayerData winner)
        {
            _winnerLabel.text = $"{winner.Name} WON!";
            Show();
        }

        protected override void CleanUp()
        {
            _menuButton.UnregisterCallback<ClickEvent>(OnMenuClicked);
            _quitButton.UnregisterCallback<ClickEvent>(OnQuitClicked);
        }

        private void OnMenuClicked(ClickEvent e)
        {
            SceneManager.LoadScene("MenuScene");
        }
        
        private void OnQuitClicked(ClickEvent e)
        {
            Application.Quit();
        }
    }
}