using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Game.BaseStructures;
using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.ComboWorker;
using Game.BaseStructures.Enums;
using Game.Commands;
using Game.GameInformation;
using Game.GameWindows;
using Game.SpecialStrikes;

namespace Game.Figters
{
    public class Unicorn : Fighter
    {
        public Unicorn(string name, float x, float y)
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
            return new ComboController(new ComboDetector<Command>(), new Dictionary<ComboName, Func<GameObject>>());
            /*
            var comboDetector = new ComboDetector<Command>();
            var comboPerfomer = new Dictionary<ComboName, Func<GameObject>>();

            if (Number == PlayerNumber.FirstPlayer)
            {
                comboDetector.Add(new[] { Keys.D, Keys.D, Keys.D, Keys.Z }, ComboName.DevastatingCharge);
                comboDetector.Add(new[] { Keys.A, Keys.A, Keys.A, Keys.Z }, ComboName.DevastatingCharge);
                CurrentImage = Picture.Right;
                LookRight = true;
            }
            else
            {
                comboDetector.Add(new[] { Keys.Right, Keys.Right, Keys.Right, Keys.K }, ComboName.DevastatingCharge);
                comboDetector.Add(new[] { Keys.Left, Keys.Left, Keys.Left, Keys.K }, ComboName.DevastatingCharge);
                CurrentImage = Picture.Left;
            }

            comboPerfomer[ComboName.DevastatingCharge] = () => {
                if (ManaPoints >= 30)
                {
                    ManaPoints -= 30;
                    //GameWindow.SpecialStrikes.Add(new DevastatingCharge(this));
                }
                return null;
            };
            
            return new ComboController(comboDetector, comboPerfomer);
            */
        }
    }
}
