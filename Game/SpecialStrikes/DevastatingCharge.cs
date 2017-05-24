using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.Enums;
using Game.GameInformation;

namespace Game.SpecialStrikes
{
    public class DevastatingCharge : SpecialStrike
    {
        private float Peak { get; }
        private float Delta { get; }
        public DevastatingCharge(Fighter master)
        {
            Source = master;
            Range = Source.AttackRange;
            Damage = Source.AttackDamage;
            var dx = GameSettings.Resolution.X / 4;
            Delta = dx / 3f;
            if (Source.LookRight)
            {
                Source.PreviousImage = Source.Picture.Right;
                Peak = Source.X + dx;
            }
            else
            {
                Source.PreviousImage = Source.Picture.Left;
                Peak = Source.X - dx;
            }
        }

        public override bool IfReached()
        {
            if (Source.LookRight)
            {
                if (!(Source.X <= Peak) || !(Source.X < GameSettings.Resolution.X)) return true;
                Source.X += Delta;
                Source.Body.BotRightX += Delta;
                Source.Body.TopLeftX += Delta;
                if (Source.Opponent.Block.Blocking && 
                    Source.Opponent.Block.Side == BlockSide.Left) return false;
                if (Source.Body.BotRightX + Range >= Source.Opponent.Body.TopLeftX &&
                    Source.Body.BotRightX + Range <= Source.Opponent.Body.BotRightX)
                {
                    Source.Opponent.HealthPoints -= Damage;
                    return true;
                }
                return false;
            }
            else
            {
                if (Source.X >= Peak && Source.X > 0)
                {
                    Source.X -= Delta;
                    Source.Body.BotRightX -= Delta;
                    Source.Body.TopLeftX -= Delta;
                    if (Source.Opponent.Block.Blocking &&
                        Source.Opponent.Block.Side == BlockSide.Right) return false;
                    if (Source.Body.TopLeftX - Range <= Source.Opponent.Body.BotRightX &&
                        Source.Body.TopLeftX - Range >= Source.Opponent.Body.TopLeftX)
                    {
                        Source.Opponent.HealthPoints -= Damage;
                        return true;
                    }
                    return false;
                }
                return true;
            }
        }
    }
}
