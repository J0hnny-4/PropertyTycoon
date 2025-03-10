using System;
using Data;

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
        private void Buy()
        {
            //TODO menu options
            if (true)
            {
                GameState.ActivePlayer.Money -= Cost;
                Owner = GameState.ActivePlayerIndex;
                GameState.ActivePlayer.Properties.Add(Index);
            }
            else
            {
                Auction();
            }
        }

        /// <summary>
        /// Auctions off the property to all the players.
        /// </summary>
        private void Auction()
        {
            //TODO Auction should return the player who won the auction
            var winner = GameState.ActivePlayerIndex;
            Owner = winner;
            GameState.
        }

        /// <summary>
        /// Adds the functionality to purchase the property or charge rent on landing
        /// </summary>
        public override void PlayerLands()
        {
            if (Owner == null) Buy();
            else if (Owner != GameState.ActivePlayerIndex && !Mortgaged) ChargeRent();
        }

        /// <summary>
        /// Charge the landing player rent for landing on the property.
        /// Must be implemented by the subclass.
        /// </summary>
        protected abstract void ChargeRent();

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