using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    class Unicorn : Fighter
    {
        public override Dictionary<PlayerNumber, List<Keys[]>> Combinations => 
            new Dictionary<PlayerNumber, List<Keys[]>>()
        {
            {PlayerNumber.FirstPlayer, new List<Keys[]>() { new[] { Keys.D, Keys.Z }, new[] { Keys.A, Keys.Z } }},
            {PlayerNumber.SecondPlayer, new List<Keys[]>() { new[] { Keys.Right, Keys.K }, new[] { Keys.Left, Keys.K } }}
        };

        public Unicorn(string name, float x, float y)
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
                Combos.Add(new[] { Keys.D, Keys.D, Keys.D, Keys.Z }, ComboName.DevastatingCharge);
                Combos.Add(new[] { Keys.A, Keys.A, Keys.A, Keys.Z }, ComboName.DevastatingCharge);
                CurrentImage = Picture.Right;
                LookRight = true;
            }
            else
            {
                Combos.Add(new[] { Keys.Right, Keys.Right, Keys.Right, Keys.K }, ComboName.DevastatingCharge);
                Combos.Add(new[] { Keys.Left, Keys.Left, Keys.Left, Keys.K }, ComboName.DevastatingCharge);
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
            ComboPerfomer[ComboName.DevastatingCharge] = () => {
                if (ManaPoints >= 30)
                {
                    ManaPoints -= 30;
                    GameWindow.SpecialStrikes.Add(new DevastatingCharge(this));
                }
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
            var cooldown = new Timer { Interval = 250, Enabled = true };
            cooldown.Tick += (sender, args) =>
            {
                Attack = false;
                CurrentImage = PreviousImage;
                cooldown.Dispose();
            };
        }
    }
}
