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
        public Unicorn(string name, float x, float y)
        {
            State = FighterMotionState.NotMoving;
            LookingRight = Number == PlayerNumber.FirstPlayer;
            
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
