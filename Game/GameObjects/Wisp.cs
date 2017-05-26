using System.Drawing;
using Game.BaseStructures;
using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.Enums;
using Game.Properties;

namespace Game.GameObjects
{
    public class Wisp : GameObject
    {
        public override int Damage => 20;

        public Wisp(RectangleF body, bool lookRight, PlayerNumber source)
        {
            Source = source;
            Picture = GameMethods.ResizeBitmap(Resources.Wisp, 40, 40);
            var y = body.Bottom - (body.Bottom - body.Top) / 1.5f;
            float x;
            if (lookRight)
            {
                Speed = 50;
                x = body.Right;
            }
            else
            {
                Speed = -50;
                x = body.Left;
            }
            Position = new PointF(x, y);
        }

        public override bool CheckState(Fighter opponent)
        {
            if (Speed > 0)
            {
                if (opponent.Block.Blocking && opponent.Block.Side == BlockSide.Left) return false;
                if (!this.IfReached(opponent)) return false;
                opponent.HealthPoints -= Damage;
                return true;
            }
            if (opponent.Block.Blocking && opponent.Block.Side == BlockSide.Right) return false;
            if (!this.IfReached(opponent)) return false;
            opponent.HealthPoints -= Damage;
            return true;
        }
    }
}
