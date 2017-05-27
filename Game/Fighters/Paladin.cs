using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Game.BaseStructures.Enums;
using Game.Commands;
using Game.Controllers;
using Game.GameObjects;

namespace Game.Fighters
{
    public class Paladin : Fighter
    {
        public Paladin(string name, PointF location) : base(name, location) { }

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
            controller.AddCombo(new[] { Command.Jump, Command.Down, Command.Down }, ComboName.HolyLight);

            return controller;
        }

        public override void RegenerateMana()
        {
            if (ManaPoints <= 100)
                ManaPoints += 0.2f;
        }

        public override void BlockCooldown()
        {
            var cooldown = new Timer { Interval = 1000, Enabled = true };
            cooldown.Tick += (sender, args) =>
            {
                IsBlocking = false;
                cooldown.Dispose();
            };
        }

        public override void AttackCooldown()
        {
            var cooldown = new Timer { Interval = 500, Enabled = true };
            cooldown.Tick += (sender, args) =>
            {
                IsAttacking = false;
                cooldown.Dispose();
            };
        }
    }
}
