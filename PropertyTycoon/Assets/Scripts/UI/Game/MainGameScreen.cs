using BackEnd;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Game
{
    public class MainGameScreen : BaseScreen<GameScreen>
    {
        [SerializeField] private VisualTreeAsset playerTemplate;
        private VisualElement _playersContainer;
        private VisualElement _controlButtonsContainer;
        
        public override void Initialise()
        {
            _playersContainer = Root.Q<VisualElement>("players-container");
            _controlButtonsContainer = Root.Q<VisualElement>("control-buttons-container");

            foreach (var playerData in GameState.Players)
            {
                _playersContainer.Add(new PlayerElement(playerTemplate, playerData));
            }
        }

        protected override void CleanUp()
        {
            // no events to unsubscribe from yet
        }
    }
}