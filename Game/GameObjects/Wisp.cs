using Game.BaseStructures;
using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.Enums;
using Game.Properties;

namespace Game.GameObjects
{
    public class Wisp : GameObject
    {
        public override int Damage => 20;

        public Wisp(HitBox body, bool lookRight, Fighter enemy)
        {
            Opponent = enemy;
            Picture = GameMethods.ResizeBitmap(Resources.Wisp, 40, 40);
            Y = body.BotRightY - (body.BotRightY - body.TopLeftY) / 1.5f;
            if (lookRight)
            {
                Speed = 50;
                X = body.BotRightX;
            }
            else
            {
                Speed = -50;
                X = body.TopLeftX;
            }
        }

        public override bool CheckState()
        {
            if (Speed > 0)
            {
                if (Opponent.Block.Blocking && Opponent.Block.Side == BlockSide.Left) return false;
                if (X >= Opponent.Body.TopLeftX && X <= Opponent.Body.BotRightX)
                {
                    Opponent.HealthPoints -= Damage;
                    return true;
                }
            }
            else
            {
                if (Opponent.Block.Blocking && Opponent.Block.Side == BlockSide.Right) return false;
                if (X <= Opponent.Body.BotRightX && X >= Opponent.Body.TopLeftX)
                {
                    Opponent.HealthPoints -= Damage;
                    return true;
                }
            }
            return false;
        }
    }
}
