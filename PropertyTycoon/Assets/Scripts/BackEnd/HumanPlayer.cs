using Data;

namespace BackEnd
{
    /// <summary>
    /// A human player that selects a piece and makes moves manually.
    /// Choices are made through menu options.
    /// Game rules function normaly.
    /// </summary>
    public class HumanPlayer : Player
    {
        public HumanPlayer(PlayerData data) : base(data)
        {
        }
    }
}