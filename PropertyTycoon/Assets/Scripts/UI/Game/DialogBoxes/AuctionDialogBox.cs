using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd;
using Data;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

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
            LeftPanel.Add(ownableCard);
            RightPanel.Q<Label>("text").text = text;
            
            _bids = new IntegerField[GameState.Players.Count];
            _bidsContainer = RightPanel.Q<VisualElement>("bids-container");
            for (var i = 0; i < _bids.Length; i++) { _bidsContainer.Add(CreateBidField(i)); }
            
            SetButtonsState();
        }
        
        /// <summary>
        /// Creates a bid field using the data of player at playerIndex.
        /// </summary>
        /// <param name="playerIndex">The index of the player in GameState.</param>
        /// <returns>A 'bid field', comprised of player name and a field for the player's bid.</returns>
        private VisualElement CreateBidField(int playerIndex)
        {
            var tempContainer = new VisualElement();
            playerBidFieldTemplate.CloneTree(tempContainer);
            var playerBidField = tempContainer.ElementAt(0);
            
            playerBidField.Q<Label>("name").text = GameState.Players[playerIndex].Name;
            var bidField = playerBidField.Q<IntegerField>("bid");
            bidField.userData = playerIndex;
            bidField.RegisterValueChangedCallback(OnBidValueChanged);
            _bids[playerIndex] = bidField;

            var player = GameState.Players[playerIndex];
            
            // simply hides the current player's bid field, since they decided not to buy the property
            if (player == GameState.ActivePlayer || player.IsBankrupt)
            {
                playerBidField.style.display = DisplayStyle.None;
                bidField.SetValueWithoutNotify(0);
            } 
            else if (player.IsAi) // if player is AI, set a random amount they can afford, then disable their field (to prevent changing it)
            {
                bidField.SetValueWithoutNotify(Random.Range(0, player.Money - 1));
                playerBidField.SetEnabled(false);
            }
            
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

            SetButtonsState();
        }

        private void SetButtonsState()
        {
            // "continue" option is only available if a clear, single highest bid is provided
            // if not, then an agreement has not been reached, meaning the auction is skipped
            if (GetHighestBid() != _errorValue)
            {
                ConfirmBtn.SetEnabled(true);
                CancelBtn.SetEnabled(false);
            }
            else
            {
                ConfirmBtn.SetEnabled(false);
                CancelBtn.SetEnabled(true);
            }
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

        private bool AllBiddersAreAI()
        {
            foreach (var bidField in _bidsContainer.Children())
            {
                // hidden means the player is not part of the auction
                if (bidField.style.display == DisplayStyle.None) continue;
                // a field is enabled if it is not AI
                if (bidField.enabledSelf) return false;
            }
            return true;
        }

        public override async Task<(int, int)> AsTask()
        {
            if (!AllBiddersAreAI()) return await base.AsTask();
            
            // hides buttons
            ConfirmBtn.style.display = DisplayStyle.None;
            CancelBtn.style.display = DisplayStyle.None;
            await Task.Delay(Cons.AIDialogBoxDelay);
            Close();
            return GetHighestBid(); // return highest bidder
        }
    }
}