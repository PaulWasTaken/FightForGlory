using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Game.BaseStructures;
using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.ComboWorker;
using Game.BaseStructures.Enums;
using Game.Commands;
using Game.Controllers;
using Game.GameInformation;

namespace Game.Figters
{
    public class Unicorn : Fighter
    {
        public Unicorn(string name, float x, float y)
        {
            State = FighterMotionState.NotMoving;
            Attack = false;
            LookRight = Number == PlayerNumber.FirstPlayer;
            Block = new BlockState();
            
            Body = new RectangleF(x, y, GameSettings.Resolution.X / 16f, GameSettings.Resolution.Y / 4.5f);

            Name = name;
            HealthPoints = 100;
            AttackDamage = 10;
            AttackRange = Body.Width / 2;
        }

        public override void ManaRegeneration()
        {
            if (ManaPoints <= 100)
                ManaPoints += 0.2f;
        }

        public override void BlockCooldown()
        {
            var cooldown = new Timer { Interval = 500, Enabled = true };
            cooldown.Tick += (sender, args) =>
            {
                Block.Blocking = false;
                cooldown.Dispose();
            };
        }

        public override void AttackCooldown()
        {
            var cooldown = new Timer { Interval = 250, Enabled = true };
            cooldown.Tick += (sender, args) =>
            {
                Attack = false;
                cooldown.Dispose();
            };
        }

        public override ComboController GetComboController()
        {
            return new ComboController(new Dictionary<ComboName, Func<GameObject>>());
        }
    }
}
