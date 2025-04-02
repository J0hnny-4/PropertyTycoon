using System.Threading.Tasks;
using Data;
using UnityEngine;

namespace BackEnd.Squares
{
    /// <summary>
    /// Upon landing the player is sent to jail.
    /// </summary>
    public class GoToJail : Square
    {
        public GoToJail(SquareData data) : base(data)
        {
        }

        /// <summary>
        /// Sends the player to jail.
        /// </summary>
        public override async Task PlayerLands()
        {
            GameState.ActivePlayer.TurnsLeftInJail = 3; //TODO: Magic number
            GameState.ActivePlayer.Position = 10; //TODO: Magic number
        }
    }
}