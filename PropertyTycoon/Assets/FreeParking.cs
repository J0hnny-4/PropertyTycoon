public class FreeParking : Square
{
    public FreeParking(string name) : base(name) { }

    public override void playerLands()
    {
        GameState.currentPlayer.addMoney(GameState.freeParkingMoney);
        GameState.freeParkingReset();
    }
}