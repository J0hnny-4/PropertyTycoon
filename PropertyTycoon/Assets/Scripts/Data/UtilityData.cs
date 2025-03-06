namespace Data
{
    /// <summary>
    /// Specialized OwnableData for Utility squares.
    /// Stores the rent base rent of the property and others for ownable squares, name, cost etc.
    /// </summary>
    public class UtilityData : OwnableData
    {
        public int[] Rent { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Display name of Square.</param>
        /// <param name="cost">Cost to purchase.</param>
        /// <param name="rent">Array of multipliers for dice roles to calculate rent charged.</param>
        public UtilityData(string name, int cost, int[] rent) : base(name, cost)
        {
            this.Rent = rent;
        }
    }
}