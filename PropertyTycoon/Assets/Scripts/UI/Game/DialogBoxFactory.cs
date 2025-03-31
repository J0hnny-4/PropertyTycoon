using System;
using Data;
using UI.Game.DialogBoxes;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Game
{
    public class DialogBoxFactory : MonoBehaviour
    {
        private static readonly GameObject SimpleDialogBoxPrefab = Resources.Load<GameObject>("Prefabs/UI/SimpleDialogBox");
        private static readonly GameObject AuctionDialogBoxPrefab = Resources.Load<GameObject>("Prefabs/UI/AuctionDialogBox");
        private static readonly GameObject DiceDialogBoxPrefab = Resources.Load<GameObject>("Prefabs/UI/DiceDialogBox");
        
        private static SimpleDialogBox MakeSimpleDialogBox()
        {
            var dialogObject = Instantiate(SimpleDialogBoxPrefab);
            var simpleDialogBox = dialogObject.GetComponent<SimpleDialogBox>();
            return simpleDialogBox;
        }

        /// <summary>
        /// Creates a 'Payment Due' dialog box. 
        /// </summary>
        /// <param name="item">The item prompting the payment (Property, Card, etc.)</param>
        /// <param name="amount">The amount due.</param>
        /// <returns>A simple dialog box, showing only the 'pay' (true) button.</returns>
        public static SimpleDialogBox PaymentDialogBox(object item, int amount)
        {
            var dialogBox = MakeSimpleDialogBox();
            VisualElement image;
            string text;
            
            switch (item)
            {
                case PropertyData propertyData:
                    image = OwnableCardFactory.MakeCard(propertyData);
                    text = $"You landed on {propertyData.Name}.\nYou need to pay ${amount} in rent.";
                    break;
                default:
                    image = MakeIconElement("fee");
                    text = $"You have been charged ${amount}.";
                    break;
            }
            dialogBox.Initialise("Payment Due", text, image, confirmText: "Pay");
            return dialogBox;
        }

        public static DiceDialogBox DiceDialogBox(string playerName, Tuple<int, int> expectedResult)
        {
            var dialogObject = Instantiate(DiceDialogBoxPrefab);
            var dialogBox = dialogObject.GetComponent<DiceDialogBox>();
            dialogBox.Initialise(playerName, expectedResult);
            return dialogBox;
        }

        /// <summary>
        /// Creates a 'Jail' dialog box (player being sent to jail).
        /// </summary>
        /// <returns>A simple dialog box, showing the 'pay' (true) and 'cancel' (false) buttons.</returns>
        public static SimpleDialogBox JailLandingDialogBox()
        {
            var dialogBox = MakeSimpleDialogBox();
            var image = MakeIconElement("jail-1");
            dialogBox.Initialise(
                "Jail", 
                "You've been sent to prison.\n\nYour bail is set to $50. Would you like to pay it?",
                image, 
                confirmText: "Pay", 
                cancelText: "Cancel");
            return dialogBox;
        }
        
        /// <summary>
        /// Creates a 'Jail' dialog box (player already in jail).
        /// </summary>
        /// <returns>A simple dialog box, showing the 'pay' (true) and 'cancel' (false) buttons.</returns>
        public static SimpleDialogBox PlayerInJailDialogBox(string playerName, int turnsLeft)
        {
            var dialogBox = MakeSimpleDialogBox();
            var image = MakeIconElement("jail-1");
            dialogBox.Initialise(
                "Jail", 
                $"{playerName} in in jail.\nTurns left: {turnsLeft}.",
                image, 
                confirmText: "Continue");
            return dialogBox;
        }

        /// <summary>
        /// Creates a 'purchase' dialog box.
        /// </summary>
        /// <param name="ownableData">Ownable for sale.</param>
        /// <returns>A simple dialog box, showing 'yes' (True) and 'no' (False) buttons.</returns>
        public static SimpleDialogBox PurchaseDialogBox(OwnableData ownableData)
        {
            var dialogBox = MakeSimpleDialogBox();
            var text = $"You landed on {ownableData.Name}.\nWould you like to buy it for ${ownableData.Cost}?";
            var image = OwnableCardFactory.MakeCard(ownableData);
            dialogBox.Initialise(ownableData.Name, text, image, confirmText: "Yes", cancelText: "No");
            return dialogBox;
        }

        /// <summary>
        /// Creates an 'auction' dialog box.
        /// </summary>
        /// <param name="ownableData">Ownable for sale.</param>
        /// <returns>A dialog box prompting players to input their bids.</returns>
        public static AuctionDialogBox AuctionDialogBox(OwnableData ownableData)
        {
            var dialogObject = Instantiate(AuctionDialogBoxPrefab);
            var dialogBox = dialogObject.GetComponent<AuctionDialogBox>();
            var text = $"The bank is auctioning {ownableData.Name}!\nEnter your bids...";
            var card = OwnableCardFactory.MakeCard(ownableData);
            dialogBox.Initialise(card, text);
            return dialogBox;
        }
        
        /// <summary>
        /// Helper function to get the name of the icon wanted, and embed it into a Visual Element.
        /// </summary>
        /// <param name="iconName">Filename of icon.</param>
        /// <returns>A Visual Element using the given icon as background.</returns>
        private static VisualElement MakeIconElement(string iconName)
        {
            var icon = Resources.Load<Texture2D>($"Images/Icons/{iconName}");
            var element = new VisualElement();
            element.style.backgroundImage = icon;
            element.name = "icon";
            return element;
        }
    }
}
