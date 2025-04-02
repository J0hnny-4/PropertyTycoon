using System;

namespace Data
{
    /// <summary>
    /// A place for global variables.
    /// </summary>
    public class Cons
    {
        public static TimeSpan TimeLimit = TimeSpan.FromHours(1); // default value for testing
        public const int StartingMoney = 500;
        public const int ColorSetMultiplier = 2;
        public const int HotelCostMultiplier = 5;
        public static readonly int[] UtilitiesMultiplier = { 4, 10 };
        public static readonly int[] StationsRent = { 25, 50, 100, 200 };
    }
}