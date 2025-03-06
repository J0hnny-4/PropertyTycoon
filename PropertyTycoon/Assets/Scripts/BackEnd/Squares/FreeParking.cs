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
        public override void PlayerLands()
        {
            // GameState.activePlayer.addMoney(GameState.freeParkingMoney); TODO: Implement addMoney
            GameState.FreeParkingReset(); // Maybe con troll via GameRunner?
        }
    }
}