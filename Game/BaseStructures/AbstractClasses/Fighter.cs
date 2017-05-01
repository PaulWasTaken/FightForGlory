using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Game.BaseStructures.Enums;
using Game.GameWindows;

namespace Game.BaseStructures.AbstractClasses
{
    public abstract class Fighter
    {
        public IEnumerable<Keys> GetCombos(PlayerNumber number)
        {
            return Combinations[number].SelectMany(list => list);
        }

        public abstract Dictionary<PlayerNumber, List<Keys[]>> Combinations { get; }

        public Dictionary<string, Dictionary<string, int>> Stats = new Dictionary<string, Dictionary<string, int>>()
        {
            {"Paladin", new Dictionary<string, int>(){ {"HealthPoints", 100}, {"AttackDamage", 10}, {"AttackRange", 45}}},
            {"Skeleton", new Dictionary<string, int>(){ {"HealthPoints", 100}, {"AttackDamage", 10}, {"AttackRange", 45}}},
            {"Necromancer", new Dictionary<string, int>(){ {"HealthPoints", 100}, {"AttackDamage", 10}, {"AttackRange", 45}}},
            {"Unicorn", new Dictionary<string, int>(){ {"HealthPoints", 100}, {"AttackDamage", 10}, {"AttackRange", 45}}}
        };



        public PlayerNumber Number { get; set; }
        public bool IsFrozen { get; set; }
        public Fighter Opponent { get; set; }
        public string Name { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float HealthPoints { get; set; }
        public float ManaPoints { get; set; }
        public float AttackDamage { get; set; }
        public float AttackRange { get; set; }
        public ImageInfo Picture { get; set; }
        public Image CurrentImage { get; set; }
        public bool OnGround { get; set; }
        public HitBox Body { get; set; }
        public bool LookRight { get; set; }
        public BlockState Block { get; set; }
        public bool Attack { get; set; }
        public Image PreviousImage { get; set; }
        public FighterMotionState State { get; set; }
        public ComboDetector.ComboDetector Combos { get; set; }
        public Dictionary<ComboName, Action> ComboPerfomer { get; set; }

        public void KnowYourEnemy(Fighter enemy)
        {
            Opponent = enemy;
        }

        public void CheckForCombo(KeyEventArgs e)
        {
            if (Combos.CurrentState.Name == ComboName.Default)
            {
                Combos.FindValue(e.KeyData);
                return;
            }
            if (Combos.CheckState(e.KeyData))
            {
                ComboPerfomer[Combos.CurrentState.Name]();
                Combos.CurrentState = Combos.DefaultState;
            }
        }

        public void ToTheGround()
        {
            if (Y < Settings.Resolution.Y / 1.5f)
            {
                Y += 15;
                Body.BotRightY += 15;
                Body.TopLeftY += 15;
            }
            else
                OnGround = true;
        }

        public void Jump()
        {
            if (Attack || Block.Blocking)
                return;
            if (OnGround && Y / 2 > 200)
            {
                OnGround = false;
                Y -= 300;
                Body.BotRightY -= 300;
                Body.TopLeftY -= 300;
            }
        }

        public void UpdateImage(int action)
        {
            if (action == (int)MovingSide.GoRight)
            {
                CurrentImage = Picture.GetRight();
                LookRight = true;
                PreviousImage = CurrentImage;
            }
            if (action == (int)MovingSide.GoLeft)
            {
                CurrentImage = Picture.GetLeft();
                LookRight = false;
                PreviousImage = CurrentImage;
            }
        }

        public void ChangeMotionState(FighterMotionState side)
        {
            State = side;
        }

        public void Update(int dx)
        {
            if (Attack || Block.Blocking)
                return;
            var delta = X + dx;
            if (delta < Settings.Resolution.X - Body.Width && delta > 0
                && (Body.BotRightX + dx < Opponent.Body.TopLeftX
                    || Body.TopLeftX + dx > Opponent.Body.BotRightX))
            {
                X = delta;
                Body.TopLeftX += dx;
                Body.BotRightX += dx;
                UpdateImage(Math.Sign(dx));
            }
        }

        public void ImageChanger(BattleStance action)
        {
            if (action == BattleStance.Attack)
                CurrentImage = LookRight ? Picture.AttackRight : Picture.AttackLeft;
            if (action == BattleStance.Block)
                CurrentImage = LookRight ? Picture.BlockRight : Picture.BlockLeft;
        }

        public void DoAttack()
        {
            if (Name == "Necromancer") return;
            if (Attack || Block.Blocking)
                return;
            PreviousImage = CurrentImage;
            ImageChanger(BattleStance.Attack);

            if (Opponent.Body.BotRightY < Body.TopLeftY + Body.Height / 2)
            {
                Attack = true;
                AttackCooldown();
                return;
            }

            if (LookRight)
            {
                Attack = true;
                if (!Opponent.Block.Blocking || Opponent.Block.Side != BlockSide.Left)
                    if (Body.BotRightX + AttackRange >= Opponent.Body.TopLeftX && Body.BotRightX + AttackRange <= Opponent.Body.BotRightX)
                        Opponent.HealthPoints -= AttackDamage;
                AttackCooldown();
            }
            else
            {
                Attack = true;
                if (!Opponent.Block.Blocking || Opponent.Block.Side != BlockSide.Right)
                    if (Body.TopLeftX - AttackRange <= Opponent.Body.BotRightX && Body.TopLeftX - AttackRange >= Opponent.Body.TopLeftX)
                        Opponent.HealthPoints -= AttackDamage;
                AttackCooldown();
            }
        }

        public void DoBlock()
        {
            if (Attack || Block.Blocking)
                return;
            PreviousImage = CurrentImage;
            ImageChanger(BattleStance.Block);
            if (LookRight)
            {
                Block.Blocking = true;
                Block.Side = BlockSide.Right;
                BlockCooldown();
            }
            else
            {
                Block.Blocking = true;
                Block.Side = BlockSide.Left;
                BlockCooldown();
            }
        }

        public abstract void BlockCooldown();
        public abstract void AttackCooldown();
        public abstract void FormComboPerfomer();
        public abstract void ManaRegeneration();
    }
}
