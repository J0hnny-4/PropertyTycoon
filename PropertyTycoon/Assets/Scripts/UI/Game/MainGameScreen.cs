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
            
            // testing only
            var button = _controlButtonsContainer.Q<VisualElement>("forfeit-button").Q<Button>();
            button.clicked += TestingDiceRoll;
        }

        private async void TestingDiceRoll()
        {
            var values = new [] {Random.Range(0, 6), Random.Range(0, 6)};
            Debug.Log($"Testing dice: {values[0]} {values[1]}");
            
            var result = await DialogBoxFactory.MakeDiceDialogBox(values).AsTask();
            Debug.Log(result);
            
        }

        protected override void CleanUp()
        {
            // no events to unsubscribe from yet
        }
    }
}