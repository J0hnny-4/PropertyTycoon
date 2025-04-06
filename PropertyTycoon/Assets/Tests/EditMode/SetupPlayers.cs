using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using NUnit.Framework;
using UI.Menu;

namespace Tests.EditMode
{
    public class SetupPlayers
    {
        private List<string> _names;
        private PlayerSetupController _controller;

        [SetUp]
        public void Setup()
        {
            _names = new List<string>() { "A", "B", "C", "D", "E", "F", "G" };
            _controller = new PlayerSetupController(_names);
            _controller.InitialisePlayers();
        }
        
        [Test]
        public void TestInitialState()
        {
            Assert.AreEqual(Cons.MinPlayers, _controller.PlayersCount, "Initialised to wrong player count.");
            var tokens = _controller.GetAllTokens();
            Assert.IsNotNull(tokens, "Cannot get tokens from resource.");
            Assert.IsTrue(tokens.Length >= Cons.MaxPlayers, "The tokens available are less than the max number of players.");
        }

        [Test]
        public void TestAddPlayer()
        {
            // setup
            var eventFired = false;
            PlayerData newPlayer = null;
            _controller.OnPlayerAdded += (p) =>
            {
                eventFired = true;
                newPlayer = p;
            };
            
            _controller.AddPlayer();
            Assert.AreEqual(Cons.MinPlayers + 1, _controller.PlayersCount, "Players count is incorrect.");
            Assert.IsTrue(eventFired, "Event was not fired.");
            Assert.IsNotNull(newPlayer, "New player is not passed correctly.");
            Assert.IsTrue(_controller.GetPlayers().Contains(newPlayer), "Player was not added to the list.");
        }

        [Test]
        public void TestRemovePlayer()
        {
            // add dummy player and get reference to random player
            _controller.AddPlayer();
            var player = _controller.GetPlayers()[0];
            
            // setup
            var eventFired = false;
            PlayerData removedPlayer = null;
            _controller.OnPlayerRemoved += (p) =>
            {
                eventFired = true;
                removedPlayer = p;
            };
            
            _controller.RemovePlayer(player);
            Assert.AreEqual( Cons.MinPlayers, _controller.PlayersCount, "Players count is incorrect.");
            Assert.IsTrue(eventFired, "Event was not fired.");
            Assert.AreEqual(player, removedPlayer, "Removed player is not passed correctly.");
            Assert.IsFalse(_controller.GetPlayers().Contains(removedPlayer), "Player was not removed from list.");
        }

        [Test]
        public void TestCannotGoBelowMinimumPlayers()
        {
            // currently already at minimum players required
            Assert.IsFalse(_controller.CanRemovePlayer, "Shouldn't be able to remove any more players.");
            Assert.Throws<InvalidOperationException>(() => _controller.RemovePlayer(null));
        }

        [Test]
        public void TestCannotGoAboveMaximumPlayers()
        {
            // currently already at minimum players required
            for (var i = Cons.MinPlayers; i < Cons.MaxPlayers; i++) { _controller.AddPlayer(); }
            
            Assert.IsFalse(_controller.CanAddPlayer, "Shouldn't be able to add any more players.");
            Assert.Throws<InvalidOperationException>(() => _controller.AddPlayer());
        }

        [Test]
        public void TestSwitchToken()
        {
            if (_controller.GetAllTokens().Length < Cons.MinPlayers + 2)
            {
                Assert.Inconclusive("This test requires at least 2 extra tokens available.");
            }
            // grabs a token, then grabs a new token, making the first one available for use
            var tokenA = _controller.GetNextAvailableToken();
            var tokenB = _controller.GetNextAvailableToken(tokenA);
            
            Assert.IsFalse(_controller.GetUsedTokens().Contains(tokenA), "Token A should be available.");
            Assert.IsTrue(_controller.GetUsedTokens().Contains(tokenB), "Token B should not be available.");
        }

        [Test]
        public void TestCannotSwitchTokenWhenNoMoreAvailable()
        {
            // use all tokens
            var tokens = _controller.GetAllTokens();
            for (var i = Cons.MinPlayers; i < tokens.Length; i++) { _controller.GetNextAvailableToken(); }
            
            Assert.AreEqual(_controller.GetUsedTokens().Length, tokens.Length);
            Assert.IsFalse(_controller.CanSwitchToken, "Shouldn't be able to switch tokens.");
            Assert.Throws<InvalidOperationException>(() => _controller.GetNextAvailableToken());
        }

        [Test]
        public void TestNamesAddedAndRemoved()
        {
            // adding should remove name from list
            _controller.AddPlayer();
            var player = _controller.GetPlayers()[0];
            Assert.IsFalse(_names.Contains(player.Name), "Name should have been removed from the list.");
            
            // removing player should return name to the list
            _controller.RemovePlayer(player);
            Assert.IsTrue(_names.Contains(player.Name), "Name should have been returned to the list.");
        }
    }
}
