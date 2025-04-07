using UnityEngine;
using UnityEngine.UIElements;
using GameState = BackEnd.GameState;

namespace UI.Game
{
    /// <summary>
    /// Base UI for the game scene. This screen is composed of 3 components:
    /// - control buttons (such as end-turn).
    /// - players elements (displaying info about each player).
    /// - owned cards area (showing properties owned by the current player).
    /// </summary>
    public class MainGameScreen : BaseScreen<GameScreen>
    {
        [SerializeField] private VisualTreeAsset playerTemplate; // uxml template (UI element) for the player element
        private OwnedCardsController _ownedCardsController;
        private VisualElement _playersContainer;
        private VisualElement _controlButtonsContainer;
        private Button _endTurnButton;
        private Button _forfeitButton;
        private Button _leaderboardButton;

        public override void Initialise()
        {
            // setup owned cards controller & player elements
            _ownedCardsController = new OwnedCardsController(Root.Q<VisualElement>("owned-cards-container"));

            // initialises player elements
            _playersContainer = Root.Q<VisualElement>("players-container");
            CreatePlayerElements();
            
            // initialises buttons 
            _controlButtonsContainer = Root.Q<VisualElement>("control-buttons-container");
            _endTurnButton = _controlButtonsContainer.Q<VisualElement>("end-turn-button").Q<Button>();
            _forfeitButton = _controlButtonsContainer.Q<VisualElement>("forfeit-button").Q<Button>();
            _leaderboardButton = _controlButtonsContainer.Q<VisualElement>("leaderboard-button").Q<Button>();
            _endTurnButton.clicked += EndTurn;
            _forfeitButton.clicked += Forfeit;
            _leaderboardButton.clicked += ShowLeaderboard;

            // listeners to enable/disable control buttons
            GameState.OnNewPlayerTurn += DisableControlButtons;
            GameState.OnActionsPhase += EnableControlButtons;
        }

        /// <summary>
        /// Creates player (UI) elements using the player data in game state.
        /// </summary>
        private void CreatePlayerElements()
        {
            foreach (var playerData in GameState.Players)
            {
                _playersContainer.Add(new PlayerElement(playerTemplate, playerData));
            }
        }

        /// <summary>
        /// Cleans up all player elements. 
        /// </summary>
        private void CleanUpPlayerElements()
        {
            foreach (var child in _playersContainer.Children())
            {
                if (child is PlayerElement playerElement) { playerElement.CleanUp(); }
            }
        }

        /// <summary>
        /// Disables control buttons.
        /// </summary>
        private void DisableControlButtons() => _controlButtonsContainer.SetEnabled(false);
        
        /// <summary>
        /// Enables control buttons.
        /// </summary>
        private void EnableControlButtons() => _controlButtonsContainer.SetEnabled(true);

        /// <summary>
        /// Method triggered by the "end turn" button. It prompts the user to confirm their action, then (if confirmed)
        /// un-pauses the game (effectively moving to the next turn).
        /// </summary>
        private async void EndTurn()
        {
            var confirmed = await DialogBoxFactory.ConfirmDialogBox(
                "End Turn",
                "You are about to end your turn."
            ).AsTask();
            if (!confirmed) return;
            GameState.Unpause();
        }

        /// <summary>
        /// Method triggered by the "forfeit" button. It prompts the user to confirm their action, then (if confirmed)
        /// eliminates the current player. Lastly, it un-pauses the game (effectively moving to the next turn).
        /// </summary>
        private async void Forfeit()
        {
            var confirmed = await DialogBoxFactory.ConfirmDialogBox(
                "Forfeit",
                "Are you sure you want to forfeit the game?"
                ).AsTask();
            if (!confirmed) return;
            GameState.ActivePlayer.Forfeit();
            GameState.Unpause();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowLeaderboard()
        {
            // todo: not yet implemented
        }

        protected override void CleanUp()
        {
            _endTurnButton.clicked -= EndTurn;
            _forfeitButton.clicked -= Forfeit;
            _leaderboardButton.clicked -= ShowLeaderboard;
            GameState.OnNewPlayerTurn -= DisableControlButtons;
            GameState.OnActionsPhase -= EnableControlButtons;
            _ownedCardsController.CleanUp();
            CleanUpPlayerElements();
        }
    }
}