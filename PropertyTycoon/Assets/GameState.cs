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

    /// <summary>
    /// Stores the single instance of GameState.
    /// This should autofill when any field is accessed.
    /// </summary>
    private static GameState instance { get; set; } = new GameState();
    public static Player currentPlayer => instance._players[instance._activePlayerIndex];
    public static int freeParkingMoney
    {
        get => instance._freeParkingMoney;
        set => instance._freeParkingMoney += value;
    }
    
    /// <summary>
    /// Sets up the board and players for a new game.
    /// Currently, creates a placeholder board and two placeholder players.
    /// </summary>
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
    
    /// <summary>
    /// Restest the free parking money to 0.
    /// Used buy the free parking square when a player lands on it.
    /// </summary>
    public static void freeParkingReset() { instance._freeParkingMoney = 0; }

    public static List<Square> board => instance._board;
    public static int boardSize => instance._board.Count;

    /// <summary>
    /// Adds a player to the game.
    /// Does not affect other players already added
    /// </summary>
    /// <param name="player">The player object to add to the lsit of players</param>
    public static void newPlayer(Player player)
    {
        instance._players.Add(player);
    }

    /// <summary>
    /// Resets the singleton instance to a new GameState object.
    /// </summary>
    public static void newGame()
    {
        instance = new GameState();
    }
}