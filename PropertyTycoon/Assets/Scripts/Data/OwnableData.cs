namespace Data
{
    /// <summary>
    /// Specialized square data for ownable squares.
    /// Stores the cost of the square, the owner index and whether the square is mortgaged.
    /// </summary>
    public class OwnableData : SquareData
    {
        public int Cost { get; }
        public int? Owner { get; set; } = null;
        public bool Mortgaged { get; set; } = false;

        public OwnableData(string name, int cost) : base(name)
        {
            this.Cost = cost;
        }
    }
}