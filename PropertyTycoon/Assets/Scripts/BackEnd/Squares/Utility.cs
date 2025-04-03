using System;
using System.Linq;
using System.Threading.Tasks;
using Data;
using UI.Game;
using UnityEngine;

namespace BackEnd.Squares
{
    /// <summary>
    /// And ownable square that cheges rent based on the last dice roll and the number of utilities owned by the player.
    /// </summary>
    public class Utility : Ownable
    {
        public Utility(UtilityData data) : base(data) { }

        protected override async Task ChargeRent()
        {
            
            Debug.Assert(Owner != null, nameof(Owner) + " != null");
            var owner = GameState.Players[(int)Owner];
            var ownedProperties = owner.Properties;
            
            var noOfUtilitiesOwned = ownedProperties.Count(tileNo => GameState.Board[tileNo] is UtilityData);
            var multiplier = Cons.UtilitiesMultiplier[noOfUtilitiesOwned] - 1; // - 1 to account for 0 indexed array
            var amountOwed = multiplier * (GameState.ActivePlayer.LastRoll.Item1 + GameState.ActivePlayer.LastRoll.Item2);
            
            await DialogBoxFactory.PaymentDialogBox(Data, amountOwed).AsTask();
            var amountPaid = GameState.ActivePlayer.TakeMoney(amountOwed);
            owner.AddMoney(amountPaid);
        }
    }
}