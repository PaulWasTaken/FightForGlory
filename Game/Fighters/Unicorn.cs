using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Game.BaseStructures.Enums;
using Game.Controllers;
using Game.GameInformation;
using Game.GameObjects;

namespace Game.Fighters
{
    public class Unicorn : Fighter
    {
        public Unicorn(string name, PointF location) : base(name, location) { }

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
            return new ComboController(new Dictionary<ComboName, Func<GameObject>>());
        }
    }
}
