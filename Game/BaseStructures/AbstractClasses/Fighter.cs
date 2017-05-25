using System;
using System.Drawing;
using Game.BaseStructures.Enums;
using Game.GameInformation;

namespace Game.BaseStructures.AbstractClasses
{
    public abstract class Fighter
    {
        public PlayerNumber Number { get; set; }
        public bool IsFrozen { get; set; }
        public Fighter Opponent { get; set; }
        public string Name { get; set; }
        public float HealthPoints { get; set; }
        public float ManaPoints { get; set; }
        public float AttackDamage { get; set; }
        public float AttackRange { get; set; }
        public ImageInfo Picture { get; set; }
        public Image CurrentImage { get; set; }
        public bool OnGround { get; set; }
        public RectangleF Body { get; set; }
        public bool LookRight { get; set; }
        public BlockState Block { get; set; }
        public bool Attack { get; set; }
        public Image PreviousImage { get; set; }
        public FighterMotionState State { get; set; }

        public void KnowYourEnemy(Fighter enemy)
        {
            Opponent = enemy;
        }

        public void ToTheGround()
        {
            if (Body.Y < GameSettings.Resolution.Y / 1.5f)
                Body = GameMethods.MoveRect(Body, 0, 15);
            else
                OnGround = true;
        }

        public void Jump()
        {
            if (Attack || Block.Blocking)
                return;
            if (!OnGround || !(Body.Y / 2 > 200)) return;
            OnGround = false;
            Body = GameMethods.MoveRect(Body, 0, -300);
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
            if (!this.IsMovementAllowed(dx, 0, Opponent)) return;
            Body = GameMethods.MoveRect(Body, dx, 0);
            UpdateImage(Math.Sign(dx));
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

            if (Opponent.Body.Y + Opponent.Body.Height / 2 < Body.Y)
            {
                Attack = true;
                AttackCooldown();
                return;
            }

            if (LookRight)
            {
                Attack = true;
                if (!Opponent.Block.Blocking || Opponent.Block.Side != BlockSide.Left)
                    if (Opponent.Body.Contains(Body.Right + AttackRange, Body.Y - Body.Height / 4))
                        Opponent.HealthPoints -= AttackDamage;
                AttackCooldown();
            }
            else
            {
                Attack = true;
                if (!Opponent.Block.Blocking || Opponent.Block.Side != BlockSide.Right)
                    if (Opponent.Body.Contains(Body.Left - AttackRange, Body.Y - Body.Height / 4))
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
        public abstract ComboController GetCombos();
        public abstract void ManaRegeneration();
    }
}
