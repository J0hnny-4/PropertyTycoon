using Data;
using UI.Game.DialogBoxes;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Game
{
    public class DialogBoxFactory : MonoBehaviour
    {
        private static readonly GameObject SimpleDialogBoxPrefab = Resources.Load<GameObject>("Prefabs/UI/SimpleDialogBox");

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
        public static SimpleDialogBox MakePaymentDialogBox(object item, int amount)
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

        /// <summary>
        /// Creates a 'Jail' dialog box.
        /// </summary>
        /// <returns>A simple dialog box, showing the 'pay' (true) and 'cancel' (false) buttons.</returns>
        public static SimpleDialogBox MakeJailLandingDialogBox()
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

        public static SimpleDialogBox MakePurchaseDialogBox(OwnableData ownableData)
        {
            var dialogBox = MakeSimpleDialogBox();
            var text = $"You landed on {ownableData.Name}.\nWould you like to buy it for ${ownableData.Cost}?";
            
            var image = ownableData switch
            {
                PropertyData propertyData => OwnableCardFactory.MakeCard(propertyData),
                _ => MakeIconElement("cross")
            };
            
            dialogBox.Initialise(ownableData.Name, text, image, confirmText: "Yes", cancelText: "No");
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
