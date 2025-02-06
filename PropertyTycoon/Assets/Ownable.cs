public class Ownable : Square
{
    public int cost { get; }
    public bool mortgaged { get; private set; } = false;
    private Player owner = null;

    public Ownable(string name, int cost) : base(name)
    {
        this.cost = cost;
    }
}