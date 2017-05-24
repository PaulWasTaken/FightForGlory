using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Game.BaseStructures;
using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.ComboWorker;
using Game.BaseStructures.Enums;
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
            HealthPoints = Stats[name]["HealthPoints"];
            AttackDamage = Stats[name]["AttackDamage"];
            AttackRange = Stats[name]["AttackRange"];

            CurrentImage = LookRight ? Picture.Right : Picture.Left;

            PreviousImage = CurrentImage;
            X = x;
            Y = y;
            Body = new HitBox(X, Y);
        }

        public override ComboController GetCombos()
        {
            var detector = new ComboDetector();

            if (Number == PlayerNumber.FirstPlayer)
                detector.Add(new[] { Keys.E, Keys.E, Keys.E }, ComboName.HolyLight);
            else
                detector.Add(new[] { Keys.O, Keys.O, Keys.O }, ComboName.HolyLight);

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
