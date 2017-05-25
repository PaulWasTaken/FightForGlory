﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Game.BaseStructures;
using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.ComboWorker;
using Game.BaseStructures.Enums;
using Game.Commands;
using Game.GameInformation;
using Game.GameObjects;

namespace Game.Figters
{
    public class Paladin : Fighter
    {
        public Paladin(string name, float x, float y)
        {
            State = FighterMotionState.NotMoving;
            Attack = false;
            LookRight = Number == PlayerNumber.FirstPlayer;
            Block = new BlockState();
            Picture = new ImageInfo(name);

            Name = name;
            HealthPoints = 100;
            AttackDamage = 10;
            AttackRange = 10;

            CurrentImage = LookRight ? Picture.Right : Picture.Left;

            PreviousImage = CurrentImage;
            Body = new RectangleF(x, y, GameSettings.Resolution.X / 16f, GameSettings.Resolution.Y / 4.5f);
        }

        public override ComboController GetCombos()
        {
            var detector = new ComboDetector<Command>();

            detector.Add(new[] { Command.Jump, Command.Down, Command.Down }, ComboName.HolyLight);

            var comboPerfomer = new Dictionary<ComboName, Func<GameObject>>();
            comboPerfomer[ComboName.HolyLight] = () => {
                if (!(ManaPoints >= 40)) return null;
                ManaPoints -= 40;
                return new Wisp(Body, LookRight, Opponent);
            };

            return new ComboController(detector, comboPerfomer);
        }

        public override void ManaRegeneration()
        {
            if (ManaPoints <= 100)
                ManaPoints += 0.2f;
        }

        public override void BlockCooldown()
        {
            var cooldown = new Timer() { Interval = 1000, Enabled = true };
            cooldown.Tick += (sender, args) =>
            {
                Block.Blocking = false;
                CurrentImage = PreviousImage;
                cooldown.Dispose();
            };
        }

        public override void AttackCooldown()
        {
            var cooldown = new Timer { Interval = 500, Enabled = true };
            cooldown.Tick += (sender, args) =>
            {
                Attack = false;
                CurrentImage = PreviousImage;
                cooldown.Dispose();
            };
        }
    }
}
