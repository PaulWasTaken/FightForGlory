using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using Game.BaseStructures.Enums;
using Game.Commands;
using Game.Controllers;
using Game.GameObjects;
using NUnit.Framework;

namespace GameTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestIfShouldPerform()
        {
            var automat = new ComboController(new Dictionary<ComboName, Func<GameObject>> { {ComboName.HolyLight,
                    () => new Wisp(new RectangleF(), false, PlayerNumber.FirstPlayer)} });
            automat.AddCombo(new[] { Command.MoveLeft, Command.MoveRight, Command.NormalAttack }, ComboName.HolyLight);
            automat.UpdateState(Command.MoveLeft);
            automat.UpdateState(Command.MoveRight);

            automat.ComboCompleted.Should().Be(false);

            automat.UpdateState(Command.NormalAttack);

            automat.ComboCompleted.Should().Be(true);
        }

        [Test]
        public void TestIfShouldNotPerform()
        {
            var automat = new ComboController(new Dictionary<ComboName, Func<GameObject>> { {ComboName.HolyLight,
                    () => new Wisp(new RectangleF(), false, PlayerNumber.FirstPlayer)} });
            automat.AddCombo(new[] { Command.MoveLeft, Command.MoveRight, Command.NormalAttack }, ComboName.HolyLight);
            Assert.AreEqual(automat.ComboCompleted, false);
            automat.UpdateState(Command.MoveLeft);
            Assert.AreEqual(automat.ComboCompleted, false);
            automat.UpdateState(Command.StrongAttack);
            Assert.AreEqual(automat.ComboCompleted, false);
            automat.UpdateState(Command.MoveLeft);
            Assert.AreEqual(automat.ComboCompleted, false);
            automat.UpdateState(Command.MoveRight);
            Assert.AreEqual(automat.ComboCompleted, false);
            automat.UpdateState(Command.Block);
            Assert.AreEqual(automat.ComboCompleted, false);
        }

        [Test]
        public void TestIfRightObjReturned()
        {
            var automat = new ComboController(new Dictionary<ComboName, Func<GameObject>> { {ComboName.HolyLight,
                    () => new Wisp(new RectangleF(), false, PlayerNumber.FirstPlayer)} });
            automat.AddCombo(new[] { Command.MoveLeft, Command.MoveRight, Command.NormalAttack }, ComboName.HolyLight);
            automat.UpdateState(Command.MoveLeft);
            automat.UpdateState(Command.MoveRight);
            automat.UpdateState(Command.NormalAttack);
            var res = automat.PerformCombo();
            Assert.AreEqual(typeof(Wisp), res.GetType());
        }

        [Test]
        public void TestIfRightObjReturned2()
        {
            var automat = new ComboController(new Dictionary<ComboName, Func<GameObject>> {
                { ComboName.HolyLight, () => new Wisp(new RectangleF(), false, PlayerNumber.FirstPlayer)},
                { ComboName.ThrowSpear, () => new Spear(new RectangleF(), false, PlayerNumber.FirstPlayer)} });

            automat.AddCombo(new[] { Command.MoveLeft, Command.MoveRight, Command.NormalAttack }, ComboName.HolyLight);
            automat.AddCombo(new[] { Command.MoveLeft, Command.MoveRight, Command.StrongAttack }, ComboName.ThrowSpear);
            automat.UpdateState(Command.MoveLeft);
            automat.UpdateState(Command.MoveRight);
            automat.UpdateState(Command.NormalAttack);
            var res = automat.PerformCombo();
            Assert.AreEqual(typeof(Wisp), res.GetType());

            automat.UpdateState(Command.StrongAttack);
            Assert.AreEqual(automat.ComboCompleted, false);

            automat.UpdateState(Command.MoveLeft);
            automat.UpdateState(Command.MoveRight);
            automat.UpdateState(Command.StrongAttack);
            res = automat.PerformCombo();
            Assert.AreEqual(typeof(Spear), res.GetType());
        }
    }
}
