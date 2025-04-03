using System.Threading.Tasks;
using Data;
using UI.Game;
using UnityEngine;

namespace BackEnd.Squares
{
    /// <summary>
    /// The coloured properties on the board that can be bought and sold.
    /// They are own-able, rent can be charged, and houses/hotels can be built.
    /// Features are unlocked when a player owns all properties of the same colour.
    /// </summary>
    public class Property : Ownable
    {
        public int[] Rent => (Data as PropertyData).Rent;
        public Colour Colour => (Data as PropertyData).Colour;
        public int HouseCost => (Data as PropertyData).HouseCost;

        public int Houses
        {
            get => (Data as PropertyData).Houses;
            private set => (Data as PropertyData).Houses = value;
        }

        public Property(PropertyData data) : base(data)
        {
        }

        /// <summary>
        /// Uses the GameState to count the number of properties in the set.
        /// </summary>
        /// <returns>The number of properties that share a colour with this one.</returns>
        public int NumberInSet
        {
            get
            {
                var count = 0;
                foreach (var i in GameState.Board)
                    if (i is PropertyData p && p.Colour == Colour)
                        count++;
                return count;
            }
        }

        /// <summary>
        /// Calculates and charges rent based on the number of properties in the set and the number of houses on the property.
        /// </summary>
        protected override async Task ChargeRent()
        {
            Debug.Assert(Owner != null, nameof(Owner) + " != null");
            
            var owner = GameState.Players[(int)Owner];
            var rentDue = Rent[Houses];
            if (Houses == 0 && OwnerHasSet) rentDue *= Cons.ColorSetMultiplier;
            
            // todo add bankruptcy state
            // gets current player and charges them
            await DialogBoxFactory.PaymentDialogBox(Data, rentDue).AsTask();
            var amountPaid = GameState.ActivePlayer.TakeMoney(rentDue);
            owner.AddMoney(amountPaid);
        }

        /// <summary>
        /// Allows the player to buy a house/hotel on the property if they own all properties in the set.
        /// The difference between number of houses on a property should never be more than 1.
        /// Hotels are considered to be 5 houses.
        /// </summary>
        /// //TODO Do this properly
        public void BuyHouse()
        {
            if (Houses <= 4
                && Owner == GameState.ActivePlayerIndex
                && OwnerHasSet)
                // GameState.activePlayer.payMoney(houseCost); // TODO decouple from player
                Houses++;
        }

        //TODO Do this properly too
        public void SellHouse()
        {
            if (Houses > 0 && Owner == GameState.ActivePlayerIndex)
                // GameState.activePlayer.addMoney(houseCost / 2); // TODO decouple from player
                Houses--;
        }

        public bool OwnerHasSet
        {
            get
            {
                var count = 0;
                foreach (var i in GameState.Board)
                    if (i is PropertyData p && p.Colour == Colour && p.Owner == Owner)
                        count++;
                return count == NumberInSet;
            }
        }

        public override void Reset()
        {
            base.Reset();
            Houses = 0;
        }
    }
}