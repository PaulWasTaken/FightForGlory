using System.Drawing;
using Game.BaseStructures.Enums;
using Game.Controllers;
using Game.Fighters;
using Game.GameInformation;
using Game.GameObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameTests
{
    [TestClass]
    public class GameObject_should
    {
        [TestMethod]
        public void TestMovementBehaviour()
        {
            var settings = new GameSettings(400, 200);
            var first = new Paladin("paladin", new PointF(50, 100));
            var second = new Skeleton("skeleton", new PointF(210, 100));
            var gameState = new GameState(first, second);
            var gc = new GameController(settings, gameState);
            var spear = new Spear(second.Body, false, PlayerNumber.SecondPlayer);
            var prevPos = spear.Size.X;
            gameState.GameObjects.Add(spear);
            gc.UpdateGameState();
            Assert.AreEqual(spear.Size.X, prevPos + spear.GetSpeed());
            for (var i = 0; i < 1000; i++)
                gc.UpdateGameState();
            Assert.AreEqual(gameState.GameObjects.Contains(spear), false);
        }
    }
}
