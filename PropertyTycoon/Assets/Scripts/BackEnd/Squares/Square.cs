using Data;

namespace BackEnd.Squares
{
    /// <summary>
    /// Holds data via the SquareData class.
    /// Handles the actions to take when a player lands on the square.
    /// Buy default the square has no effect, can be used for Go, Jail etc.
    /// </summary>
    public class Square
    {
        protected SquareData Data { get; }

        public Square(SquareData data)
        {
            this.Data = data;
        }

        /// <summary>
        /// Action to take when a player lands on this square.
        /// Base class is a blank square such as Go with no effect
        /// </summary>
        public virtual void PlayerLands()
        {
        }
    }
}