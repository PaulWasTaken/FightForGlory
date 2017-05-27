using System.Drawing;
using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.Enums;
using Game.GameInformation;

namespace Game.GameObjects
{
    public class Lightning : GameObject
    {
        public override int Damage => 40;

        public Lightning(RectangleF body, bool lookRight, PlayerNumber source) :
            base(body, lookRight, GameSettings.Resolution.X / 10f, source, 
                GameSettings.Resolution.X / 16f, GameSettings.Resolution.Y / 16f)
        {
        }

        public override bool ShouldDealDamage(Fighter opponent)
        {
            return HasReached(opponent);
        }

        public override bool ShouldBeRemoved(Fighter opponent)
        {
            if (IfLookingRight())
            {
                if (Size.X + Size.Width >= GameSettings.Resolution.X)
                    return true;
            }
            else
            {
                if (Size.X <= 0)
                    return true;
            }
            return HasReached(opponent);
        }

        protected override bool HasReached(Fighter opponent)
        {
            return IfLookingRight() ? opponent.Body.Contains(Size.X + Size.Width + opponent.Body.Width / 3, Size.Y + Size.Height / 2) ||
                opponent.Body.Contains(Size.X + Size.Width - opponent.Body.Width / 3, Size.Y + Size.Height / 2) : 
                opponent.Body.Contains(Size.X - opponent.Body.Width / 3, Size.Y + Size.Height / 2) ||
                opponent.Body.Contains(Size.X + opponent.Body.Width / 3, Size.Y + Size.Height / 2);
        }

        public override void Move()
        {
            Size = IfLookingRight() ? new RectangleF(Size.Location, new SizeF(Size.Width + Speed, Size.Height)) : 
                new RectangleF(new PointF(Size.X + Speed, Size.Y), new SizeF(Size.Width - Speed, Size.Height));
        }
    }
}
