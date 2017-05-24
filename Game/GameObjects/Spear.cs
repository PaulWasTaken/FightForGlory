using Game;
using Game.BaseStructures;
using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.Enums;
using Game.Properties;

public class Spear : GameObject
{
    public override int Damage => 20;

    public Spear(HitBox body, bool lookRight, Fighter enemy)
    {
        Opponent = enemy;
        Y = body.BotRightY - (body.BotRightY - body.TopLeftY) / 1.5f;
        if (lookRight)
        {
            Picture = GameMethods.ResizeBitmap(Resources.SpearRight, 160, 40);
            Speed = 60;
            X = body.BotRightX;

        }
        else
        {
            Picture = GameMethods.ResizeBitmap(Resources.SpearLeft, 160, 40);
            Speed = -60;
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