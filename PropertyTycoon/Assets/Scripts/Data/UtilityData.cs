namespace Data
{
    /// <summary>
    /// Specialized OwnableData for Utility squares.
    /// Stores the rent base rent of the property and others for ownable squares, name, cost etc.
    /// </summary>
    public class UtilityData : OwnableData
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Display name of Square.</param>
        /// <param name="cost">Cost to purchase.</param>
        public UtilityData(string name, int cost) : base(name, cost) { }
    }
}