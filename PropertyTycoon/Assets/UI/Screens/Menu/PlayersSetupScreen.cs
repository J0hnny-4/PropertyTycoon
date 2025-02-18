using UI.Screens.Menu.Components.PlayerPanel;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Screens.Menu
{
    public class PlayersSetupScreen : BaseScreen
    {
        [SerializeField]
        private int minPlayers = 2;
        [SerializeField]
        private int maxPLayers = 6;
        [SerializeField]
        private VisualTreeAsset playerPanelTemplate;
        private VisualElement _playersGrid;
        private Button _readyButton;
        private Button _backButton;
        
        public override void Initialise()
        {
            // get reference to UI elements
            _readyButton = Root.Q<Button>("ready-button");
            _backButton = Root.Q<Button>("back-button");
            _playersGrid = Root.Q<VisualElement>("players-grid");
            
            // register button actions
            _readyButton.RegisterCallback<ClickEvent>(OnReadyClicked);
            _backButton.RegisterCallback<ClickEvent>(OnBackClicked);
            
            GeneratePlayersPanels();
        }

        protected override void CleanUp()
        {
            _readyButton.UnregisterCallback<ClickEvent>(OnReadyClicked);
            _backButton.UnregisterCallback<ClickEvent>(OnBackClicked);
        }
        
        /// <summary>
        /// Generates the player panels elements, according to the minimum number of players needed.
        /// </summary>
        private void GeneratePlayersPanels()
        {
            for (var i = 0; i < minPlayers; i++) { AddNewPlayer(); }
        }

        /// <summary>
        /// Adds a new player panel. It is added as a second-to-last element as the last element is a spacer used to
        /// maintain consistent look independent on the number of player panels.
        /// </summary>
        private void AddNewPlayer()
        {
            var playerPanel = new PlayerPanel(playerPanelTemplate);
            var childCount = _playersGrid.hierarchy.childCount;
            _playersGrid.hierarchy.Insert(childCount - 1,playerPanel);
        }
        
        private void OnReadyClicked(ClickEvent e)
        {
            AddNewPlayer();
            // todo: setup players
            // todo: move to game scene
        }

        private void OnBackClicked(ClickEvent e)
        {
            UIManager.NavigateTo<GameModeScreen>();
        }
    }
}