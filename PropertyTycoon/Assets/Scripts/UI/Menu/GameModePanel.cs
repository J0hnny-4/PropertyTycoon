using System;
using Data;
using UnityEngine.UIElements;

namespace UI.Menu
{
    public class GameModePanel : VisualElement
    {
        public GameMode GameMode { get; private set; }
        private readonly Toggle _selectedToggle;
        private readonly EventCallback<ClickEvent> _callback;
        public event Action<GameModePanel> OnClicked;
        
        /// <summary>
        /// Constructs a GameMode panel. 
        /// </summary>
        /// <param name="template">The uxml reference.</param>
        /// <param name="data">Data used to populate the panel.</param>
        public GameModePanel(VisualTreeAsset template, GameModeData data)
        {
            // clone template
            template.CloneTree(this);
            
            // set attributes
            GameMode = data.gameMode;
            _selectedToggle = this.Q<Toggle>("selected-toggle");
            _selectedToggle.value = false;
            
            // adjust UI
            this.Q<Label>("title").text = data.gameMode.ToString();
            this.Q<VisualElement>("image").style.backgroundImage = data.image;
            this.Q<Label>("description").text = data.description;

            // set up on click event & callback
            _callback = evt => OnClicked.Invoke(this);
            this.RegisterCallback<ClickEvent>(_callback);
        }
        
        /// <summary>
        /// Toggles whether the card is currently selected.
        /// </summary>
        /// <param name="selected">Value to assign to toggle.</param>
        public void ToggleSelected(bool selected)
        {
            _selectedToggle.value = selected;
        }
        
        /// <summary>
        /// Used to un-register the click event.
        /// </summary>
        public void CleanUp()
        {
            this.UnregisterCallback<ClickEvent>(_callback);
        }
    }
}