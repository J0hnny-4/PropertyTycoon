using System.Threading.Tasks;
using Data;
using UnityEngine;
using UI.Board;

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
            await GameState.ActivePlayer.GoToJail();//TODO: Magic number
        }
    }
}
