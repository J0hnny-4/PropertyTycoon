using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Screens.Menu
{
    public class MainMenuScreen : BaseScreen
    {
        private Button _startButton;
        private Button _settingButon;
        private Button _quitButton;

        public override void Initialise()
        {
            // get reference to UI elements
            _startButton = Root.Q<Button>("start-button");
            _settingButon = Root.Q<Button>("setting-button");
            _quitButton = Root.Q<Button>("quit-button");
            
            // register button actions
            _startButton.RegisterCallback<ClickEvent>(OnStartClicked);
            _settingButon.RegisterCallback<ClickEvent>(OnSettingsClicked);
            _quitButton.RegisterCallback<ClickEvent>(OnQuitClicked);
        }
        
        protected override void CleanUp()
        {
            _startButton.UnregisterCallback<ClickEvent>(OnStartClicked);
            _settingButon.UnregisterCallback<ClickEvent>(OnSettingsClicked);
            _quitButton.UnregisterCallback<ClickEvent>(OnQuitClicked);
        }

        private void OnStartClicked(ClickEvent e)
        {
            Debug.Log("START clicked");
            UIManager.NavigateTo<GameModeScreen>();
        }

        private void OnSettingsClicked(ClickEvent e){
            Debug.Log("Settings clicked"); 
            UIManager.NavigateTo<SettingsScreen>();
        }

        private void OnQuitClicked(ClickEvent e)
        {
            Debug.Log("QUIT clicked");
            Application.Quit();
        }

        

    }
}