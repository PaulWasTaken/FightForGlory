using System.Drawing;
using Game.Controllers;
using Game.Fighters;
using Game.GameInformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameTests
{
    [TestClass]
    public class CombatController_should
    {
        [TestMethod]
        public void TestNoOneInRange()
        {
            var settings = new GameSettings(600, 600);
            var first = new Paladin("paladin", new PointF(200, 200));
            var second = new Skeleton("skeleton", new PointF(400, 200));
            var gameState = new GameState(first, second);
            var cc = new CombatController(first, second);

            first.Attack();
            second.Attack();
            cc.CheckForCombat(gameState);
            Assert.AreEqual(second.HealthPoints, 100);
            Assert.AreEqual(first.HealthPoints, 100);

            first.Move(50, second);
            second.Move(-50, first);
            first.Attack();
            second.Attack();
            cc.CheckForCombat(gameState);
            Assert.AreEqual(second.HealthPoints, 100);
            Assert.AreEqual(first.HealthPoints, 100);
        }

        [TestMethod]
        public void TestOneInRange()
        {
            //You need to change range setter.
        }

        [TestMethod]
        public void TestBothInRange()
        {
            var settings = new GameSettings(600, 600);
            var first = new Paladin("paladin", new PointF(280, 200));
            var second = new Skeleton("skeleton", new PointF(320, 200));
            var gameState = new GameState(first, second);
            var cc = new CombatController(first, second);

            first.Attack();
            second.Attack();
            cc.CheckForCombat(gameState);
            Assert.AreEqual(second.HealthPoints, 100 - first.AttackDamage);
            Assert.AreEqual(first.HealthPoints, 100 - second.AttackDamage);

            first.Move((int)-(second.AttackRange - (second.Body.X - first.Body.X) - 1), second);

            second.Attack();
            cc.CheckForCombat(gameState);
            Assert.AreEqual(first.HealthPoints, 100 - second.AttackDamage);
        }

        [TestMethod]
        public void TestCameOutOfRange()
        {
            var settings = new GameSettings(600, 600);
            var first = new Paladin("paladin", new PointF(280, 200));
            var second = new Skeleton("skeleton", new PointF(320, 200));
            var gameState = new GameState(first, second);
            var cc = new CombatController(first, second);

            first.Attack();
            second.Attack();
            cc.CheckForCombat(gameState);
            Assert.AreEqual(second.HealthPoints, 100 - first.AttackDamage);
            Assert.AreEqual(first.HealthPoints, 100 - second.AttackDamage);

            first.Move((int)-(second.AttackRange - (second.Body.X - first.Body.X) - 1), second);

            second.Attack();
            cc.CheckForCombat(gameState);
            Assert.AreEqual(first.HealthPoints, 100 - second.AttackDamage);

            second.Move((int)-(second.Body.X - first.Body.X - first.AttackRange + 1), first);
            first.Attack();
            cc.CheckForCombat(gameState);
            Assert.AreEqual(second.HealthPoints, 100 - first.AttackDamage);
        }
    }
}
