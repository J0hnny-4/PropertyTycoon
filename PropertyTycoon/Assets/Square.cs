/// <summary>
/// Holds the data for a square on the board.
/// Handles the actions to take when a player lands on the square.
/// Buy default the square has no effect, can be used for Go, Jail etc.
/// </summary>
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