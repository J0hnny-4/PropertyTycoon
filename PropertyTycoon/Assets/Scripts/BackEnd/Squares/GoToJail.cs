using Data;

/// <summary>
/// Upon landing the player is sent to jail.
/// </summary>
public class GoToJail : Square
{
    public GoToJail(SquareData data) : base(data)
    {
    }

    /// <summary>
    /// Sends the player to jail.
    /// </summary>
    public override void PlayerLands()
    {
        //TODO: Implement sending the player to jail
    }
}