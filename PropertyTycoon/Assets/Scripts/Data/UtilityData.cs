namespace Data
{
    /// <summary>
    /// Specialized OwnableData for Utility squares.
    /// Stores the rent base rent of the property and others for ownable squares, name, cost etc.
    /// </summary>
    public class UtilityData : OwnableData
    {
        public int[] Rent { get; }

        public UtilityData(string name, int cost, int[] rent) : base(name, cost)
        {
            this.Rent = rent;
        }
    }
}