public class Square
{
    private string name { get; }

    public Square(string name)
    {
        this.name = name;
    }
    
    /// <summary>
    /// Action to take when a player lands on this square.
    /// Base class is a blank square such as Go with no effect
    /// </summary>
    public virtual void playerLands() { }
}