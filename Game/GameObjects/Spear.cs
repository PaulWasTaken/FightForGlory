using System.Drawing;
using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.Enums;
using Game.Properties;

namespace Game.GameObjects
{
    public class Spear : GameObject
    {
        public override int Damage => 20;

        public Spear(RectangleF body, bool lookRight, Fighter enemy)
        {
            Opponent = enemy;
            var y = body.Bottom - (body.Bottom - body.Top) / 1.5f;
            float x;
            if (lookRight)
            {
                Picture = Resources.SpearRight.Resize(160, 40);
                Speed = 60;
                x = body.Right;

            }
            else
            {
                Picture = Resources.SpearLeft.Resize(160, 40);
                Speed = -60;
                x = body.Left;
            }
            Position = new PointF(x, y);
        }

        public override bool CheckState()
        {
            if (Speed > 0)
            {
                if (Opponent.Block.Blocking && Opponent.Block.Side == BlockSide.Left) return false;
                if (!this.HasReached(Opponent)) return false;
                Opponent.HealthPoints -= Damage;
                return true;
            }
            if (Opponent.Block.Blocking && Opponent.Block.Side == BlockSide.Right) return false;
            if (!this.HasReached(Opponent)) return false;
            Opponent.HealthPoints -= Damage;
            return true;
        }
    }
}