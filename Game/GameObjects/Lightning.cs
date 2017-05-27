using System;
using System.Drawing;
using Game.BaseStructures.Enums;
using Game.Fighters;
using Game.GameInformation;

namespace Game.GameObjects
{
    public class Lightning : GameObject
    {
        public override int Damage => 40;
        private bool isFirstTime = true;

        public Lightning(RectangleF body, bool lookRight, PlayerNumber source)
        {
            Source = source;
            var y = body.Top;
            var x = lookRight ? body.Right : body.Left;
            Size = new RectangleF(x, y, 400, 100);  //Need to get the distance to the enemy
        }

        public override bool CheckState(Fighter opponent)
        {
            if (!isFirstTime) return true;
            if (Math.Abs(Size.X) > 0.01 && Math.Abs(Size.Y - GameSettings.Resolution.X) > 0.01)
                if (opponent.Body.Bottom > Size.Y)
                    opponent.HealthPoints -= Damage;
            isFirstTime = false;
            return false;
        }
    }
}
