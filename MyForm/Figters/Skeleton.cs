using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Game
{
    class Skeleton : Fighter
    {
        public override Dictionary<PlayerNumber, List<Keys[]>> Combinations => 
            new Dictionary<PlayerNumber, List<Keys[]>>()
        {
            {PlayerNumber.FirstPlayer, new List<Keys[]>() { new[] { Keys.I, Keys.O, Keys.P } }},
            {PlayerNumber.SecondPlayer, new List<Keys[]>() { new[] { Keys.B, Keys.N, Keys.M } }}
        };

        public Skeleton(string name, float x, float y)
        {
            State = FighterMotionState.NotMoving;
            Attack = false;
            LookRight = false;
            Block = new BlockState();
            Picture = new ImageInfo(name);
            Body = new HitBox(x, y);

            Name = name;
            HealthPoints = Stats[name]["HealthPoints"];
            AttackDamage = Stats[name]["AttackDamage"];
            AttackRange = Stats[name]["AttackRange"];

            Combos = new ComboDetector(name);

            if (x < Settings.Resolution.X / 2)
            {
                Combos.Add(new[] { Keys.I, Keys.O, Keys.P }, ComboName.ThrowSpear);
                CurrentImage = Picture.Right;
                LookRight = true;
            }
            else
            {
                Combos.Add(new[] { Keys.B, Keys.N, Keys.M }, ComboName.ThrowSpear);
                CurrentImage = Picture.Left;
            }
            PreviousImage = CurrentImage;
            ComboPerfomer = new Dictionary<ComboName, Action>();
            FormComboPerfomer();
            X = x;
            Y = y;
        }

        public override void FormComboPerfomer()
        {
            ComboPerfomer[ComboName.ThrowSpear] = () => {
                if (!(ManaPoints >= 40)) return;
                ManaPoints -= 40;
                Settings.GameObjects.Add(new Spear(Body, LookRight, Opponent));
            };
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
            var cooldown = new Timer() { Interval = 250, Enabled = true };
            cooldown.Tick += (sender, args) =>
            {
                Attack = false;
                CurrentImage = PreviousImage;
                cooldown.Dispose();
            };
        }
    }
}
