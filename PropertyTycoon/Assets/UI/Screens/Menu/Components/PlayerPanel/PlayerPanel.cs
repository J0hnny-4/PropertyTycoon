using System;
using Objects.Tokens;
using UnityEngine.UIElements;

namespace UI.Screens.Menu.Components.PlayerPanel
{
    public class PlayerPanel : VisualElement
    {
        // todo: add reference to player element -- directly update player through input fields
        private readonly Token[] _tokens;
        private int _currentTokenIndex;
        private Token CurrentToken => _tokens[_currentTokenIndex];
        private readonly Button _removePlayerButton;
        private readonly Toggle _aiToggle;
        private readonly TextField _playerName;
        private readonly VisualElement _tokenPreview;
        private readonly Button _leftArrowButton;
        private readonly Button _rightArrowButton;
        public event Action<PlayerPanel> OnPlayerRemovedClicked;

        /// <summary>
        /// Constructs a Player panel.
        /// </summary>
        /// <param name="template">The uxml reference.</param>
        /// <param name="name">Default name of the player.</param>
        /// <param name="tokens">A reference to the array of tokens available.</param>
        public PlayerPanel(VisualTreeAsset template, string name, Token[] tokens)
        {
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
            
            // Set default name & takes the first token from the available ones
            _playerName.value = name;
            _tokens = tokens;
            _currentTokenIndex = 0;
            MoveToNextToken();
            
            // register callbacks
            _removePlayerButton.RegisterCallback<ClickEvent>(TriggerPayerRemovedClicked);
            _leftArrowButton.clicked += MoveToPreviousToken;
            _rightArrowButton.clicked += MoveToNextToken;
        }

        public void CleanUp()
        {
            RemoveFromToken();
            _leftArrowButton.clicked -= MoveToPreviousToken;
            _rightArrowButton.clicked -= MoveToNextToken;
            _removePlayerButton.UnregisterCallback<ClickEvent>(TriggerPayerRemovedClicked);
        }

        /// <summary>
        /// Removes itself as owner of the token, if it currently is.
        /// </summary>
        private void RemoveFromToken()
        {
            if (CurrentToken?.Owner == this) { CurrentToken.Owner = null; }
        }

        private void MoveToPreviousToken()
        {
            FindAndUpdateToken(forward:false);
        }
        
        private void MoveToNextToken()
        {
            FindAndUpdateToken(forward:true);
        }
        
        /// <summary>
        /// Does up to a full loop around <c>_tokens</c>, and updates <c>_currentTokenIndex</c> to reflect the next
        /// available token. It also updates the tokens owner in the process.
        /// </summary>
        /// <param name="forward">Decided the direction to follow. Set to <c>true</c> by default, setting it to
        /// <c>false</c> would instead find the previous available token.</param>>
        private void FindAndUpdateToken(bool forward)
        {
            RemoveFromToken();
            var increment = forward ? 1 : -1;
            var i = (_currentTokenIndex + increment + _tokens.Length) % _tokens.Length;
            while (i != _currentTokenIndex && _tokens[i].HasOwner)
            {
                i = (i + increment + _tokens.Length) % _tokens.Length;
            }
            _currentTokenIndex = i;
            _tokenPreview.style.backgroundImage = CurrentToken.icon;
            CurrentToken.Owner = this;
        }
        
        /// <summary>
        /// Enable/disables the remove button. Hiding it prevents the player from being removed.
        /// </summary>
        /// <param name="enable">State to be assigned to the button.</param>
        public void ToggleRemovePlayerButton(bool enable)
        {
            _removePlayerButton.SetEnabled(enable);
        }
        
        /// <summary>
        /// Trigger the <c>OnPlayerRemovedClicked</c> event. 
        /// </summary>
        /// <param name="e">IGNORE - Click event, passed by default.</param>
        private void TriggerPayerRemovedClicked(ClickEvent e)
        {
            OnPlayerRemovedClicked?.Invoke(this);
        }
    }
}