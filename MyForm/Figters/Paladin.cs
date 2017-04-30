using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Game
{
    public class Paladin : Fighter
    {
        public override Dictionary<PlayerNumber, List<Keys[]>> Combinations => 
            new Dictionary<PlayerNumber, List<Keys[]>>()
        {
            {PlayerNumber.FirstPlayer, new List<Keys[]>() { new[] { Keys.E } }},
            {PlayerNumber.SecondPlayer, new List<Keys[]>() { new[] { Keys.O } }}
        };

        public Paladin(string name, float x, float y)
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
                Combos.Add(new[] { Keys.E, Keys.E, Keys.E }, ComboName.HolyLight);
                CurrentImage = Picture.Right;
                LookRight = true;
            }
            else
            {
                Combos.Add(new[] { Keys.O, Keys.O, Keys.O }, ComboName.HolyLight);
                CurrentImage = Picture.Left;
            }
            ComboPerfomer = new Dictionary<ComboName, Action>();
            PreviousImage = CurrentImage;
            FormComboPerfomer();
            X = x;
            Y = y;
        }

        public override void FormComboPerfomer()
        {
            ComboPerfomer[ComboName.HolyLight] = () => {
                if (!(ManaPoints >= 40)) return;
                ManaPoints -= 40;
                Settings.GameObjects.Add(new Wisp(Body, LookRight, Opponent));
            };
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
