using System.Drawing;
using Game.BaseStructures.Enums;
using Game.GameInformation;
using Game.Fighters;

namespace Game.GameObjects
{
    public class Wisp : GameObject
    {
        public override int Damage => 20;

        public Wisp(RectangleF body, bool lookRight, PlayerNumber source) :
            base(body, lookRight, GameSettings.Resolution.X / 18f, source, 
                GameSettings.Resolution.Y / 8f, GameSettings.Resolution.Y / 8f)
        {
        }

        public override bool ShouldDealDamage(Fighter opponent)
        {
            if (IfLookingRight())
            {
                if (opponent.IsBlocking && !opponent.LookingRight) return false;
                return HasReached(opponent);
            }
            if (opponent.IsBlocking && opponent.LookingRight) return false;
            return HasReached(opponent);
        }
        public override bool ShouldBeRemoved(Fighter opponent)
        {
            if (!(Size.X >= 0 && Size.X <= GameSettings.Resolution.X)) return true;
            return HasReached(opponent);
        }
        protected override bool HasReached(Fighter opponent)
        {
            return opponent.Body.Contains(Size.X, Size.Y + Size.Height / 2);
        }

        public override void Move()
        {
            Size = Size.Move(Speed, 0);
        }
    }
}
