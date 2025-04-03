using System;
using BackEnd;
using UnityEngine;
using UnityEngine.UIElements;
using GameState = BackEnd.GameState;
using Random = UnityEngine.Random;

namespace UI.Game
{
    public class MainGameScreen : BaseScreen<GameScreen>
    {
        [SerializeField] private VisualTreeAsset playerTemplate;
        
        private OwnedCardsController _ownedCardsController;
        private VisualElement _playersContainer;
        private VisualElement _controlButtonsContainer;
        private Button _endTurnButton;
        private Button _forfeitButton;
        private Button _leaderboardButton;


        public override void Initialise()
        {
            _ownedCardsController = new OwnedCardsController(Root.Q<VisualElement>("owned-cards-container"));

            _playersContainer = Root.Q<VisualElement>("players-container");
            _controlButtonsContainer = Root.Q<VisualElement>("control-buttons-container");

            foreach (var playerData in GameState.Players)
            {
                _playersContainer.Add(new PlayerElement(playerTemplate, playerData));
            }

            // initialises buttons
            _endTurnButton = _controlButtonsContainer.Q<VisualElement>("end-turn-button").Q<Button>();
            _forfeitButton = _controlButtonsContainer.Q<VisualElement>("forfeit-button").Q<Button>();
            _leaderboardButton = _controlButtonsContainer.Q<VisualElement>("leaderboard-button").Q<Button>();
            _endTurnButton.clicked += EndTurn;
            _forfeitButton.clicked += Forfeit;

            // listeners to enable/disable control buttons
            GameState.OnNewPlayerTurn += DisableControlButtons;
            GameState.OnActionsPhase += EnableControlButtons;
        }
        

        private void EndTurn() => GameState.Unpause();

        private void DisableControlButtons() => ToggleControlButtons(false);
        private void EnableControlButtons() => ToggleControlButtons(true);
        
        private void ToggleControlButtons(bool state)
        {
            _endTurnButton.SetEnabled(state);
            _forfeitButton.SetEnabled(state);
            _leaderboardButton.SetEnabled(state);
        }

        private void Forfeit()
        {
            GameState.ActivePlayer.Forfeit();
            EndTurn();
        }

        protected override void CleanUp()
        {
            _endTurnButton.clicked -= EndTurn;
        }
    }
}