using System.Drawing;
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
                //Source.PreviousImage = Source.Picture.Right;
                Peak = Source.Body.X + dx;
            }
            else
            {
                //Source.PreviousImage = Source.Picture.Left;
                Peak = Source.Body.X - dx;
            }
        }

        public override bool IfReached()
        {
            if (Source.LookRight)
            {
                if (!(Source.Body.X <= Peak) || !(Source.Body.X < GameSettings.Resolution.X)) return true;
                Source.Body = GameMethods.MoveRect(Source.Body, Delta, 0);
                if (Source.Opponent.Block.Blocking && 
                    Source.Opponent.Block.Side == BlockSide.Left) return false;
                if (!Source.Opponent.Body.Contains(Source.Body.Right + Range, Source.Body.Top + Source.Body.Height / 2))
                    return false;
                Source.Opponent.HealthPoints -= Damage;
                return true;
            }
            if (!(Source.Body.X >= Peak) || Source.Body.X <= 0) return true;
            Source.Body = GameMethods.MoveRect(Source.Body, -Delta, 0);
            if (Source.Opponent.Block.Blocking &&
                Source.Opponent.Block.Side == BlockSide.Right) return false;
            if (!Source.Opponent.Body.Contains(Source.Body.Right - Range, Source.Body.Top + Source.Body.Height / 2))
                return false;
            Source.Opponent.HealthPoints -= Damage;
            return true;
        }
    }
}
