using UnityEngine.UIElements;

namespace UI.Screens.Menu
{
    public class PlayersSetupScreen : BaseScreen
    {
        private Button _readyButton;
        private Button _backButton;
        
        public override void Initialise()
        {
            // get reference to UI elements
            _readyButton = Root.Q<Button>("ready-button");
            _backButton = Root.Q<Button>("back-button");
            
            // register button actions
            _readyButton.RegisterCallback<ClickEvent>(OnReadyClicked);
            _backButton.RegisterCallback<ClickEvent>(OnBackClicked);
        }

        protected override void CleanUp()
        {
            _readyButton.UnregisterCallback<ClickEvent>(OnReadyClicked);
            _backButton.UnregisterCallback<ClickEvent>(OnBackClicked);
        }

        private void OnReadyClicked(ClickEvent e)
        {
            // todo: setup players
            // todo: move to game scene
        }

        private void OnBackClicked(ClickEvent e)
        {
            UIManager.NavigateTo<GameModeScreen>();
        }
    }
}