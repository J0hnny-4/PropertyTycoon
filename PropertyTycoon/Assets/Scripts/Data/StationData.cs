namespace Data
{
    /// <summary>
    /// Specialized OwnableData for Station squares.
    /// Stores the rent base rent of the property and others for ownable squares, name, cost etc.
    /// </summary>
    public class StationData : OwnableData
    {
        public int Rent { get; }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Displayed name of square.</param>
        /// <param name="cost">Cost to purchase.</param>
        /// <param name="rent">Base rent charged before multipliers.</param>
        public StationData(string name, int cost, int rent) : base(name, cost)
        {
            this.Rent = rent;
        }
    }
}