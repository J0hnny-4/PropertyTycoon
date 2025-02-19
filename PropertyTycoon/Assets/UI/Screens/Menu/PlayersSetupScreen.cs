using UI.Managers;
using UI.Screens.Menu.Components.PlayerPanel;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Screens.Menu
{
    public class PlayersSetupScreen : BaseScreen<MenuScreen>
    {
        [SerializeField]
        private VisualTreeAsset playerPanelTemplate;
        [SerializeField]
        private int minPlayers = 2;
        [SerializeField]
        private int maxPlayers = 6;
        private int _currentPlayers;
        private VisualElement _playersGrid;
        private Button _addPlayerButton;
        private Button _readyButton;
        private Button _backButton;
        
        public override void Initialise()
        {
            // get reference to UI elements
            _readyButton = Root.Q<Button>("ready-button");
            _backButton = Root.Q<Button>("back-button");
            _playersGrid = Root.Q<VisualElement>("players-grid");
            _addPlayerButton = Root.Q<Button>("add-player-button");
            
            // register button actions
            _readyButton.RegisterCallback<ClickEvent>(OnReadyClicked);
            _backButton.RegisterCallback<ClickEvent>(OnBackClicked);
            _addPlayerButton.clicked += AddNewPlayer;

            _currentPlayers = 0;
            GeneratePlayersPanels();
        }

        protected override void CleanUp()
        {
            _readyButton.UnregisterCallback<ClickEvent>(OnReadyClicked);
            _backButton.UnregisterCallback<ClickEvent>(OnBackClicked);
            for (var i = 0; i < _currentPlayers; i++)
            {
                var panel = _playersGrid.hierarchy[i] as PlayerPanel;
                RemovePlayer(panel);
            }
        }
        
        /// <summary>
        /// Generates the player panels elements, according to the minimum number of players needed.
        /// </summary>
        private void GeneratePlayersPanels()
        {
            for (var i = 0; i < minPlayers; i++) { AddNewPlayer(); }
        }

        /// <summary>
        /// Adds a new player panel. The panel is inserted based on number of players, rather than just appended at the
        /// end.
        /// </summary>
        private void AddNewPlayer()
        {
            Debug.Assert(_currentPlayers < maxPlayers, "Current players cannot be more than minimum players.");
            var playerPanel = new PlayerPanel(playerPanelTemplate);
            playerPanel.OnPlayerRemovedClicked += RemovePlayer;
            _playersGrid.hierarchy.Insert(_currentPlayers, playerPanel);
            _currentPlayers++;
            UpdateGrid();
        }
        
        /// <summary>
        /// Removes a player panel, un-registering related events/callbacks in the process.
        /// </summary>
        /// <param name="playerPanel">The panel to be removed.</param>
        private void RemovePlayer(PlayerPanel playerPanel)
        {
            Debug.Assert(_currentPlayers > minPlayers, "Current players cannot be less than minimum players.");
            _currentPlayers--;
            playerPanel.OnPlayerRemovedClicked -= RemovePlayer;
            playerPanel.CleanUp();
            playerPanel.RemoveFromHierarchy();
            UpdateGrid();
        }

        /// <summary>
        /// Updates all panels. Called when a player is added/removed.
        /// </summary>
        private void UpdateGrid()
        {
            var canBeRemoved = _currentPlayers > minPlayers;
            for (var i = 0; i < _currentPlayers; i++)
            {
                var panel = _playersGrid.hierarchy[i] as PlayerPanel;
                Debug.Assert(panel != null);
                panel.ToggleRemovePlayerButton(canBeRemoved);
            }
            
            var canBeAdded = _currentPlayers < maxPlayers;
            ToggleAddPlayerButton(canBeAdded);
        }

        /// <summary>
        /// Toggles the add player button by simply hiding its container. This is because the button itself takes a slot
        /// in the 'players grid', therefore we want to hide this slot when we have reached the maximum number of
        /// players.
        /// </summary>
        /// <param name="enable">The state to set the button to.</param>
        private void ToggleAddPlayerButton(bool enable)
        {
            var buttonContainer = _addPlayerButton.parent;
            buttonContainer.style.display = enable ? DisplayStyle.Flex : DisplayStyle.None;
        }
        
        private void OnReadyClicked(ClickEvent e)
        {
            // todo: setup players
            // todo: move to game scene
        }

        private void OnBackClicked(ClickEvent e)
        {
            UIManager.NavigateTo(MenuScreen.GameMode);
        }
    }
}