using System.Drawing;
using Game.BaseStructures.Enums;
using Game.Fighters;

namespace Game.GameObjects
{
    public abstract class GameObject
    {
        protected GameObject(RectangleF body, bool lookRight, float speed, PlayerNumber source, float width, float height)
        {
            Source = source;
            var y = body.Bottom - (body.Bottom - body.Top) / 1.5f;
            float x;
            if (lookRight)
            {
                Speed = speed;
                x = body.Right;
            }
            else
            {
                Speed = -speed;
                x = body.Left;
            }
            Size = new RectangleF(x, y, width, height);
        }
        protected float Speed { get; set; }
        public RectangleF Size { get; set; }
        public abstract int Damage { get; }
        public PlayerNumber Source { get; set; }

        public bool IfLookingRight()
        {
            return Speed > 0;
        }

        public abstract bool ShouldBeRemoved(Fighter opponent);

        protected abstract bool HasReached(Fighter opponent);

        public abstract void Move();

        public abstract bool ShouldDealDamage(Fighter opponent);
    }
}
