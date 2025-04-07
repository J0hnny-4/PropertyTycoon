using System;
using Data;
using UI.Game.DialogBoxes;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Game
{
    /// <summary>
    /// Factory class to easily create a wide variety of dialog boxes.
    /// </summary>
    public class DialogBoxFactory : MonoBehaviour
    {
        private static readonly GameObject SimpleDialogBoxPrefab = Resources.Load<GameObject>("Prefabs/UI/SimpleDialogBox");
        private static readonly GameObject AuctionDialogBoxPrefab = Resources.Load<GameObject>("Prefabs/UI/AuctionDialogBox");
        private static readonly GameObject DiceDialogBoxPrefab = Resources.Load<GameObject>("Prefabs/UI/DiceDialogBox");
        private static readonly GameObject ConfirmationDialogBoxPrefab = Resources.Load<GameObject>("Prefabs/UI/ConfirmationDialogBox");
        private static readonly GameObject AIDialogBoxPrefab = Resources.Load<GameObject>("Prefabs/UI/AIDialogBox");
        
        /// <summary>
        /// Helper method to create a simple, non customised dialog box. This dialog box is used as base and
        /// further customised by other methods.
        /// </summary>
        /// <returns>A plain, non customised simple dialog box.</returns>
        private static SimpleDialogBox MakeSimpleDialogBox()
        {
            var dialogObject = Instantiate(SimpleDialogBoxPrefab);
            var simpleDialogBox = dialogObject.GetComponent<SimpleDialogBox>();
            return simpleDialogBox;
        }

        /// <summary>
        /// Helper method to create a simple confirmation (yes/no) dialog box. This dialog box is used as base and
        /// further customised by other methods.
        /// </summary>
        /// <returns>A plain, non customised confirmation dialog box.</returns>
        private static SimpleDialogBox MakeConfirmationDialogBox()
        {
            var dialogObject = Instantiate(ConfirmationDialogBoxPrefab);
            var simpleDialogBox = dialogObject.GetComponent<SimpleDialogBox>();
            return simpleDialogBox;
        }

        /// <summary>
        /// Creates a confirmation (confirm/cancel) dialog box.
        /// </summary>
        /// <param name="title">The title shown by the dialog box.</param>
        /// <param name="text">A brief description of what the player is prompted to confirm.</param>
        /// <returns>A confirmation dialog box, with the options to confirm or cancel.</returns>
        public static SimpleDialogBox ConfirmDialogBox(string title, string text)
        {
            var dialogBox = MakeConfirmationDialogBox();
            dialogBox.Initialise(title, text, confirmText: "Confirm", cancelText: "Cancel");
            return dialogBox;
        }

        /// <summary>
        /// A temporary (self-closing) dialog box used to display a brief message. Mostly used to show outcomes of
        /// actions taken by AI players.
        /// </summary>
        /// <param name="title">The title shown by the dialog box.</param>
        /// <param name="text">A brief message shown by the dialog box.</param>
        /// <returns>A simple pop-up like dialog box, with no buttons. It closes automatically after a short
        /// time.</returns>
        public static AIDialogBox AIDialogBox(string title, string text)
        {
            var dialogObject = Instantiate(AIDialogBoxPrefab);
            var dialogBox = dialogObject.GetComponent<AIDialogBox>();
            dialogBox.Initialise(title, text);
            return dialogBox;
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
                case OwnableData ownableData:
                    image = OwnableCardFactory.MakeCard(ownableData);
                    text = $"You landed on {ownableData.Name}.\nYou need to pay ${amount} in rent.";
                    break;
                default:
                    image = MakeIconElement("fee");
                    text = $"You have been charged ${amount}.";
                    break;
            }
            dialogBox.Initialise("Payment Due", text, image, confirmText: "Pay");
            return dialogBox;
        }
        
        /// <summary>
        /// Creates a 'Dice' dialog box.
        /// </summary>
        /// <param name="playerName">Name of the player who's rolling the dice.</param>
        /// <param name="expectedResult">The pre-calculate result of the dice roll.</param>
        /// <returns>A simple dialog box, showing two dice shuffling and eventually landing on the expected
        /// result.</returns>
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
        public static SimpleDialogBox JailLandingDialogBox(bool afford)
        {
            var dialogBox = MakeSimpleDialogBox();
            var image = MakeIconElement("jail-1");
            var text = afford ? "Your bail is set to $50. Would you like to pay it?" : "";
            dialogBox.Initialise(
                "Jail",
                "You've been sent to prison.\n\n" + text,
                image,
                confirmText: afford ? "Pay" : null,
                cancelText: afford ? "Cancel" : "Continue");
            return dialogBox;
        }

        /// <summary>
        /// Creates a 'Bankruptcy' dialog box. 
        /// </summary>
        /// <param name="playerName">Name of the player going bankrupt.</param>
        /// <returns>A simple dialog box informing that a player has been eliminated.</returns>
        public static SimpleDialogBox BankruptcyDialogBox(string playerName)
        {
            var dialogBox = MakeSimpleDialogBox();
            var image = MakeIconElement("broken-piggy-bank");
            dialogBox.Initialise(
                "Bankruptcy",
                $"{playerName} has lost all of their money, they file for bankruptcy and leave the game!",
                image,
                confirmText: "Continue"
                );
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
