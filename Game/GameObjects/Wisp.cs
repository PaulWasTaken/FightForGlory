using System.Drawing;
using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.Enums;
using Game.Properties;

namespace Game.GameObjects
{
    public class Wisp : GameObject
    {
        public override int Damage => 20;

        public Wisp(RectangleF body, bool lookRight, Fighter enemy)
        {
            Opponent = enemy;
            Picture = Resources.Wisp.Resize(40, 40);
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

        public override bool CheckState()
        {
            if (Speed > 0)
            {
                if (Opponent.Block.Blocking && Opponent.Block.Side == BlockSide.Left) return false;
                if (!HasReached(Opponent)) return false;
                Opponent.HealthPoints -= Damage;
                return true;
            }
            if (Opponent.Block.Blocking && Opponent.Block.Side == BlockSide.Right) return false;
            if (!HasReached(Opponent)) return false;
            Opponent.HealthPoints -= Damage;
            return true;
        }
    }
}
