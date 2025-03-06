/// <summary>
/// Specialized square data for ownable squares.
/// Stores the cost of the square, the owner index and whether the square is mortgaged.
/// </summary>
public class OwnableData : SquareData
{
    public int cost { get; }
    public int? owner { get; set; } = null;
    public bool mortgaged { get; set; } = false;
    public OwnableData(string name, int cost) : base(name)
    {
        this.cost = cost;
    }
}