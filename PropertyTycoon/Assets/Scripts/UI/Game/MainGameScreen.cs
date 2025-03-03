using UnityEngine.UIElements;

namespace UI.Game
{
    public class MainGameScreen : BaseScreen<GameScreen>
    {
        private VisualElement _playersContainer;
        private VisualElement _controlButtonsContainer;
        
        public override void Initialise()
        {
            _playersContainer = Root.Q<VisualElement>("players-container");
            _controlButtonsContainer = Root.Q<VisualElement>("control-buttons-container");
        }

        protected override void CleanUp()
        {
            // no events to unsubscribe from yet
        }
    }
}