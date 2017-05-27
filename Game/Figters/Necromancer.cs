﻿using System;
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
using Game.GameObjects;

namespace Game.Figters
{
    public class Necromancer : Fighter
    {
        public Necromancer(string name, float x, float y)
        {
            State = FighterMotionState.NotMoving;
            Attack = false;
            LookRight = Number == PlayerNumber.FirstPlayer;
            Block = new BlockState();

            Name = name;
            HealthPoints = 100;
            AttackDamage = 0;
            AttackRange = 0;

            Body = new RectangleF(x, y, GameSettings.Resolution.X / 16f, GameSettings.Resolution.Y / 4.5f);
        }

        public override void ManaRegeneration()
        {
            if (ManaPoints <= 100)
                ManaPoints += 0.4f;
        }

        public override void BlockCooldown()
        {
            var cooldown = new Timer { Interval = 200, Enabled = true };
            cooldown.Tick += (sender, args) =>
            {
                Block.Blocking = false;
                cooldown.Dispose();
            };
        }

        public override void AttackCooldown()
        {
            var cooldown = new Timer { Interval = 10, Enabled = true };
            cooldown.Tick += (sender, args) =>
            {
                Attack = false;
                cooldown.Dispose();
            };
        }

        public void TeleportCooldown()
        {
            var cooldown = new Timer { Interval = 200, Enabled = true };
            cooldown.Tick += (sender, args) =>
            {
                State = FighterMotionState.NotMoving;
                cooldown.Dispose();
            };
        }

        public override ComboController GetComboController()
        {
            var comboResults = new Dictionary<ComboName, Func<GameObject>>
            {
                [ComboName.LightningAttack] = () =>
                {
                    if (!(ManaPoints >= 40)) return null;
                    ManaPoints -= 40;
                    Attack = true;
                    AttackCooldown();
                    return new Lightning(Body, LookRight, Number);
                }
            };

            var controller = new ComboController(comboResults);
            controller.AddCombo(new[] { Command.NormalAttack, Command.NormalAttack, Command.NormalAttack, Command.NormalAttack, Command.NormalAttack }, ComboName.LightningAttack);
            controller.AddCombo(new[] { Command.Down, Command.Down, Command.Down }, ComboName.Teleport);

            return controller;
        }
    }
}
