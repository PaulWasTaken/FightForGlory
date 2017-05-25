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
using Game.GameObjects;
using Game.Properties;

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
            Picture = new ImageInfo(name);

            Name = name;
            HealthPoints = 100;
            AttackDamage = 0;
            AttackRange = 0;

            CurrentImage = LookRight ? Picture.Right : Picture.Left;
            PreviousImage = CurrentImage;
            
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
                CurrentImage = PreviousImage;
                cooldown.Dispose();
            };
        }

        public override void AttackCooldown()
        {
            var cooldown = new Timer { Interval = 200, Enabled = true };
            cooldown.Tick += (sender, args) =>
            {
                Attack = false;
                CurrentImage = PreviousImage;
                cooldown.Dispose();
            };
        }

        public void TeleportCooldown()
        {
            var cooldown = new Timer { Interval = 200, Enabled = true };
            cooldown.Tick += (sender, args) =>
            {
                CurrentImage = PreviousImage;
                cooldown.Dispose();
            };
        }

        public override ComboController GetCombos()
        {
            var comboDetector = new ComboDetector<Command>();
            var comboPerfomer = new Dictionary<ComboName, Func<GameObject>>();

            comboDetector.Add(new[] { Command.NormalAttack, Command.NormalAttack, Command.NormalAttack, Command.NormalAttack, Command.NormalAttack }, ComboName.LightningAttack);
            comboDetector.Add(new[] { Command.Down, Command.Down, Command.Down }, ComboName.Teleport);

            comboPerfomer[ComboName.LightningAttack] = () =>
            {
                if (!(ManaPoints >= 40)) return null;
                ManaPoints -= 40;
                if (LookRight)
                {
                    PreviousImage = CurrentImage;
                    CurrentImage = Picture.AttackRight;
                    AttackCooldown();
                }
                else
                {
                    PreviousImage = CurrentImage;
                    CurrentImage = Picture.AttackLeft;
                    AttackCooldown();
                }
                return new Bolt(Body, LookRight, Opponent);
            };

            comboPerfomer[ComboName.Teleport] = () =>
            {
                if (ManaPoints >= 10)
                {
                    ManaPoints -= 10;
                    PreviousImage = CurrentImage;
                    var dx = GameSettings.Resolution.X / 5;
                    if (LookRight)
                    {
                        if (!this.IsMovementAllowed(dx, 0, Opponent)) return null;
                        Body = GameMethods.MoveRect(Body, dx, 0);
                        CurrentImage = Resources.NecromancerTeleportRight;
                        TeleportCooldown();
                    }
                    else
                    {
                        if (!this.IsMovementAllowed(-dx, 0, Opponent)) return null;
                        Body = GameMethods.MoveRect(Body, -dx, 0);
                        CurrentImage = Resources.NecromancerTeleportLeft;
                        TeleportCooldown();
                    }
                    return null;
                }
                return null;
            };
            return new ComboController(comboDetector, comboPerfomer);
        }
    }
}
