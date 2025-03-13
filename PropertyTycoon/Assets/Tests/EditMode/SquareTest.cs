using NUnit.Framework;
using Data;
using BackEnd;
using BackEnd.Squares;


namespace Tests.EditMode
{
    public class SquareTest
    {
        public Property TestProperty;
        
        [SetUp]
        public void Setup()
        {
            GameState.AddSquare(new SquareData("Go"));;
            GameState.AddSquare(new SquareData("Community Chest"));
            GameState.AddSquare(new SquareData("Income Tax"));
            GameState.AddSquare(new PropertyData("Old Kent Road", 60, new []{1,10,20,30,40}, "Brown", 100));
            
            TestProperty = new Property((PropertyData)GameState.Board[3]);
        }
    }
}