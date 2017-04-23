using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public abstract class Object
    {
        public float Speed { get; set; }
        public Image Picture { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public abstract int Damage { get; }
        public void Move()
        {
            X += Speed;
        }
        public abstract bool CheckState();
        public Fighter Opponent { get; set; }
    }

    public abstract class Fighter
    {
        public IEnumerable<Keys> GetCombos(PlayerNumber number)
        {
            foreach (var list in Combinations[number])
                foreach (var key in list)
                    yield return key;
        }

        public abstract Dictionary<PlayerNumber, List<Keys[]>> Combinations { get; }

        public Dictionary<string, Dictionary<string, int>> Stats = new Dictionary<string, Dictionary<string, int>>()
        {
            {"Paladin", new Dictionary<string, int>(){ {"HealthPoints", 100}, {"AttackDamage", 10}, {"AttackRange", 45}}},
            {"Skeleton", new Dictionary<string, int>(){ {"HealthPoints", 100}, {"AttackDamage", 10}, {"AttackRange", 45}}},
            {"Necromancer", new Dictionary<string, int>(){ {"HealthPoints", 100}, {"AttackDamage", 10}, {"AttackRange", 45}}},
            {"Unicorn", new Dictionary<string, int>(){ {"HealthPoints", 100}, {"AttackDamage", 10}, {"AttackRange", 45}}}
        };

        public enum Side
        {
            GoLeft = -1,
            GoRight = 1,
        }

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
        public ComboDetector Combos { get; set; }
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
            if (Y < GameWindow.Resolution.Y / 1.5f)
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
            if (action == (int)Side.GoRight)
            {
                CurrentImage = Picture.GetRight();
                LookRight = true;
                PreviousImage = CurrentImage;
            }
            if (action == (int)Side.GoLeft)
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
            if (Body.BotRightX < Opponent.Body.BotRightX && Body.BotRightX > Opponent.Body.TopLeftX || Body.TopLeftX > Opponent.Body.TopLeftX && Body.TopLeftX < Opponent.Body.BotRightX)
            {
                var a = 10;
            }
            if (delta < GameWindow.Resolution.X - Body.Width && delta > 0 && (Body.BotRightX + dx < Opponent.Body.TopLeftX || Body.TopLeftX + dx > Opponent.Body.BotRightX))
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
                if (LookRight)
                    CurrentImage = Picture.AttackRight;
                else
                    CurrentImage = Picture.AttackLeft;
            if (action == BattleStance.Block)
                if (LookRight)
                    CurrentImage = Picture.BlockRight;
                else
                    CurrentImage = Picture.BlockLeft;
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

    public abstract class SpecialStrike
    {
        public Fighter Source { get; set; }
        public float Damage { get; set; }
        public float Range { get; set; }
        public abstract bool IfReached();
        public void FixPicture()
        {
            Source.CurrentImage = Source.PreviousImage;
            Source.State = FighterMotionState.NotMoving;
        }
    }
}
