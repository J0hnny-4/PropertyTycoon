using System;
using BackEnd;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Game.DialogBoxes
{
    public class AuctionDialogBox : BaseDialogBox<Tuple<int, int>>
    {
        [SerializeField] private VisualTreeAsset playerBidFieldTemplate;
        private VisualElement _bidsContainer;
        private IntegerField[] _bids;
        
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
            _bids[playerIndex] = bidField;

            return playerBidField;
        }

        protected override void HandleCancelClicked()
        {
            RaiseOnChoiceMade(new Tuple<int, int>(-1, 0));
            Close();
        }

        protected override void HandleConfirmClicked()
        {
            var winningPlayer = 0;

            foreach (var bidField in _bids)
            {
                if (bidField.value <= winningPlayer) { continue; }
                winningPlayer = (int)bidField.userData;
            }
            
            RaiseOnChoiceMade(new Tuple<int, int>(winningPlayer, _bids[winningPlayer].value));
            Close();
        }
    }
}