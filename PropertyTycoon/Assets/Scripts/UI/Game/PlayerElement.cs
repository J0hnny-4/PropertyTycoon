using Data;
using UnityEngine.UIElements;

namespace UI.Game
{
    public class PlayerElement : VisualElement
    {
        private readonly PlayerData _data;
        private readonly Label _money;
        private readonly VisualElement _jailIcon;
        private readonly VisualElement _leaderIcon;
            
            
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
            _data.OnUpdate += UpdateUI;
        }

        private void CleanUp()
        {
            _data.OnUpdate -= UpdateUI;
        }
        
        /// <summary>
        /// Updates variables to match player object.
        /// </summary>
        private void UpdateUI()
        {
            _money.text = $"$ {_data.Money}";
            _jailIcon.visible = (_data.TurnsLeftInJail != 0);
        }
    }
}