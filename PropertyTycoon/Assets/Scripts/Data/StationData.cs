namespace Data
{
    /// <summary>
    /// Specialized OwnableData for Station squares.
    /// Stores the rent base rent of the property and others for ownable squares, name, cost etc.
    /// </summary>
    public class StationData : OwnableData
    {
        public int Rent { get; }
        public StationData(string name, int cost, int rent) : base(name, cost)
        {
            this.Rent = rent;
        }
    }
}