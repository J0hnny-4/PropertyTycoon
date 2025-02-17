using System.Collections.Generic;
using UI.Screens.Menu.Components;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Screens.Menu
{
    public class GameModeScreen : BaseScreen
    {
        [SerializeField]
        private VisualTreeAsset gameModePanelTemplate;
        [SerializeField]
        private List<GameModeData> gameModesData;
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
            var UIelement = Root.Q<VisualElement>("content");
            foreach (var data in gameModesData)
            {
                var panel = new GameModePanel(gameModePanelTemplate, data);
                panel.OnClicked += OnPanelClicked;
                _gameModePanels.Add(panel);
                UIelement.Add(panel);
            }
        }
        
        /// <summary>
        /// Invoked when clicked on a game panel. It de-select the previous (if any) selected panel and selects the
        /// newly clicked one.
        /// </summary>
        /// <param name="panel">The newly clicked panel</param>
        private void OnPanelClicked(GameModePanel panel)
        {
            Debug.Log($"GameMode panel clicked: {panel}");
            _selectedPanel?.ToggleSelected(false);
            _selectedPanel = panel;
            _selectedPanel.ToggleSelected(true);
            UpdateContinueButtonState();
        }

        /// <summary>
        /// Uses <c>_selectedPanel</c> to determine whether to enable/disable the continue button.
        /// </summary>
        private void UpdateContinueButtonState()
        {
            _continueButton.SetEnabled(_selectedPanel != null);
        }
        
        private void OnContinueClicked(ClickEvent e)
        {
            Debug.Log("CONTINUE clicked");
        }

        private void OnBackClicked(ClickEvent e)
        {
            Debug.Log("BACK clicked");
        }
    }
}