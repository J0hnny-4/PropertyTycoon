namespace Data
{
    /// <summary>
    /// Specialized square data for ownable squares.
    /// Stores the cost of the square, the owner index and whether the square is mortgaged.
    /// </summary>
    public class OwnableData : SquareData
    {
        public int Cost { get; }
        public string Name { get; }
        public int? Owner { get; set; } = null;
        public bool Mortgaged { get; set; } = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Display name of the Square.</param>
        /// <param name="cost">Cost to purchase.</param>
        public OwnableData(string name, int cost) : base(name)
        {
            this.Cost = cost;
            this.Name = name;
        }
    }
}
