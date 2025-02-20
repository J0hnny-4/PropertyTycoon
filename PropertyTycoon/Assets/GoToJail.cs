public class GoToJail : Square
{
    public GoToJail(string name) : base(name) { }

    public override void playerLands()
    {
        GameState.currentPlayer.goToJail();
    }
}