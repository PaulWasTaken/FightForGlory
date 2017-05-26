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
                Body = Body.Move(0, 15);
            else
                OnGround = true;
        }

        public void Jump()
        {
            if (Attack || Block.Blocking)
                return;
            if (!OnGround || !(Body.Y / 2 > 200)) return;
            OnGround = false;
            Body = Body.Move(0, -300);
        }

        public void Move(int dx, Fighter opponent)
        {
            if (Attack || Block.Blocking)
                return;
            if (!this.IsMovementAllowed(dx, 0, opponent)) return;
            Body = Body.Move(dx, 0);
            if (!IsMovementAllowed(dx, 0, opponent)) return;
            Body = Body.Move(dx, 0);
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

        protected bool IsMovementAllowed(float dx, float dy, Fighter opponent)
        {
            var newFighterPos = new RectangleF(Body.X + dx, Body.Y + dy, Body.Width, Body.Height);

            var notAllowed = newFighterPos.IntersectsWith(opponent.Body) || IsOutsideScreen(dx, dy);
            return !notAllowed;
        }

        protected bool IsOutsideScreen(float dx, float dy)
        {
            var newFighterPos = new RectangleF(Body.X + dx, Body.Y + dy, Body.Width, Body.Height);

            var leftScreenBorder = new RectangleF(-1, 0, 1, GameSettings.Resolution.Y);
            var rightScreenBorder = new RectangleF(GameSettings.Resolution.X - 1, 0, 1, GameSettings.Resolution.Y);

            return newFighterPos.IntersectsWith(leftScreenBorder) || newFighterPos.IntersectsWith(rightScreenBorder);
        }

        public abstract void BlockCooldown();
        public abstract void AttackCooldown();
        public abstract ComboController GetCombos();
        public abstract void ManaRegeneration();
    }
}
