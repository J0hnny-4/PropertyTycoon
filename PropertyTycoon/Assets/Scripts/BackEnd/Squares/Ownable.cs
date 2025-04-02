using System;
using System.Threading.Tasks;
using Codice.Client.BaseCommands;
using Data;
using UI.Game;
using UnityEngine;

namespace BackEnd.Squares
{
    /// <summary>
    /// Extension of the Square class that can be owned by a player.
    /// Provides the interface for charging rent, buy must be implemented by the subclass.
    /// </summary>
    public abstract class Ownable : Square
    {
        protected int Cost => (Data as OwnableData).Cost;

        private int? _index;// Cache the index of the property on the board.
        protected int Index
        {
            get
            {
                if (_index == null)
                {
                    _index = GameState.Board.IndexOf(Data);
                }
                return _index.Value;
            }
        }

        protected bool Mortgaged
        {
            get => (Data as OwnableData).Mortgaged;
            set => (Data as OwnableData).Mortgaged = value;
        }

        protected int? Owner
        {
            get => (Data as OwnableData).Owner;
            set => (Data as OwnableData).Owner = value;
        }


        protected Ownable(OwnableData data) : base(data)
        {
        }

        /// <summary>
        /// Gives the player the option to buy the property, otherwise it is auctioned. 
        /// </summary>
        private async Task Buy()
        {
            var player = GameState.ActivePlayer;

            // player is prompted if they have enough money to buy it
            var bought = false;
            if (player.Money > Cost)
            {
                bought = await DialogBoxFactory.PurchaseDialogBox(Data as OwnableData).AsTask();
            }
            
            if (bought) // if player bought the property, assign them as owners
            {
                GameState.ActivePlayer.TakeMoney(Cost);
                Owner = GameState.ActivePlayerIndex;
                GameState.ActivePlayer.AddProperty(Index);
            }
            else // else auction it
            {
                await Auction();
            }
        }

        /// <summary>
        /// Auctions off the property to all the players.
        /// </summary>
        private async Task Auction()
        {
            var (winner, bid) = await DialogBoxFactory.AuctionDialogBox(Data as OwnableData).AsTask();

            // -1 represents auction being skipped
            if (winner == -1) { return; }

            // bid is guaranteed to be less than the amount held by the player
            GameState.Players[winner].TakeMoney(bid);
            Owner = winner;
            GameState.Players[winner].AddProperty(Index);
        }

        /// <summary>
        /// Adds the functionality to purchase the property or charge rent on landing
        /// </summary>
        public override async Task PlayerLands()
        {
            if (Owner == null) await Buy();
            else if (Owner != GameState.ActivePlayerIndex && !Mortgaged) await ChargeRent();
        }

        /// <summary>
        /// Charge the landing player rent for landing on the property.
        /// Must be implemented by the subclass.
        /// </summary>
        protected abstract Task ChargeRent();

        /// <summary>
        /// Sets the property to mortgaged and gives the owner half the cost of the property.
        /// Rent cannot be charged on mortgaged properties.
        /// </summary>
        public void Mortgage()
        {
            Mortgaged = true;
            // owner.addMoney(cost / 2); //TODO decouple from player
        }
    }
}