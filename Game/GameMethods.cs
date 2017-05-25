using System.Drawing;
using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.Enums;
using Game.GameInformation;

namespace Game
{
    public class GameMethods
    {
        public static Image ResizeBitmap(Image image, double newWidth, double newHeight)
        {
            Image result = new Bitmap((int)newWidth, (int)newHeight);
            using (var g = Graphics.FromImage(result))
                g.DrawImage(image, 0, 0, (int)newWidth, (int)newHeight);
            return result;
        }

        public static RectangleF MoveRect(RectangleF target, float dx, float dy)
        {
            var rect = target;
            rect.Offset(dx, dy);
            return rect;
        }
    }
    public static class Extansions
    {
        public static void ChooseYourSide(this Fighter fighter, FighterMotionState side)
        {
            if (fighter.LookRight && side == FighterMotionState.MovingLeft)
            {
                fighter.LookRight = false;
                fighter.CurrentImage = fighter.Picture.Left;
            }
            else if (side == FighterMotionState.MovingRight)
            {
                fighter.LookRight = true;
                fighter.CurrentImage = fighter.Picture.Right;
            }
        }

        public static bool IsMovementAllowed(this Fighter fighter, float dx, float dy, Fighter opponent)
        {
            var newFighterPos = new RectangleF(fighter.Body.X - fighter.Body.Width / 2 + dx, 
                                               fighter.Body.Y - fighter.Body.Height / 2 + dy,
                                               fighter.Body.Width, fighter.Body.Height);

            var opponentPos = new RectangleF(opponent.Body.X - opponent.Body.Width / 2, opponent.Body.Y - opponent.Body.Height / 2,
                opponent.Body.Width, opponent.Body.Height);
            var leftScreenBorder = new PointF(0, 2 * GameSettings.Resolution.Y / 3f);
            var rightScreenBorder = new PointF(GameSettings.Resolution.X, 2 * GameSettings.Resolution.Y / 3f);
            var notAllowed = newFighterPos.IntersectsWith(opponentPos) ||
                          newFighterPos.Contains(leftScreenBorder) ||
                          newFighterPos.Contains(rightScreenBorder);
            return !notAllowed;
        }

        public static bool IfInTheScreen(this Fighter fighter, float dx, float dy)
        {
            var newFighterPos = new RectangleF(fighter.Body.X - fighter.Body.Width / 2 + dx,
                                               fighter.Body.Y - fighter.Body.Height / 2 + dy,
                                               fighter.Body.Width, fighter.Body.Height);

            var leftScreenBorder = new PointF(0, GameSettings.Resolution.Y / 3f);
            var rightScreenBorder = new PointF(GameSettings.Resolution.X, GameSettings.Resolution.Y / 3f);
            return !(newFighterPos.Contains(leftScreenBorder) ||
                     newFighterPos.Contains(rightScreenBorder));
        }

        public static bool IfReached(this GameObject obj, Fighter enemy)
        {
            return enemy.Body.Contains(obj.Position.X, obj.Position.Y);
        }
    }
}
