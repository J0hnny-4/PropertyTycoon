using System;
using Data;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

namespace UI.Menu
{
    public class PlayerPanel : VisualElement
    {
        private PlayerSetupController _controller;
        private PlayerData _playerData;
        private readonly Button _removePlayerButton;
        private readonly Toggle _aiToggle;
        private readonly TextField _playerName;
        private readonly VisualElement _tokenPreview;
        private readonly Button _leftArrowButton;
        private readonly Button _rightArrowButton;

        /// <summary>
        /// Constructs a Player panel.
        /// </summary>
        /// <param name="template">The uxml reference.</param>
        /// <param name="playerData">PlayerData object to be linked to this panel.</param>
        /// <param name="controller">Reference to controller.</param>
        public PlayerPanel(VisualTreeAsset template, PlayerData playerData, PlayerSetupController controller)
        {
            // setup controller and register event listener
            _controller = controller;
            _controller.OnPlayersChanged += UpdateUI;
            
            // clone template & set its class
            template.CloneTree(this);
            this.AddToClassList("players-grid-cell");
            
            // get reference to UI elements
            _removePlayerButton = this.Q<Button>("remove-player-button");
            _aiToggle = this.Q<Toggle>("ai-toggle");
            _playerName = this.Q<TextField>("name-input");
            _tokenPreview = this.Q<VisualElement>("token-preview");
            _leftArrowButton = this.Q<Button>("left-arrow");
            _rightArrowButton = this.Q<Button>("right-arrow");
            
            // links to PlayerData object
            _playerData = playerData;
            _playerName.value = playerData.Name;
            _aiToggle.value = playerData.IsAI;
            _tokenPreview.style.backgroundImage = _playerData.Token.icon;
            
            // register callbacks
            _playerName.RegisterCallback<ChangeEvent<string>>(UpdatePlayerName);
            _aiToggle.RegisterCallback<ChangeEvent<bool>>(UpdatePlayerAI);
            _removePlayerButton.RegisterCallback<ClickEvent>(TriggerPayerRemovedClicked);
            _leftArrowButton.clicked += MoveToPreviousToken;
            _rightArrowButton.clicked += MoveToNextToken;
            
            UpdateUI();
        }

        private void CleanUp()
        {
            _controller.OnPlayersChanged -= UpdateUI;
            _leftArrowButton.clicked -= MoveToPreviousToken;
            _rightArrowButton.clicked -= MoveToNextToken;
            _removePlayerButton.UnregisterCallback<ClickEvent>(TriggerPayerRemovedClicked);
        }

        private void UpdatePlayerName(ChangeEvent<string> evt)
        {
            _playerData.Name = evt.newValue;
        }

        private void UpdatePlayerAI(ChangeEvent<bool> evt)
        {
            _playerData.IsAI = evt.newValue;
        }

        /// <summary>
        /// Updates state of buttons based on controller state.
        /// </summary>
        private void UpdateUI()
        {
            _removePlayerButton.SetEnabled(_controller.CanRemovePlayer);
            _leftArrowButton.SetEnabled(_controller.CanSwitchToken);
            _rightArrowButton.SetEnabled(_controller.CanSwitchToken);
        }

        private void MoveToPreviousToken()
        {
            _playerData.Token = _controller.GetNextAvailableToken(_playerData.Token, false);
            _tokenPreview.style.backgroundImage = _playerData.Token.icon;
        }
        
        private void MoveToNextToken()
        {
            _playerData.Token = _controller.GetNextAvailableToken(_playerData.Token, true);
            _tokenPreview.style.backgroundImage = _playerData.Token.icon;
        }
        
        /// <summary>
        /// Sends command to remove player. It then deletes itself from the hierarchy.
        /// </summary>
        /// <param name="e">IGNORE - Click event, passed by default.</param>
        private void TriggerPayerRemovedClicked(ClickEvent e)
        {
            _controller.RemovePlayer(_playerData);
            CleanUp();
            this.hierarchy.parent.Remove(this);
        }

    }
}