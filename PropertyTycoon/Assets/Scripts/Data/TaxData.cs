namespace Data
{
    /// <summary>
    /// Data about the Tax square. Indicate the amount to pay.
    /// </summary>
    public class TaxData : SquareData
    {
        public int Amount {  get; }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Displayed name of square.</param>
        /// <param name="amount">Taxed amount.</param>
        public TaxData(string name, int amount) : base(name)
        {
            Amount = amount;
        }
    }
}