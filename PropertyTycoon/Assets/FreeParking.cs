/// <summary>
/// Gives the player the money collected on free parking upon landing.
/// Free parking money is stored in the GameState class.
/// </summary>
public class FreeParking : Square
{
    public FreeParking(string name) : base(name) { }

    /// <summary>
    /// Gives the money in free parking to the player who lands on it.
    /// </summary>
    public override void playerLands()
    {
        GameState.currentPlayer.addMoney(GameState.freeParkingMoney);
        GameState.freeParkingReset();
    }
}