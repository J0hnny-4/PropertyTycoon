namespace Data
{
    /// <summary>
    /// Specialized OwnableData for property squares.
    /// Stores the rent, colour, house cost and number of houses on the property.
    /// </summary>
    public class PropertyData : OwnableData
    {
        public int[] Rent { get; }
        public Colour Colour { get; }
        public int HouseCost { get; }
        public int Houses { get; set; } = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Property Name.</param>
        /// <param name="cost">Cost to purchase.</param>
        /// <param name="rent">Array of rents at levels of set ownership and houses.</param>
        /// <param name="colour">Colour set of property.</param>
        /// <param name="houseCost">Cost to build a house or hotel.</param>
        public PropertyData(string name, int cost, int[] rent, Colour colour, int houseCost) : base(name, cost)
        {
            this.Rent = rent;
            this.Colour = colour;
            this.HouseCost = houseCost;
        }
    }
}