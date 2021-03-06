﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Game.BaseStructures.Enums;
using Game.Commands;
using Game.Controllers;
using Game.GameObjects;

namespace Game.Fighters
{
    public class Necromancer : Fighter
    {
        public Necromancer(string name, PointF location) : base(name, location)
        {
            AttackDamage = 0;
            AttackRange = 0;
        }

        public void TeleportCooldown()
        {
            var cooldown = new Timer {Interval = 200, Enabled = true};
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
                    if (ManaPoints < 40) return null;
                    ManaPoints -= 40;
                    IsAttacking = true;
                    AttackCooldown();
                    return new Lightning(Body, LookingRight, Number);
                }
            };

            var controller = new ComboController(comboResults);
            controller.AddCombo(
                new[]
                {
                    Command.NormalAttack, Command.NormalAttack, Command.NormalAttack, Command.NormalAttack,
                    Command.NormalAttack
                }, ComboName.LightningAttack);
            controller.AddCombo(new[] {Command.Down, Command.Down, Command.Down}, ComboName.Teleport);

            return controller;
        }

        protected override float ManaRegenerationAmount => 0.5f;
        protected override int AttackCooldownValue => 200;
        protected override int BlockCooldownValue => 200;
    }
}
