using System.Collections.Generic;
using BackEnd;
using Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Menu
{
    /// <summary>
    /// Allows the user to choose the game mode to play.
    /// </summary>
    public class GameModeScreen : BaseScreen<MenuScreen>
    {
        [SerializeField] private VisualTreeAsset gameModePanelTemplate; // uxml template (UI element) for the game mode
        [SerializeField] private List<GameModeData> gameModesData; // data about each game mode
        private List<GameModePanel> _gameModePanels;
        private GameModePanel _selectedPanel;
        private Button _continueButton;
        private Button _backButton;

        public override void Initialise()
        {
            // get reference to UI elements
            _continueButton = Root.Q<Button>("continue-button");
            _backButton = Root.Q<Button>("back-button");

            // register button actions
            _continueButton.RegisterCallback<ClickEvent>(OnContinueClicked);
            _backButton.RegisterCallback<ClickEvent>(OnBackClicked);

            // setup panels & updates the button state
            _gameModePanels = new List<GameModePanel>();
            UpdateContinueButtonState();
            GenerateGameModePanels();
        }

        protected override void CleanUp()
        {
            _continueButton.UnregisterCallback<ClickEvent>(OnContinueClicked);
            _backButton.UnregisterCallback<ClickEvent>(OnBackClicked);
            foreach (var panel in _gameModePanels)
            {
                panel.OnClicked -= OnPanelClicked;
                panel.CleanUp();
            }
        }

        /// <summary>
        /// Generates panels based on the given list (gameModesData). For each panel:<br/>
        /// - an <c>onClicked</c> event is registered.<br/>
        /// - a reference to the panel is added to <c>_gameModePanels</c>, to allow un-registering the callback.<br/>
        /// - the panel is added to the UI.
        /// </summary>
        private void GenerateGameModePanels()
        {
            var container = Root.Q<VisualElement>("content");
            foreach (var data in gameModesData)
            {
                var panel = new GameModePanel(gameModePanelTemplate, data);
                panel.OnClicked += OnPanelClicked;
                _gameModePanels.Add(panel);
                container.Add(panel);
            }
        }

        /// <summary>
        /// Invoked when clicked on a game panel. It de-select the previous (if any) selected panel and selects the
        /// newly clicked one.
        /// </summary>
        /// <param name="panel">The newly clicked panel</param>
        private void OnPanelClicked(GameModePanel panel)
        {
            _selectedPanel?.ToggleSelected(false);
            _selectedPanel = panel;
            _selectedPanel.ToggleSelected(true);
            UpdateContinueButtonState();
        }

        /// <summary>
        /// Uses <c>_selectedPanel</c> to determine whether to enable/disable the continue button: the button is only
        /// enabled if a mode has been selected. 
        /// </summary>
        private void UpdateContinueButtonState() => _continueButton.SetEnabled(_selectedPanel != null);
        
        /// <summary>
        /// Method triggered by the "continue" button. It saves the selected game mode, then takes the user to the
        /// players setup screen.
        /// </summary>
        /// <param name="e">Click event -- not used.</param>
        private void OnContinueClicked(ClickEvent e)
        {
            GameState.GameMode = _selectedPanel.GameMode;
            UIManager.NavigateTo(MenuScreen.PlayerSetup);
        }
        
        /// <summary>
        /// Method triggered by the "back" button. It takes the user back to the main screen.
        /// </summary>
        /// <param name="e">Click event -- not used.</param>
        private void OnBackClicked(ClickEvent e) => UIManager.NavigateTo(MenuScreen.MainMenu);
    }
}