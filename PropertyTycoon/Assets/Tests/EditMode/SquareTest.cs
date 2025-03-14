using NUnit.Framework;
using Data;
using BackEnd;
using BackEnd.Squares;
using System.Collections.Generic;
using UnityEngine;


namespace Tests.EditMode
{
    public class SquareTest
    {
        public Property TestProperty;
        
        [SetUp]
        public void Setup()
        {
            GameState.NewGame();
            GameState.Players = new List<PlayerData>
            {
                new PlayerData("A", ScriptableObject.CreateInstance<Token>()),
                new PlayerData("B", ScriptableObject.CreateInstance<Token>()),
                new PlayerData("C", ScriptableObject.CreateInstance<Token>())
            };
            
            GameState.AddSquare(new SquareData("Go"));;
            GameState.AddSquare(new SquareData("Community Chest"));
            GameState.AddSquare(new SquareData("Income Tax"));
            GameState.AddSquare(new PropertyData("Old Kent Road", 60, new []{1,10,20,30,40}, "Brown", 100));
            
            TestProperty = new Property((PropertyData)GameState.Board[3]);
        }
        
        [Test]
        public void TestPropertySquare()
        {
            Assert.AreEqual(1, TestProperty.NumberInSet, "Number in set is incorrect.");
            int playerMoney = GameState.ActivePlayer.Money;
            TestProperty.PlayerLands();
            Assert.AreEqual(playerMoney - 60, GameState.ActivePlayer.Money, "Player money is incorrect.");
            Assert.AreEqual(1, GameState.ActivePlayer.Properties.Count, "Player properties is incorrect.");
            Assert.AreEqual(true, GameState.ActivePlayer.Properties.Contains(3), "Player properties is incorrect.");
        }
    }
}