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
    public class Skeleton : Fighter
    {
        public Skeleton(string name, PointF location) : base(name, location) { }
        public override void RegenerateMana()
        {
            if (ManaPoints <= 100)
                ManaPoints += 0.2f;
        }

        public override void BlockCooldown()
        {
            var cooldown = new Timer { Interval = 500, Enabled = true };
            cooldown.Tick += (sender, args) =>
            {
                IsBlocking = false;
                cooldown.Dispose();
            };
        }

        public override void AttackCooldown()
        {
            var cooldown = new Timer { Interval = 250, Enabled = true };
            cooldown.Tick += (sender, args) =>
            {
                IsAttacking = false;
                cooldown.Dispose();
            };
        }

        public override ComboController GetComboController()
        {
            var comboResults = new Dictionary<ComboName, Func<GameObject>>
            {
                [ComboName.ThrowSpear] = () =>
                {
                    if (ManaPoints < 40) return null;
                    ManaPoints -= 40;
                    return new Spear(Body, LookingRight, Number);
                }
            };

            var controller = new ComboController(comboResults);
            controller.AddCombo(new[] { Command.NormalAttack, Command.Down, Command.StrongAttack }, ComboName.ThrowSpear);

            return controller;
        }
    }
}
