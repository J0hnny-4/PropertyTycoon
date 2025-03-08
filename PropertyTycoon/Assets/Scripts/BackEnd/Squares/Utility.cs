using System;
using Data;

namespace BackEnd.Squares
{
    /// <summary>
    /// And ownable square that cheges rent based on the last dice roll and the number of utilities owned by the player.
    /// </summary>
    public class Utility : Ownable
    {
        private int[] Rent { get; }

        public Utility(UtilityData data) : base(data) { }

        protected override void ChargeRent()
        {
            //TODO Decouple
            // var noOfUtilitiesOwned = 0;
            // foreach (var i in Owner.properties)
            //     if (i is Station)
            //         noOfUtilitiesOwned++;
            // noOfUtilitiesOwned = Math.Min(noOfUtilitiesOwned, Rent.Length) - 1;
            // var rentOwed = Rent[noOfUtilitiesOwned] *
            //                (GameState.ActivePlayer.LastRoll.Item1 + GameState.ActivePlayer.LastRoll.Item2);
            // int money = GameState.ActivePlayer.payMoney(rentOwed);
            // Owner.addMoney(money);
        }
    }
}