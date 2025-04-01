using System;
using System.Collections.Generic;
using System.Linq;
using BackEnd;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Game.DialogBoxes
{
    public class AuctionDialogBox : BaseDialogBox<(int, int)>
    {
        [SerializeField] private VisualTreeAsset playerBidFieldTemplate;
        private VisualElement _bidsContainer;
        private IntegerField[] _bids;
        private readonly (int, int) _errorValue = (-1, -1);
        
        /// <summary>
        /// Initialise the dialog box by setting values and callbacks.
        /// </summary>
        /// <param name="ownableCard">Card shown in the left right panel.</param>
        /// <param name="text">Text shown in the right panel.</param>
        /// 
        public void Initialise(VisualElement ownableCard, string text)
        {
            base.Initialise();
            SetTitle("Auction");
            SetCancelButton("Skip");
            SetConfirmButton("Submit");
            ConfirmBtn.SetEnabled(false);
            LeftPanel.Add(ownableCard);
            RightPanel.Q<Label>("text").text = text;
            
            _bids = new IntegerField[GameState.Players.Count];
            _bidsContainer = RightPanel.Q<VisualElement>("bids-container");
            for (var i = 0; i < _bids.Length; i++) { _bidsContainer.Add(CreateBidField(i)); }
        }
        
        /// <summary>
        /// Creates a bid field using the data of player at playerIndex.
        /// </summary>
        /// <param name="playerIndex">The index of the player in GameState.</param>
        /// <returns>A 'bid field', comprised of player name and a field for the player's bid.</returns>
        private VisualElement CreateBidField(int playerIndex)
        {
            VisualElement tempContainer = new VisualElement();
            playerBidFieldTemplate.CloneTree(tempContainer);
            var playerBidField = tempContainer.ElementAt(0);
            
            playerBidField.Q<Label>("name").text = GameState.Players[playerIndex].Name;
            var bidField = playerBidField.Q<IntegerField>("bid");
            bidField.userData = playerIndex;
            bidField.RegisterValueChangedCallback(OnBidValueChanged);
            _bids[playerIndex] = bidField;

            return playerBidField;
        }

        /// <summary>
        /// Called when a value is updated in a bid field. Clamps the new value between 0 and the amount of money the
        /// player holds minus 1. 
        /// </summary>
        /// <param name="evt">The event invoked by the change in value.</param>
        private void OnBidValueChanged(ChangeEvent<int> evt)
        {
            var field = (IntegerField)evt.target;
            var playerIndex = (int)field.userData;
            var upperBound = GameState.Players[playerIndex].Money - 1;
            var clampedValue = Math.Clamp(evt.newValue, 0, upperBound);
            field.SetValueWithoutNotify(clampedValue);

            ConfirmBtn.SetEnabled(GetHighestBid() != _errorValue);
        }
        
        private (int, int) GetHighestBid()
        {
            var highestBid = -1;
            var highestBidders = new List<int>();

            foreach (var bid in _bids)
            {
                if (bid.value > highestBid)
                {
                    highestBid = bid.value;
                    highestBidders.Clear();
                    highestBidders.Add((int)bid.userData);
                }
                else if (bid.value == highestBid)
                {
                    highestBidders.Add((int)bid.userData);
                }
            }

            // returns the error value if multiple players share the highest bid
            return highestBidders.Count != 1 ? _errorValue : (highestBidders.First(), highestBid);
        }

        protected override void HandleCancelClicked()
        {
            RaiseOnChoiceMade(_errorValue);
            Close();
        }

        protected override void HandleConfirmClicked()
        {
            RaiseOnChoiceMade(GetHighestBid());
            Close();
        }
    }
}