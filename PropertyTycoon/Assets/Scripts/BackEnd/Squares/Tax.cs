using System.Threading.Tasks;
using Data;
using UI.Game;

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
            if (GameState.ActivePlayer.IsAi)
            {
                await DialogBoxFactory.AIDialogBox("Tax", $"You have been charged ${Amount}.").AsTask();
            }       
            else
            {
                await DialogBoxFactory.PaymentDialogBox(Data, Amount).AsTask();
            }
            GameState.FreeParkingMoney += GameState.ActivePlayer.TakeMoney(Amount);
        }
    }
}