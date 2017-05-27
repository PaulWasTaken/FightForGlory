using System.Drawing;
using Game.BaseStructures.Enums;
using Game.Fighters;
using Game.GameInformation;
using Game.Properties;

namespace Game.GameObjects
{
    public class Spear : GameObject
    {
        public override int Damage => 20;

        public Spear(RectangleF body, bool lookRight, PlayerNumber source)
        {
            Source = source;
            var y = body.Bottom - (body.Bottom - body.Top) / 1.5f;
            float x;
            if (lookRight)
            {
                Speed = 60;
                x = body.Right;

            }
            else
            {
                Speed = -60;
                x = body.Left;
            }
            Size = new RectangleF(x, y, GameSettings.Resolution.X / 10f, GameSettings.Resolution.Y / 18f);
        }

        public override bool CheckState(Fighter opponent)
        {
            if (IsOutsideScreen()) return true;
            if (Speed > 0)
            {
                if (opponent.IsBlocking && !opponent.LookingRight) return false;
                if (!HasReached(opponent)) return false;
                opponent.HealthPoints -= Damage;
                return true;
            }
            if (opponent.IsBlocking && opponent.LookingRight) return false;
            if (!HasReached(opponent)) return false;
            opponent.HealthPoints -= Damage;
            return true;
        }
    }
}