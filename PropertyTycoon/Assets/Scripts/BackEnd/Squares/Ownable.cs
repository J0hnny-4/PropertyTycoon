using System.Threading.Tasks;
using Data;
using UI.Game;
using Random = UnityEngine.Random;

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
                if (!player.IsAi)
                {
                    bought = await DialogBoxFactory.PurchaseDialogBox(Data as OwnableData).AsTask();
                }
                else
                {
                    var des = Random.Range(0, 10);
                    if (des >= 5)
                    {
                        bought = true;
                    }
                    else
                    {
                        bought = false;
                    }

                    await DialogBoxFactory.AIDialogBox("Ai Action ", bought ? "Bought property: " + Data.Name : "Ai didn't buy property " + Data.Name).AsTask();
                }

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
            if (winner == -1)
            {
                await DialogBoxFactory.AIDialogBox(
                    "Auction Failed", 
                    "No one bought the property.").AsTask();
                return;
            }

            // bid is guaranteed to be less than the amount held by the player
            var player = GameState.Players[winner];
            await DialogBoxFactory.AIDialogBox(
                "Auction Success", 
                $"{player.Name} won the auction!").AsTask();
            player.TakeMoney(bid);
            Owner = winner;
            player.AddProperty(Index);
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

        /// <summary>
        /// Resets the ownable to its original state.
        /// </summary>
        public virtual void Reset()
        {
            Owner = null;
            Mortgaged = false;
        }
    }
}
