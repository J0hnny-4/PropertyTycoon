using System.Threading.Tasks;
using Data;

namespace BackEnd.Squares
{
    /// <summary>
    /// Imposes a tax on the player landing on it.
    /// </summary>
    public class Tax : Square
    {
        private int Amount => (Data as TaxData).Amount;
        
        public Tax(TaxData data) : base(data) { }

        /// <summary>
        /// Requires the player to pay the tax amount.
        /// </summary>
        public override async Task PlayerLands()
        {
            GameState.ActivePlayer.TakeMoney(Amount);
        }
    }
}