using BackEnd;
using Data;
using UnityEngine.UIElements;

namespace UI.Game
{
    /// <summary>
    /// Handles displaying player's info during the game.
    /// It links the UI element (uxml template) to the PlayerData object to represent.
    /// </summary>
    public class PlayerElement : VisualElement
    {
        private readonly PlayerData _data;
        private readonly Label _money;
        private readonly VisualElement _jailIcon;
        private readonly VisualElement _leaderIcon;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="template">The player's uxml document template.</param>
        /// <param name="playerData">The PlayerData object to link to</param>
        public PlayerElement(VisualTreeAsset template, PlayerData playerData)
        {
            // clone template
            template.CloneTree(this);
            
            // set up variables
            _data = playerData;
            _money = this.Q<Label>("money");
            _jailIcon = this.Q<VisualElement>("jail-icon");
            _leaderIcon = this.Q<VisualElement>("leader-icon");
            
            // set up values
            this.Q<Label>("name").text = _data.Name;
            this.Q<VisualElement>("ai-icon").visible = _data.IsAi;
            this.Q<VisualElement>("token-icon").style.backgroundImage = _data.Token.icon;
            _leaderIcon.visible = false;
            UpdateUI();
            
            // register callbacks
            _data.OnStateUpdated += UpdateUI;
            _data.OnBankrupted += DisableElement;
            GameState.OnNewPlayerTurn += HighlightCurrent;
        }

        /// <summary>
        /// Removes listeners.
        /// </summary>
        public void CleanUp()
        {
            _data.OnStateUpdated -= UpdateUI;
            _data.OnBankrupted -= DisableElement;
            GameState.OnNewPlayerTurn -= HighlightCurrent;
        }
        
        /// <summary>
        /// Updates variables to match player object.
        /// </summary>
        private void UpdateUI()
        {
            _money.text = $"$ {_data.Money}";
            _jailIcon.visible = (_data.TurnsLeftInJail != 0);
        }

        /// <summary>
        /// Disables the player element, used to distinguish eliminated players.
        /// </summary>
        /// <param name="_">PlayerData object -- not used.</param>
        private void DisableElement(PlayerData _)
        {
            _money.text = "-";
            SetEnabled(false);
        }

        /// <summary>
        /// Highlights the current (active) player by changing its appearance (style dictated by the 'active' selector).
        /// </summary>
        private void HighlightCurrent()
        {
            var background = this.Q<VisualElement>("top");
            if (GameState.ActivePlayer == _data)
            {
                background.AddToClassList("active");
            }
            else 
            {
                background.RemoveFromClassList("active");
            }
        }
    }
}