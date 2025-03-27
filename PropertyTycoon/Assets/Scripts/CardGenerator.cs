using NUnit.Framework;
using System.Collections.Generic;
using BackEnd;

namespace DefaultNamespace
{
    
    public static class CardGenerator
    {
        public static Dictionary<string, List<Card>> GenerateCards(string path = null) 
        {
            var decks = new Dictionary<string, List<Card>>();
            decks["PotLuck"] = new List<Card>();
            decks["OpportunityKnocks"] = new List<Card>();
            
            //Advance to square
            string squareToAdvanceTo = "Go";
            decks["PotLuck"].Add(new Card("PotLuck", "Advance to Go", () =>
            {
                while(GameState.Board[GameState.Players[GameState.ActivePlayerIndex].Position].Name != squareToAdvanceTo)
                {
                    GameState.Players[GameState.ActivePlayerIndex].Position++;
                    if (GameState.Players[GameState.ActivePlayerIndex].Position >= GameState.Board.Count)
                    {
                        GameState.Players[GameState.ActivePlayerIndex].Position -= GameState.Board.Count;
                        Banker.PayPlayer(null, GameState.ActivePlayerIndex, 200); //TODO magic number
                    }
                }
            }));
            
            //Go back to square
            string squareToGoBackTo = "Go";
            decks["PotLuck"].Add(new Card("PotLuck", "Advance to Go", () =>
            {
                while(GameState.Board[GameState.Players[GameState.ActivePlayerIndex].Position].Name != squareToGoBackTo)
                {
                    GameState.Players[GameState.ActivePlayerIndex].Position++;
                    GameState.Players[GameState.ActivePlayerIndex].Position %= GameState.Board.Count;
                }
            }));
            
            //Bank Pays player
            int amount = 200;
            decks["PotLuck"].Add(new Card("PotLuck", "You inherit £200", () =>
            {
                Banker.PayPlayer(null, GameState.ActivePlayerIndex, amount);
            }));
            
            
            
            

            return decks;
        }
    }
}