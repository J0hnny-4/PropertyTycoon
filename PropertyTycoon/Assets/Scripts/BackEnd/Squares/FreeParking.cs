using System.Threading.Tasks;
using Data;
using UI.Game;

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
            await DialogBoxFactory.AIDialogBox(
                "Free Parking",
                $"You landed on free parking! You collect {GameState.FreeParkingMoney}.").AsTask();
            GameState.ActivePlayer.AddMoney(GameState.FreeParkingMoney);
            GameState.FreeParkingReset();
        }
    }
}