using Data;
using UnityEngine.UIElements;

namespace UI.Menu
{
    /// <summary>
    /// Panel displaying information about a player. It allows to customise the player.
    /// </summary>
    public class PlayerPanel : VisualElement
    {
        private readonly PlayerSetupController _controller;
        private readonly PlayerData _playerData;
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
            // clone template & set its class (for styling purpose)
            template.CloneTree(this);
            AddToClassList("players-grid-cell");
            
            // get reference to UI elements
            _removePlayerButton = this.Q<Button>("remove-player-button");
            _aiToggle = this.Q<Toggle>("ai-toggle");
            _playerName = this.Q<TextField>("name-input");
            _tokenPreview = this.Q<VisualElement>("token-preview");
            _leftArrowButton = this.Q<Button>("left-arrow");
            _rightArrowButton = this.Q<Button>("right-arrow");
            
            // setup variables
            _controller = controller;
            _playerData = playerData;
            _playerName.value = playerData.Name;
            _aiToggle.value = playerData.IsAi;
            _tokenPreview.style.backgroundImage = _playerData.Token.icon;
            
            // register callbacks
            _playerName.RegisterCallback<ChangeEvent<string>>(UpdatePlayerName);
            _aiToggle.RegisterCallback<ChangeEvent<bool>>(UpdatePlayerAI);
            _removePlayerButton.RegisterCallback<ClickEvent>(HandleRemoveClicked);
            _leftArrowButton.clicked += MoveToPreviousToken;
            _rightArrowButton.clicked += MoveToNextToken;
            
            UpdateButtonsState();
        }

        public void CleanUp()
        {
            _playerName.UnregisterCallback<ChangeEvent<string>>(UpdatePlayerName);
            _aiToggle.UnregisterCallback<ChangeEvent<bool>>(UpdatePlayerAI);
            _removePlayerButton.UnregisterCallback<ClickEvent>(HandleRemoveClicked);
            _leftArrowButton.clicked -= MoveToPreviousToken;
            _rightArrowButton.clicked -= MoveToNextToken;
        }

        /// <summary>
        /// Updates state of buttons based on controller state.
        /// </summary>
        public void UpdateButtonsState()
        {
            _removePlayerButton.SetEnabled(_controller.CanRemovePlayer);
            _leftArrowButton.SetEnabled(_controller.CanSwitchToken);
            _rightArrowButton.SetEnabled(_controller.CanSwitchToken);
        }
        
        /// <summary>
        /// Method triggered when the 'name' text field is updated. It updates the player name accordingly.
        /// </summary>
        /// <param name="e">Event carrying the new (name) value.</param>
        private void UpdatePlayerName(ChangeEvent<string> e) => _playerData.Name = e.newValue;

        /// <summary>
        /// Method triggered when the 'ai' toggle is updated. It updates the player 'is AI' value accordingly.
        /// </summary>
        /// <param name="e">Event carrying the new (is AI) value.</param>
        private void UpdatePlayerAI(ChangeEvent<bool> e) => _playerData.IsAi = e.newValue;

        /// <summary>
        /// Shorthand method, calls UpdateToken moving forward.
        /// </summary>
        private void MoveToPreviousToken() => UpdateToken(false);

        /// <summary>
        /// Shorthand method, calls UpdateToken moving backward.
        /// </summary>
        private void MoveToNextToken() => UpdateToken(true);
        
        /// <summary>
        /// Calls controller to update token with the next available one.
        /// </summary>
        /// <param name="forward">Whether to go forward or backward.</param>
        private void UpdateToken(bool forward)
        {
            _playerData.Token = _controller.GetNextAvailableToken(_playerData.Token, forward);
            _tokenPreview.style.backgroundImage = _playerData.Token.icon;
        }
        
        /// <summary>
        /// Sends remove player command to controller.
        /// </summary>
        /// <param name="e">Click event -- not used.</param>
        private void HandleRemoveClicked(ClickEvent e) => _controller.RemovePlayer(_playerData);
    }
}