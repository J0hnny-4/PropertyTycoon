using BackEnd;
using Data;
using UnityEngine.UIElements;

namespace UI.Game
{
    /// <summary>
    /// Controller used to handle logic & operations involved in displaying owned cards of the current player.
    /// </summary>
    public class OwnedCardsController
    {
        private readonly VisualElement _container;
        private PlayerData _currentPlayer;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="container">The UI element used as container for the cards.</param>
        public OwnedCardsController(VisualElement container)
        {
            _container = container;
            GameState.OnNewPlayerTurn += ShowCurrentPlayer; // listen for new turn event
        }

        /// <summary>
        /// Removes listener attached to previous player (if any), then sets up a new listener (listening for updates
        /// in owned properties) for the current player. Lastly, it refreshes the cards displayed.
        /// </summary>
        private void ShowCurrentPlayer()
        {
            if (_currentPlayer != null) { _currentPlayer.OnOwnedPropertiesUpdated -= RefreshCards; }
            _currentPlayer = GameState.ActivePlayer;
            _currentPlayer.OnOwnedPropertiesUpdated += RefreshCards;
            RefreshCards();
        }

        /// <summary>
        /// Clears the container, then displays the current player's owned cards.
        /// </summary>
        private void RefreshCards()
        {
            _container.Clear();
            ShowOwnedCards();
        }
        
        /// <summary>
        /// Displays the current player's owned cards.
        /// </summary>
        private void ShowOwnedCards()
        {
            foreach (var tileNo in _currentPlayer.Properties)
            {
                var owned = GameState.Board[tileNo];
                var card = OwnableCardFactory.MakeCard((OwnableData)owned);
                card.AddToClassList("owned-card"); // add class for styling purpose
                _container.Add(card);
            }
        }

        /// <summary>
        /// Removes listeners.
        /// </summary>
        public void CleanUp()
        {
            if (_currentPlayer != null) { _currentPlayer.OnOwnedPropertiesUpdated -= RefreshCards; }
            GameState.OnNewPlayerTurn -= ShowCurrentPlayer;
        }
    }
}