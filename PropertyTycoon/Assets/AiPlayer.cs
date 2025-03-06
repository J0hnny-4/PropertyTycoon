/// <summary>
/// An AI player that selects a piece and makes moves automatically.
/// Has the option to override default player methods to "cheat" ie, change dice rolls.
/// </summary>
public class AiPlayer : Player
{
    // TODO Make AI select unused piece
    public AiPlayer(string name, string piece) : base(name, piece) { }
}