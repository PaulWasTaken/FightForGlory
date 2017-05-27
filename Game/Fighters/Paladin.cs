using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Game.BaseStructures.Enums;
using Game.Commands;
using Game.Controllers;
using Game.GameInformation;
using Game.GameObjects;

namespace Game.Fighters
{
    public class Paladin : Fighter
    {
        public Paladin(string name, float x, float y)
        {
            State = FighterMotionState.NotMoving;
            LookingRight = Number == PlayerNumber.FirstPlayer;

            Body = new RectangleF(x, y, GameSettings.Resolution.X / 16f, GameSettings.Resolution.Y / 4.5f);

            Name = name;
            HealthPoints = 100;
            AttackDamage = 10;
            AttackRange = Body.Width / 2;
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
            controller.AddCombo(new[] { Command.Jump, Command.Down, Command.Down }, ComboName.HolyLight);

            return controller;
        }

        public override void ManaRegeneration()
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
