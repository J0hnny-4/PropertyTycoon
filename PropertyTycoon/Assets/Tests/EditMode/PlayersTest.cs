using NUnit.Framework;
using Data;
using BackEnd;
using System.Collections.Generic;
using UnityEngine;

namespace Tests.EditMode
{
    public class PlayersTest
    {
        public Player TestPlayer;

        [SetUp]
        public void Setup()
        {
            GameState.NewGame();
            List<PlayerData> players = new List<PlayerData>
            {
                new PlayerData("A", ScriptableObject.CreateInstance<Token>()),
                new PlayerData("B", ScriptableObject.CreateInstance<Token>()),
                new PlayerData("C", ScriptableObject.CreateInstance<Token>())
            };
            GameState.Players = players;
            
            GameState.AddSquare(new SquareData("Go"));;
            GameState.AddSquare(new SquareData("Community Chest"));
            GameState.AddSquare(new SquareData("Income Tax"));
            GameState.AddSquare(new SquareData("Chance"));
            GameState.AddSquare(new SquareData("Jail"));
            GameState.AddSquare(new SquareData("Free Parking"));
            
            TestPlayer = new HumanPlayer(GameState.Players[0]);
        }
        
        [Test]
        public void TestDice()
        {
            int diceRoll = TestPlayer.RollDice();
            Assert.Greater(diceRoll, 1, "Dice roll is too low.");
            Assert.Less(diceRoll, 13, "Dice roll is too high.");
            
            Assert.AreEqual(GameState.Players[0].LastRoll.Item1 + GameState.Players[0].LastRoll.Item2, diceRoll, "Dice roll is not saved correctly.");
        }
        
        [Test]
        public void TestMoney()
        {
            int balance = GameState.Players[0].Money;
            TestPlayer.Data.AddMoney(200);
            Assert.AreEqual(balance + 200, GameState.Players[0].Money, "Money was not added correctly.");
        }
    }
}