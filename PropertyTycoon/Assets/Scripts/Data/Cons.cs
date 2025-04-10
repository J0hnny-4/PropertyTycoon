using System;

namespace Data
{
    /// <summary>
    /// A place for global variables.
    /// </summary>
    public class Cons
    {
        private const bool DevMode = false;
        
        public static TimeSpan TimeLimit = DevMode ? TimeSpan.FromMinutes(1) : TimeSpan.FromHours(1); // default abridged value
        public const int AIDialogBoxDelay = DevMode ? 500 : 3000; // milliseconds
        public const float PlayerWait = DevMode ? 0.1f : 0.5f; // seconds 
        public const int StartingMoney = 1500;
        public const int MinPlayers = 2;
        public const int MaxPlayers = 6;

        public const int ColorSetMultiplier = 2;
        public const int HotelCostMultiplier = 5;
        public const int IncomeTax = 200;
        public const int SuperTax = 400;
        public const int DoublesToJail = 2;
        public static readonly int[] UtilitiesMultiplier = { 4, 10 };
        public static readonly int[] StationsRent = { 25, 50, 100, 200 };
    }
}
