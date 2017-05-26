using Game.BaseStructures.ComboWorker;
using Game.BaseStructures.Enums;
using Game.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameTests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestTree()
        {
            var automat = new ComboDetector<Command>();
            automat.Add(new[] { Command.MoveLeft, Command.MoveRight, Command.NormalAttack }, ComboName.HolyLight);
            //automat.Add(new[] { Keys.B, Keys.B, Keys.C }, ComboName.HolyLight);
            //automat.Add(new[] { Keys.C, Keys.B, Keys.C }, ComboName.HolyLight);
            automat.FindValue(Command.MoveLeft);
            Assert.AreEqual(automat.CheckState(Command.MoveRight), false);
            Assert.AreEqual(automat.CheckState(Command.NormalAttack), true);
            Assert.AreEqual(automat.CurrentState.Value, "Default");
        }
    }
}
