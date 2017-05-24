using Game.BaseStructures;
using Game.BaseStructures.AbstractClasses;
using Game.GameInformation;
using Game.Properties;

namespace Game.GameObjects
{
    public class Bolt : GameObject
    {
        public override int Damage => 40;
        private bool isFirstTime = true;

        public Bolt(HitBox body, bool lookRight, Fighter enemy)
        {
            Opponent = enemy;
            var height = body.BotRightY - body.TopLeftY;
            Y = body.BotRightY - (body.BotRightY - body.TopLeftY);
            if (lookRight)
            {
                X = body.BotRightX;
                var distance = enemy.Body.TopLeftX - body.BotRightX;
                if (distance < 0)
                {
                    distance = GameSettings.Resolution.X - body.BotRightX;
                    X = GameSettings.Resolution.X;
                }
                Picture = GameMethods.ResizeBitmap(Resources.LightningRight, distance, height);
            }
            else
            {
                X = body.TopLeftX;
                var distance = body.TopLeftX - enemy.Body.BotRightX;
                if (distance < 0)
                {
                    distance = body.BotRightX;
                    X = 0;
                }
                else
                    X = enemy.Body.BotRightX;
                Picture = GameMethods.ResizeBitmap(Resources.LightningLeft, distance, height);
            }
        }

        public override bool CheckState()
        {
            if (isFirstTime)
            {
                if (X != 0 && X != GameSettings.Resolution.X)
                    if (Opponent.Body.BotRightY > Y)
                        Opponent.HealthPoints -= Damage;
                isFirstTime = false;
                return false;
            }
            return true;
        }
    }
}
