using System.Threading.Tasks;
using Data;

namespace BackEnd.Squares
{
    /// <summary>
    /// Gives the player the money collected on free parking upon landing.
    /// Free parking money is stored in the GameState class.
    /// </summary>
    public class FreeParking : Square
    {
        public FreeParking(SquareData data) : base(data)
        {
        }

        /// <summary>
        /// Gives the money in free parking to the player who lands on it.
        /// </summary>
        public override async Task PlayerLands()
        {
            GameState.ActivePlayer.AddMoney(GameState.FreeParkingMoney);
            GameState.FreeParkingReset();
        }
    }
}