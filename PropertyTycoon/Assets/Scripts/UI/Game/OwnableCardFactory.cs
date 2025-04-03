using Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Game
{
    public class OwnableCardFactory : MonoBehaviour
    {
        private static OwnableCardFactory _instance;
        
        [SerializeField] private VisualTreeAsset propertyCardTemplate;
        [SerializeField] private VisualTreeAsset stationCardTemplate;
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
                case StationData stationData:
                    ownableCard = MakeStationCard(stationData);
                    break;
                case UtilityData utilityData:
                    ownableCard = MakeUtilityCard(utilityData);
                    break;
                default:
                    Debug.LogWarning($"Invalid ownable data: {ownableData}");
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

            card.Q<VisualElement>("default-rent").Q<Label>("value").text = ToPrice(propertyData.Rent[0]);
            card.Q<VisualElement>("color-set-rent").Q<Label>("value").text = ToPrice(propertyData.Rent[0] * Cons.ColorSetMultiplier);
            card.Q<VisualElement>("one-house-rent").Q<Label>("value").text = ToPrice(propertyData.Rent[1]);
            card.Q<VisualElement>("two-houses-rent").Q<Label>("value").text = ToPrice(propertyData.Rent[2]);
            card.Q<VisualElement>("three-houses-rent").Q<Label>("value").text = ToPrice(propertyData.Rent[3]);
            card.Q<VisualElement>("four-houses-rent").Q<Label>("value").text = ToPrice(propertyData.Rent[4]);
            card.Q<VisualElement>("hotel-rent").Q<Label>("value").text = ToPrice(propertyData.Rent[5]);
            card.Q<VisualElement>("house-cost").Q<Label>("value").text = ToPrice(propertyData.HouseCost);
            card.Q<VisualElement>("hotel-cost").Q<Label>("value").text = ToPrice(propertyData.HouseCost * Cons.HotelCostMultiplier);

            return card;
        }

        private static VisualElement MakeStationCard(StationData stationData)
        {
            var card = new VisualElement();
            _instance.stationCardTemplate.CloneTree(card);
            
            card.Q<Label>("name").text = stationData.Name;

            card.Q<VisualElement>("one-station").Q<Label>("value").text = ToPrice(Cons.StationsRent[0]);
            card.Q<VisualElement>("two-stations").Q<Label>("value").text = ToPrice(Cons.StationsRent[1]);
            card.Q<VisualElement>("three-stations").Q<Label>("value").text = ToPrice(Cons.StationsRent[2]);
            card.Q<VisualElement>("four-stations").Q<Label>("value").text = ToPrice(Cons.StationsRent[3]);

            return card;
        }

        private static VisualElement MakeUtilityCard(UtilityData utilityData)
        {
            var card = new VisualElement();
            _instance.utilityCardTemplate.CloneTree(card);
            
            card.Q<Label>("name").text = utilityData.Name;
            return card;
        }

        /// <summary>
        /// Helper function to format a number to represent a price.
        /// </summary>
        /// <param name="number">Value to format.</param>
        /// <returns>String in the format "$ {value}"</returns>
        private static string ToPrice(int number)
        {
            return $"$ {number}";
        }
    }
}
