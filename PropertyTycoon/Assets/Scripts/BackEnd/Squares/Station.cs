using Data;

namespace BackEnd.Squares
{
    /// <summary>
    /// Stations are an ownable square that charge rent based on the number of stations owned by the player.
    /// </summary>
    public class Station : Ownable
    {
        private int Rent { get; }

        public Station(StationData data) : base(data) { }

        /// <summary>
        /// Counts the number of stations owned by the player and charges rent based on that number.
        /// </summary>
        protected override void ChargeRent()
        {
            // TODO decouple
            // var noOfStationsOwned = 0;
            // foreach (var i in Owner.properties) 
            //     if (i is Station)
            //         noOfStationsOwned++;
            // int money = GameState.ActivePlayer.payMoney(Rent * noOfStationsOwned);
            // Owner.addMoney(money);
        }
    }
}