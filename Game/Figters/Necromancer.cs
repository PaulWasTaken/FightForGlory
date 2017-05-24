using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Game.BaseStructures;
using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.ComboWorker;
using Game.BaseStructures.Enums;
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
            var cooldown = new Timer() { Interval = 200, Enabled = true };
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
            var comboDetector = new ComboDetector();
            var comboPerfomer = new Dictionary<ComboName, Func<GameObject>>();

            if (Number == PlayerNumber.FirstPlayer)
            {
                comboDetector.Add(new[] { Keys.Z, Keys.Z, Keys.Z, Keys.Z, Keys.Z }, ComboName.LightningAttack);
                comboDetector.Add(new[] { Keys.S, Keys.S, Keys.S }, ComboName.Teleport);
            }
            else
            {
                comboDetector.Add(new[] { Keys.K, Keys.K, Keys.K, Keys.K, Keys.K }, ComboName.LightningAttack);
                comboDetector.Add(new[] { Keys.Down, Keys.Down, Keys.Down }, ComboName.Teleport);
            }

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
                    var dx = 300;
                    if (LookRight)
                    {
                        if (Body.BotRightX + dx < GameSettings.Resolution.X)
                        {
                            X += dx;
                            Body.BotRightX += dx;
                            Body.TopLeftX += dx;
                            CurrentImage = Resources.NecromancerTeleportRight;
                            TeleportCooldown();
                        }
                    }
                    else
                    {
                        if (Body.TopLeftX - dx > 0)
                        {
                            X -= dx;
                            Body.BotRightX -= dx;
                            Body.TopLeftX -= dx;
                            CurrentImage = Resources.NecromancerTeleportLeft;
                            TeleportCooldown();
                        }
                    }
                    return null;
                }
                return null;
            };
            return new ComboController(comboDetector, comboPerfomer);
        }
    }
}
