using System.Collections.Generic;
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
            _addPlayerButton.clicked += AddNewPlayer;
            _controller.OnPlayersChanged += ToggleAddPlayerButton;
            
            // initialise players
            for (var i = 0; i < minPlayers; i++) { AddNewPlayer(); }
        }

        protected override void CleanUp()
        {
            _readyButton.UnregisterCallback<ClickEvent>(OnReadyClicked);
            _backButton.UnregisterCallback<ClickEvent>(OnBackClicked);
            _addPlayerButton.clicked -= AddNewPlayer;
            _controller.OnPlayersChanged -= ToggleAddPlayerButton;
        }

        /// <summary>
        /// Adds a new player panel. The panel is inserted based on number of players, rather than just appended at the
        /// end.
        /// </summary>
        private void AddNewPlayer()
        {
            var newPlayer = _controller.AddPlayer();
            var playerPanel = new PlayerPanel(playerPanelTemplate, newPlayer, _controller);
            var index = _controller.PlayersCount - 1;
            _playersGrid.hierarchy.Insert(index, playerPanel);
        }

        /// <summary>
        /// Hide/show the add player button based on the controller's state.
        /// </summary>
        private void ToggleAddPlayerButton()
        {
            var buttonContainer = _addPlayerButton.parent;
            buttonContainer.style.display = _controller.CanAddPlayer ? DisplayStyle.Flex : DisplayStyle.None;
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