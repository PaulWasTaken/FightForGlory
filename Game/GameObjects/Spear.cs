using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using Game.BaseStructures;
using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.Enums;
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
                Picture = GameMethods.ResizeBitmap(Resources.SpearRight, 160, 40);
                Speed = 60;
                x = body.Right;

            }
            else
            {
                Picture = GameMethods.ResizeBitmap(Resources.SpearLeft, 160, 40);
                Speed = -60;
                x = body.Left;
            }
            Position = new PointF(x, y);
        }

        public override bool CheckState(Fighter Opponent)
        {
            if (Speed > 0)
            {
                if (Opponent.Block.Blocking && Opponent.Block.Side == BlockSide.Left) return false;
                if (!this.IfReached(Opponent)) return false;
                Opponent.HealthPoints -= Damage;
                return true;
            }
            if (Opponent.Block.Blocking && Opponent.Block.Side == BlockSide.Right) return false;
            if (!this.IfReached(Opponent)) return false;
            Opponent.HealthPoints -= Damage;
            return true;
        }
    }
}