using System;
using System.Drawing;
using Game.BaseStructures.Enums;
using Game.Controllers;
using Game.GameInformation;

namespace Game.BaseStructures.AbstractClasses
{
    public abstract class Fighter
    {
        public PlayerNumber Number { get; set; }
        public bool IsFrozen { get; set; }
        //public Fighter Opponent { get; set; }
        public string Name { get; set; }
        public float HealthPoints { get; set; }
        public float ManaPoints { get; set; }
        public float AttackDamage { get; set; }
        public float AttackRange { get; set; }
        public bool OnGround { get; set; }
        public RectangleF Body { get; set; }
        public bool LookRight { get; set; }
        public BlockState Block { get; set; }
        public bool Attack { get; set; }
        public FighterMotionState State { get; set; }

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

        public void Move(int dx, Fighter opponent)
        {
            if (Attack || Block.Blocking)
                return;
            if (!this.IsMovementAllowed(dx, 0, opponent)) return;
            Body = GameMethods.MoveRect(Body, dx, 0);
        }

        public void DoAttack()
        {
            //if (Name == "Necromancer") return;
            if (Attack || Block.Blocking)
                return;

            Attack = true;
            AttackCooldown();
        }

        public void DoBlock()
        {
            if (Attack || Block.Blocking)
                return;
            Block.Blocking = true;
            BlockCooldown();
            Block.Side = LookRight ? BlockSide.Right : BlockSide.Left;
        }

        public abstract void BlockCooldown();
        public abstract void AttackCooldown();
        public abstract ComboController GetCombos();
        public abstract void ManaRegeneration();
    }
}
