using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Menu
{
    /// <summary>
    /// Landing screen. Displays the game title/logo, and buttons for navigation.
    /// </summary>
    public class MainMenuScreen : BaseScreen
    {
        private Button _startButton;
        private Button _settingButton;
        private Button _quitButton;

        public override void Initialise()
        {
            // sets screen type
            Type = ScreenType.MainMenu;
            
            // get reference to UI elements
            _startButton = Root.Q<Button>("start-button");
            _settingButton = Root.Q<Button>("setting-button");
            _quitButton = Root.Q<Button>("quit-button");
            
            // register button actions
            _startButton.RegisterCallback<ClickEvent>(OnStartClicked);
            _settingButton.RegisterCallback<ClickEvent>(OnSettingsClicked);
            _quitButton.RegisterCallback<ClickEvent>(OnQuitClicked);
        }
        
        protected override void CleanUp()
        {
            _startButton.UnregisterCallback<ClickEvent>(OnStartClicked);
            _settingButton.UnregisterCallback<ClickEvent>(OnSettingsClicked);
            _quitButton.UnregisterCallback<ClickEvent>(OnQuitClicked);
        }

        /// <summary>
        /// Method triggered by the "start" button. It takes the user to the game mode screen.
        /// </summary>
        /// <param name="e">Click event -- not used.</param>
        private void OnStartClicked(ClickEvent e) => NavManager.NavigateTo(ScreenType.GameMode);

        /// <summary>
        /// Method triggered by the "settings" button. It takes the user to the settings screen.
        /// </summary>
        /// <param name="e">Click event -- not used.</param>
        private void OnSettingsClicked(ClickEvent e) => NavManager.NavigateTo(ScreenType.Settings);

        /// <summary>
        /// Method triggered by the "quit" button. It closes the game.
        /// </summary>
        /// <param name="e">Click event -- not used.</param>
        private void OnQuitClicked(ClickEvent e) => Application.Quit();

        

    }
}