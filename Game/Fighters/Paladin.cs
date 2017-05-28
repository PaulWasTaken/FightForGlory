using System;
using System.Collections.Generic;
using System.Drawing;
using Game.BaseStructures.Enums;
using Game.Commands;
using Game.Controllers;
using Game.GameObjects;

namespace Game.Fighters
{
    public class Paladin : Fighter
    {
        public Paladin(string name, PointF location) : base(name, location)
        {
        }

        public override ComboController GetComboController()
        {
            var comboResults = new Dictionary<ComboName, Func<GameObject>>
            {
                [ComboName.HolyLight] = () =>
                {
                    if (!(ManaPoints >= 40)) return null;
                    ManaPoints -= 40;
                    return new Wisp(Body, LookingRight, Number);
                }
            };

            var controller = new ComboController(comboResults);
            controller.AddCombo(new[] {Command.Jump, Command.Down, Command.Down}, ComboName.HolyLight);

            return controller;
        }

        protected override float ManaRegenerationAmount => 0.2f;
        protected override int AttackCooldownValue => 500;
        protected override int BlockCooldownValue => 1000;
    }
}
