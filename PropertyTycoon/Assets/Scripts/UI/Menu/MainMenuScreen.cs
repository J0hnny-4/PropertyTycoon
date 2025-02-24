using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Menu
{
    public class MainMenuScreen : BaseScreen<MenuScreen>
    {
        private Button _startButton;
        private Button _quitButton;

        public override void Initialise()
        {
            // get reference to UI elements
            _startButton = Root.Q<Button>("start-button");
            _quitButton = Root.Q<Button>("quit-button");
            
            // register button actions
            _startButton.RegisterCallback<ClickEvent>(OnStartClicked);
            _quitButton.RegisterCallback<ClickEvent>(OnQuitClicked);
        }
        
        protected override void CleanUp()
        {
            _startButton.UnregisterCallback<ClickEvent>(OnStartClicked);
            _quitButton.UnregisterCallback<ClickEvent>(OnQuitClicked);
        }

        private void OnStartClicked(ClickEvent e)
        {
            Debug.Log("START clicked");
            UIManager.NavigateTo(MenuScreen.GameMode);
        }

        private void OnQuitClicked(ClickEvent e)
        {
            Debug.Log("QUIT clicked");
            Application.Quit();
        }
    }
}