/// <summary>
/// Upon landing the player is sent to jail.
/// </summary>
public class GoToJail : Square
{
    public GoToJail(string name) : base(name) { }
    
    /// <summary>
    /// Sends the player to jail.
    /// </summary>
    public override void playerLands()
    {
        GameState.currentPlayer.goToJail();
    }
}