using BackEnd;
using Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Game
{
    public class OwnedCardsController
    {
        private readonly VisualElement _container;
        private PlayerData _current;
        
        public OwnedCardsController(VisualElement container)
        {
            _container = container;
            
            // listen for new turn event
            GameState.OnNewPlayerTurn += SetupCurrentPlayer;
        }

        private void SetupCurrentPlayer()
        {
            if (_current != null) { _current.OnOwnedPropertiesUpdated -= RefreshCards; }
            _current = GameState.ActivePlayer;
            _current.OnOwnedPropertiesUpdated += RefreshCards;
            RefreshCards();
        }

        private void RefreshCards()
        {
            Debug.Log("Cleared cards");
            _container.Clear();
            ShowOwnedCards();
        }
        
        private void ShowOwnedCards()
        {
            foreach (var tileNo in _current.Properties)
            {
                var owned = GameState.Board[tileNo];
                var card = OwnableCardFactory.MakeCard((OwnableData)owned);
                _container.Add(card);
                Debug.Log(owned.Name);
            }
        }
    }
}