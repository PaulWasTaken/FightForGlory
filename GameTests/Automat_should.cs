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
    public class Automat_should
    {
        [Test]
        public void TestIfShouldPerform()
        {
            var automat = new ComboController(new Dictionary<ComboName, Func<GameObject>> { {ComboName.HolyLight,
                    () => new Wisp(new RectangleF(), false, PlayerNumber.FirstPlayer)} });
            automat.AddCombo(new[] { Command.MoveLeft, Command.MoveRight, Command.NormalAttack }, ComboName.HolyLight);
            automat.UpdateState(Command.MoveLeft);
            automat.UpdateState(Command.MoveRight);

            automat.ComboCompleted.Should().BeFalse();

            automat.UpdateState(Command.NormalAttack);

            automat.ComboCompleted.Should().BeTrue();
        }

        [Test]
        public void TestIfShouldNotPerform()
        {
            var automat = new ComboController(new Dictionary<ComboName, Func<GameObject>> { {ComboName.HolyLight,
                    () => new Wisp(new RectangleF(), false, PlayerNumber.FirstPlayer)} });
            automat.AddCombo(new[] { Command.MoveLeft, Command.MoveRight, Command.NormalAttack }, ComboName.HolyLight);

            automat.ComboCompleted.Should().BeFalse();

            automat.UpdateState(Command.MoveLeft);

            automat.ComboCompleted.Should().BeFalse();

            automat.UpdateState(Command.StrongAttack);

            automat.ComboCompleted.Should().BeFalse();

            automat.UpdateState(Command.MoveLeft);

            automat.ComboCompleted.Should().BeFalse();

            automat.UpdateState(Command.MoveRight);

            automat.ComboCompleted.Should().BeFalse();

            automat.UpdateState(Command.Block);

            automat.ComboCompleted.Should().BeFalse();
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
            res.GetType().Should().Be(typeof(Wisp));
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
            res.GetType().Should().Be(typeof(Wisp));

            automat.UpdateState(Command.StrongAttack);

            automat.ComboCompleted.Should().BeFalse();

            automat.UpdateState(Command.MoveLeft);
            automat.UpdateState(Command.MoveRight);
            automat.UpdateState(Command.StrongAttack);
            res = automat.PerformCombo();

            res.GetType().Should().Be(typeof(Spear));
        }

        [Test]
        public void TestStatesChanging()
        {
            var automat = new ComboController(new Dictionary<ComboName, Func<GameObject>>
            {
                {ComboName.HolyLight, () => new Wisp(new RectangleF(), false, PlayerNumber.FirstPlayer)}
            });

            automat.AddCombo(new[] {Command.MoveLeft, Command.MoveRight, Command.NormalAttack}, ComboName.HolyLight);
            automat.UpdateState(Command.MoveLeft);
            automat.UpdateState(Command.MoveRight);
            automat.UpdateState(Command.MoveLeft);

            automat.CurrentState.Value.ShouldBeEquivalentTo(Command.MoveLeft);
        }
    }
}
