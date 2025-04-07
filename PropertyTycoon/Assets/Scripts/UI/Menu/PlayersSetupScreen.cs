using System.Collections.Generic;
using BackEnd;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI.Menu
{
    /// <summary>
    /// Allows to set up:
    /// - number of players.
    /// - their names.
    /// - their tokens.
    /// - whether they're human or AI.
    /// </summary>
    public class PlayersSetupScreen : BaseScreen<MenuScreen>
    {
        [SerializeField] private List<string> defaultNames; // list of default names, setup in the editor.
        [SerializeField] private VisualTreeAsset playerPanelTemplate; // uxml template (UI element) for each player
        private readonly Dictionary<PlayerData, PlayerPanel> _panels = new ();
        private PlayerSetupController _controller;
        private VisualElement _playersGrid;
        private Button _addPlayerButton;
        private Button _readyButton;
        private Button _backButton;
        
        public override void Initialise()
        {
            // creates controller
            _controller = new PlayerSetupController(defaultNames);
            
            // gets reference to UI elements
            _readyButton = Root.Q<Button>("ready-button");
            _backButton = Root.Q<Button>("back-button");
            _addPlayerButton = Root.Q<Button>("add-player-button");
            _playersGrid = Root.Q<VisualElement>("players-grid");
            
            // register listeners
            _readyButton.RegisterCallback<ClickEvent>(OnReadyClicked);
            _backButton.RegisterCallback<ClickEvent>(OnBackClicked);
            _addPlayerButton.clicked += _controller.AddPlayer;
            _controller.OnPlayerAdded += HandlePlayerAdded;
            _controller.OnPlayerRemoved += HandlePlayerRemoved;
            
            // initialises controller
            _controller.InitialisePlayers();
        }

        protected override void CleanUp()
        {
            _readyButton.UnregisterCallback<ClickEvent>(OnReadyClicked);
            _backButton.UnregisterCallback<ClickEvent>(OnBackClicked);
            _addPlayerButton.clicked -= _controller.AddPlayer;
            _controller.OnPlayerAdded -= HandlePlayerAdded;
            _controller.OnPlayerRemoved -= HandlePlayerRemoved;
        }

        /// <summary>
        /// When a new player is added, a new player panel is created and linked to the player. The panel is inserted
        /// based on number of players, rather than just appended at the end, as some additional VisualElement are used
        /// within the grid.
        /// </summary>
        /// <param name="player">The player to be added.</param>
        private void HandlePlayerAdded(PlayerData player)
        {
            var playerPanel = new PlayerPanel(playerPanelTemplate, player, _controller);
            var index = _controller.PlayersCount - 1;
            _playersGrid.hierarchy.Insert(index, playerPanel);
            _panels.Add(player, playerPanel);
            UpdateButtonsState();
        }

        /// <summary>
        /// When a player is removed, the corresponding panel is deleted and removed from the grid.
        /// </summary>
        /// <param name="player"></param>
        private void HandlePlayerRemoved(PlayerData player)
        {
            var panel = _panels[player];
            panel.CleanUp();
            _panels.Remove(player);
            _playersGrid.hierarchy.Remove(panel);
            UpdateButtonsState();
        }

        /// <summary>
        /// Enable/disable buttons depending on the controller's state.
        /// </summary>
        private void UpdateButtonsState()
        {
            var buttonContainer = _addPlayerButton.parent;
            buttonContainer.style.display = _controller.CanAddPlayer ? DisplayStyle.Flex : DisplayStyle.None;
            foreach (var panel in _panels.Values) { panel.UpdateButtonsState(); }
        }
        
        /// <summary>
        /// Method triggered by the "ready" button. It saves the created players, then moves the user to the game scene.
        /// </summary>
        /// <param name="e">Click event -- not used.</param>
        private void OnReadyClicked(ClickEvent e)
        {
            GameState.Players = _controller.GetPlayers();
            SceneManager.LoadScene("GameScene");
        }
        
        /// <summary>
        /// Method triggered by the "back" button. It takes the user back to the game mode screen.
        /// </summary>
        /// <param name="e">Click event -- not used.</param>
        private void OnBackClicked(ClickEvent e)
        {
            UIManager.NavigateTo(MenuScreen.GameMode);
        }
    }
}