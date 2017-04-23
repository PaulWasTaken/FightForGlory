using Game.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Wisp : Object
    {
        public override int Damage { get { return 20;} }

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
                if (!Opponent.Block.Blocking || Opponent.Block.Side != BlockSide.Left)
                    if (X >= Opponent.Body.TopLeftX && X <= Opponent.Body.BotRightX)
                    {
                        Opponent.HealthPoints -= Damage;
                        return true;
                    }
            }
            else
            {
                if (!Opponent.Block.Blocking || Opponent.Block.Side != BlockSide.Right)
                    if (X <= Opponent.Body.BotRightX && X >= Opponent.Body.TopLeftX)
                    {
                        Opponent.HealthPoints -= Damage;
                        return true;
                    }
            }
            return false;
        }
    }
    public class Spear : Object
    {
        public override int Damage { get { return 20;} }

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
                if (!Opponent.Block.Blocking || Opponent.Block.Side != BlockSide.Left)
                    if (X >= Opponent.Body.TopLeftX && X <= Opponent.Body.BotRightX)
                    {
                        Opponent.HealthPoints -= Damage;
                        return true;
                    }
            }
            else
            {
                if (!Opponent.Block.Blocking || Opponent.Block.Side != BlockSide.Right)
                    if (X <= Opponent.Body.BotRightX && X >= Opponent.Body.TopLeftX)
                    {
                        Opponent.HealthPoints -= Damage;
                        return true;
                    }
            }
            return false;
        }
    }
    public class Bolt : Object
    {
        public override int Damage { get { return 40; } }
        private bool IsFirtsTime = true;

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
                    distance = GameWindow.Resolution.X - body.BotRightX;
                    X = GameWindow.Resolution.X;
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
            if (IsFirtsTime)
            {
                if (X != 0 && X != GameWindow.Resolution.X)
                    if (Opponent.Body.BotRightY > Y)
                        Opponent.HealthPoints -= Damage;
                IsFirtsTime = false;
                return false;
            }
            return true;
        }
    }
}
