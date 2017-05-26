using System;
using System.Drawing;
using Game.BaseStructures.AbstractClasses;
using Game.GameInformation;
using Game.Properties;

namespace Game.GameObjects
{
    public class Bolt : GameObject
    {
        public override int Damage => 40;
        private bool isFirstTime = true;

        public Bolt(RectangleF body, bool lookRight, Fighter enemy)
        {
            Opponent = enemy;
            var y = body.Top;
            float x;
            if (lookRight)
            {
                x = body.Right;
                var distance = enemy.Body.Left - body.Right;
                if (distance < 0)
                {
                    distance = GameSettings.Resolution.X - body.Right;
                    x = GameSettings.Resolution.X;
                }
                Picture = Resources.LightningRight.Resize(distance, body.Height);
            }
            else
            {
                var distance = body.Left - enemy.Body.Right;
                if (distance < 0)
                {
                    distance = body.Right;
                    x = 0;
                }
                else
                    x = enemy.Body.Right;
                Picture = Resources.LightningLeft.Resize(distance, body.Height);
            }
            Position = new PointF(x, y);
        }

        public override bool CheckState()
        {
            if (!isFirstTime) return true;
            if (Math.Abs(Position.X) > 0.01 && Math.Abs(Position.Y - GameSettings.Resolution.X) > 0.01)
                if (Opponent.Body.Bottom > Position.Y)
                    Opponent.HealthPoints -= Damage;
            isFirstTime = false;
            return false;
        }
    }
}
