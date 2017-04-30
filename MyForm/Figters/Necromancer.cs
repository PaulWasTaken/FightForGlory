using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Game.Properties;

namespace Game
{
    class Necromancer : Fighter
    {
        public override Dictionary<PlayerNumber, List<Keys[]>> Combinations => 
            new Dictionary<PlayerNumber, List<Keys[]>>()
        {
            {PlayerNumber.FirstPlayer, new List<Keys[]>() { new[] { Keys.Z }, new[] { Keys.S } }},
            {PlayerNumber.SecondPlayer, new List<Keys[]>() { new[] { Keys.Down }, new[] { Keys.K } }}
        };

        public Necromancer(string name, float x, float y)
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
            ComboPerfomer = new Dictionary<ComboName, Action>();
            FormComboPerfomer();

            if (x < Settings.Resolution.X / 2)
            {
                Combos.Add(new[] { Keys.Z, Keys.Z, Keys.Z, Keys.Z, Keys.Z }, ComboName.LightningAttack);
                Combos.Add(new[] { Keys.S, Keys.S, Keys.S }, ComboName.Teleport);
                CurrentImage = Picture.Right;
                LookRight = true;
            }
            else
            {
                Combos.Add(new[] { Keys.K, Keys.K, Keys.K, Keys.K, Keys.K }, ComboName.LightningAttack);
                Combos.Add(new[] { Keys.Down, Keys.Down, Keys.Down }, ComboName.Teleport);
                CurrentImage = Picture.Left;
            }
            PreviousImage = CurrentImage;
            
            X = x;
            Y = y;
        }

        public override void FormComboPerfomer()
        {
            ComboPerfomer[ComboName.LightningAttack] = () => 
            {
                if (ManaPoints >= 40)
                {
                    ManaPoints -= 40;
                    GameWindow.GameObjects.Add(new Bolt(Body, LookRight, Opponent));
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
                }
            };
            ComboPerfomer[ComboName.Teleport] = () =>
            {
                if (ManaPoints >= 10)
                {
                    ManaPoints -= 10;
                    PreviousImage = CurrentImage;
                    var dx = 300;
                    if (LookRight)
                    {
                        if (Body.BotRightX + dx < Settings.Resolution.X)
                        {
                            X += dx;
                            Body.BotRightX += dx;
                            Body.TopLeftX += dx;
                            CurrentImage = Resources.NecromancerTeleportRight;
                            TeleportCooldown();
                        }
                    }
                    else
                        if (Body.TopLeftX - dx > 0)
                        {
                            X -= dx;
                            Body.BotRightX -= dx;
                            Body.TopLeftX -= dx;
                            CurrentImage = Resources.NecromancerTeleportLeft;
                            TeleportCooldown();
                        }
                }
            };
        }

        public override void ManaRegeneration()
        {
            if (ManaPoints <= 100)
                ManaPoints += 0.4f;
        }

        public override void BlockCooldown()
        {
            var cooldown = new Timer() { Interval = 200, Enabled = true };
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
            var cooldown = new Timer() { Interval = 200, Enabled = true };
            cooldown.Tick += (sender, args) =>
            {
                CurrentImage = PreviousImage;
                cooldown.Dispose();
            };
        }
    }
}
