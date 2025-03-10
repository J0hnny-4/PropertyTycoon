namespace Data
{
    /// <summary>
    /// Holds data for a square on the board.
    /// functionality is defined by the Square class.
    /// </summary>
    public class SquareData
    {
        public string Name { get; }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Displayed name of piece.</param>
        public SquareData(string name)
        {
            this.Name = name;
        }
    }
}