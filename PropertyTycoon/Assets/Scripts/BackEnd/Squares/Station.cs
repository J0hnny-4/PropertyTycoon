using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Data;
using UI.Game;

namespace BackEnd.Squares
{
    /// <summary>
    /// Stations are an ownable square that charge rent based on the number of stations owned by the player.
    /// </summary>
    public class Station : Ownable
    {
        public Station(StationData data) : base(data) { }

        /// <summary>
        /// Counts the number of stations owned by the player and charges rent based on that number.
        /// </summary>
        protected override async Task ChargeRent()
        {
            Debug.Assert(Owner != null, nameof(Owner) + " != null");
            var owner = GameState.Players[(int)Owner];
            var ownedProperties = owner.Properties;
            
            var noOfStationsOwned = ownedProperties.Count(tileNo => GameState.Board[tileNo] is StationData);
            var amountOwed = Cons.StationsRent[noOfStationsOwned - 1]; // - 1 to account for 0 indexed array
            
            await DialogBoxFactory.PaymentDialogBox(Data, amountOwed).AsTask();
            var amountPaid = GameState.ActivePlayer.TakeMoney(amountOwed);
            owner.AddMoney(amountPaid);
        }
    }
}