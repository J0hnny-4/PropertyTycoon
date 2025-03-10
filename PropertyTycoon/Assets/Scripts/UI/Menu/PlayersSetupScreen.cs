using System.Collections.Generic;
using Data;
using UI.Controllers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Menu
{
    public class PlayersSetupScreen : BaseScreen<MenuScreen>
    {
        [SerializeField] private List<string> defaultNames;
        [SerializeField] private VisualTreeAsset playerPanelTemplate;
        [SerializeField] private int minPlayers = 2;
        [SerializeField] private int maxPlayers = 6;
        private Dictionary<PlayerData, PlayerPanel> _panels = new ();
        private PlayerSetupController _controller;
        private VisualElement _playersGrid;
        private Button _addPlayerButton;
        private Button _readyButton;
        private Button _backButton;
        
        public override void Initialise()
        {
            // creates controller and listen to changes made by it
            _controller = new PlayerSetupController(minPlayers, maxPlayers, defaultNames);
            
            // get reference to UI elements
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
        
        private void OnReadyClicked(ClickEvent e)
        {
            // todo: setup players
            // todo: move to game scene
            var playersData = _controller.GetPlayers();
            
            Debug.Log($"Total players: {playersData.Count} ------------------------");
            for (var i = 0; i < playersData.Count; i++)
            {
                var player = playersData[i];
                Debug.Log($"Player {i+1} data: "
                    + $"\nName: {player.Name} "
                    + $"\nToken: {player.Token.name} "
                    + $"\nisAI: {player.IsAi}\n");
            }
            Debug.Log($"End of list -----------------------------------------------");
        }

        private void OnBackClicked(ClickEvent e)
        {
            UIManager.NavigateTo(MenuScreen.GameMode);
        }
    }
}