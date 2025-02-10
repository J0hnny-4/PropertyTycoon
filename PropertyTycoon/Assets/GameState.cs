using System.Collections.Generic;

/// <summary>
/// The primary interface for interacting with the game.
/// Stores the current state of the games and controls the flow of game actions.
/// A singleton class, fields of the instance are accessible globally with static fields.
/// </summary>
public class GameState
{
    private int _activePlayerIndex = 0;
    private List<Player> _players = new List<Player>();
    private List<Square> _board = new List<Square>();
    private int _freeParkingMoney = 0;
    
    //TODO sort out names for static and non static fields

    private static GameState instance { get; set; } = new GameState();
    public static Player currentPlayer => instance._players[instance._activePlayerIndex];
    public static int freeParkingMoney
    {
        get => instance._freeParkingMoney;
        set => instance._freeParkingMoney += value;
    }
    private GameState()
    {
        //TODO replace placholder board with actual squares

        for (int i = 0; i < 40; i++)
        {
            if (i == 20) _board.Add(new FreeParking("Free Parking"));
            else if (i == 30) _board.Add(new GoToJail("Go To Jail"));
            else _board.Add(new Square("Square " + i));
        }

        _players.Add(new HumanPlayer("Player 1", "Hat"));
        _players.Add(new HumanPlayer("Player 2", "Car"));
    }
    public static void freeParkingReset() { instance._freeParkingMoney = 0; }

    public static List<Square> board => instance._board;
    public static int boardSize => instance._board.Count;

    public static void newPlayer(Player player)
    {
        instance._players.Add(player);
    }

    public static void newGame()
    {
        instance = new GameState();
    }
}