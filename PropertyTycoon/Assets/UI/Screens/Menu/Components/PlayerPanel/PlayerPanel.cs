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
        
        /// <summary>
        /// Constructs a Player panel.
        /// </summary>
        /// <param name="template">The uxml reference.</param>
        /// <param name="id">ID to assign to the player</param>
        public PlayerPanel(VisualTreeAsset template)
        {
            // clone template & set id
            template.CloneTree(this);
            
            // get reference to UI elements
            _removePlayerButton = this.Q<Button>("remove-player-button");
            _aiToggle = this.Q<Toggle>("ai-toggle");
            _playerName = this.Q<TextField>("name-input");
            _tokenDropdown = this.Q<EnumField>("token-dropdown");
            
            // register callbacks
            _removePlayerButton.RegisterCallback<ClickEvent>(OnRemovePlayerClicked);
        }

        public void CleanUp()
        {
            _removePlayerButton.UnregisterCallback<ClickEvent>(OnRemovePlayerClicked);
        }
        
        /// <summary>
        /// Removes the player by setting it to inactive and hiding its panel.
        /// </summary>
        /// <param name="e">IGNORE - Click event, passed by default.</param>
        private void OnRemovePlayerClicked(ClickEvent e)
        {
            this.RemoveFromHierarchy();
        }
    }
}