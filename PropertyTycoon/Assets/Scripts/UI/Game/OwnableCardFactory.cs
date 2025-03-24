using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Game
{
    public class OwnableCardFactory : MonoBehaviour
    {
        private static OwnableCardFactory _instance;
        
        [SerializeField] private VisualTreeAsset propertyCardTemplate;
        [SerializeField] private VisualTreeAsset trainCardTemplate;
        [SerializeField] private VisualTreeAsset utilityCardTemplate;
        
        /// <summary>
        /// Not quite a singleton, but having access to an instance provides a way to assign templates in the editor,
        /// and easily access them from within the class.
        /// </summary>
        private void Awake() => _instance = this;
        

        /// <summary>
        /// A general method to create a card. It simply calls the correct method depending on the type of ownable data.
        /// </summary>
        /// <param name="ownableData">Data used to generate the card.</param>
        /// <returns>A card matching the data given.</returns>
        public static VisualElement MakeCard(OwnableData ownableData)
        {
            var ownableCard = new VisualElement();
            switch (ownableData)
            {
                case PropertyData propertyData:
                    ownableCard = MakePropertyCard(propertyData);
                    break;
                default:
                    Debug.LogWarning("Invalid ownable data");
                    break;
            }
            return ownableCard;
        }
        
        /// <summary>
        /// Creates a property card, based on the data given to it.
        /// </summary>
        /// <param name="propertyData">Data used to generate the card.</param>
        /// <returns>A card matching the data given.</returns>
        private static VisualElement MakePropertyCard(PropertyData propertyData)
        {
            var card = new VisualElement();
            _instance.propertyCardTemplate.CloneTree(card);

            var nameTag = card.Q<Label>("name");
            nameTag.text = propertyData.Name;
            nameTag.style.backgroundColor = propertyData.Colour.UnityColour;

            card.Q<VisualElement>("default-rent").Q<Label>("value").text = propertyData.Rent[0].ToString();
            card.Q<VisualElement>("color-set-rent").Q<Label>("value").text = (propertyData.Rent[0] * Cons.ColorSetMultiplier).ToString();
            card.Q<VisualElement>("one-house-rent").Q<Label>("value").text = propertyData.Rent[1].ToString();
            card.Q<VisualElement>("two-houses-rent").Q<Label>("value").text = propertyData.Rent[2].ToString();
            card.Q<VisualElement>("three-houses-rent").Q<Label>("value").text = propertyData.Rent[3].ToString();
            card.Q<VisualElement>("four-houses-rent").Q<Label>("value").text = propertyData.Rent[4].ToString();
            card.Q<VisualElement>("hotel-rent").Q<Label>("value").text = propertyData.Rent[5].ToString();
            
            card.Q<VisualElement>("house-cost").Q<Label>("value").text = propertyData.HouseCost.ToString();
            card.Q<VisualElement>("hotel-cost").Q<Label>("value").text = (propertyData.HouseCost * Cons.HotelCostMultiplier).ToString();

            return card;
        }
    }
}
