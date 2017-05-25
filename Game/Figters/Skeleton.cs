using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Game.BaseStructures;
using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.ComboWorker;
using Game.BaseStructures.Enums;
using Game.Commands;

namespace Game.Figters
{
    public class Skeleton : Fighter
    {
        public Skeleton(string name, float x, float y)
        {
            State = FighterMotionState.NotMoving;
            Attack = false;
            LookRight = false;
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

        public override void ManaRegeneration()
        {
            if (ManaPoints <= 100)
                ManaPoints += 0.2f;
        }

        public override void BlockCooldown()
        {
            var cooldown = new Timer() { Interval = 500, Enabled = true };
            cooldown.Tick += (sender, args) =>
            {
                Block.Blocking = false;
                CurrentImage = PreviousImage;
                cooldown.Dispose();
            };
        }

        public override void AttackCooldown()
        {
            var cooldown = new Timer { Interval = 250, Enabled = true };
            cooldown.Tick += (sender, args) =>
            {
                Attack = false;
                CurrentImage = PreviousImage;
                cooldown.Dispose();
            };
        }

        public override ComboController GetCombos()
        {
            var comboDetector = new ComboDetector<Command>();
            var comboPerfomer = new Dictionary<ComboName, Func<GameObject>>();

            comboDetector.Add(new[] { Command.NormalAttack, Command.Down, Command.StrongAttack}, ComboName.ThrowSpear);

            comboPerfomer[ComboName.ThrowSpear] = () => {
                if (ManaPoints < 40) return null;
                ManaPoints -= 40;
                return new Spear(Body, LookRight, Opponent);
            };

            return new ComboController(comboDetector, comboPerfomer);
        }
    }
}
