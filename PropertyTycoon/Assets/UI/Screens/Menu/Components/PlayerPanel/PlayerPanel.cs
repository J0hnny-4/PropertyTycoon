using System;
using UnityEngine.UIElements;

namespace UI.Screens.Menu.Components.PlayerPanel
{
    public class PlayerPanel : VisualElement
    {
        private Button _removePlayerButton;
        private Toggle _aiToggle;
        private TextField _playerName;
        private EnumField _tokenDropdown;
        public event Action<PlayerPanel> OnPlayerRemovedClicked;
        
        /// <summary>
        /// Constructs a Player panel.
        /// </summary>
        /// <param name="template">The uxml reference.</param>
        public PlayerPanel(VisualTreeAsset template)
        {
            // clone template & set its class
            template.CloneTree(this);
            this.AddToClassList("players-grid-cell");
            
            // get reference to UI elements
            _removePlayerButton = this.Q<Button>("remove-player-button");
            _aiToggle = this.Q<Toggle>("ai-toggle");
            _playerName = this.Q<TextField>("name-input");
            _tokenDropdown = this.Q<EnumField>("token-dropdown");
            
            // register callbacks
            _removePlayerButton.RegisterCallback<ClickEvent>(TriggerPayerRemovedClicked);
        }

        public void CleanUp()
        {
            _removePlayerButton.UnregisterCallback<ClickEvent>(TriggerPayerRemovedClicked);
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
        /// Trigger the <c>OnPlayerRemovedClicked</c> event, when 
        /// </summary>
        /// <param name="e">IGNORE - Click event, passed by default.</param>
        private void TriggerPayerRemovedClicked(ClickEvent e)
        {
            OnPlayerRemovedClicked?.Invoke(this);
        }
    }
}