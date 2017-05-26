using System.Drawing;
using Game.BaseStructures.AbstractClasses;
using Game.GameInformation;

namespace Game
{
    public static class Extensions
    {
        public static bool IsMovementAllowed(this Fighter fighter, float dx, float dy, Fighter opponent)
        {
            var newFighterPos = new RectangleF(fighter.Body.X + dx, fighter.Body.Y+ dy,
                                               fighter.Body.Width, fighter.Body.Height);

            var notAllowed = newFighterPos.IntersectsWith(opponent.Body) || fighter.IsOutsideScreen(dx, dy);
            return !notAllowed;
        }

        public static bool IsOutsideScreen(this Fighter fighter, float dx, float dy)
        {
            var newFighterPos = new RectangleF(fighter.Body.X + dx, fighter.Body.Y + dy,
                                               fighter.Body.Width, fighter.Body.Height);

            var leftScreenBorder = new RectangleF(-1, 0, 1, GameSettings.Resolution.Y);
            var rightScreenBorder = new RectangleF(GameSettings.Resolution.X - 1, 0, 1, GameSettings.Resolution.Y);

            return newFighterPos.IntersectsWith(leftScreenBorder) || newFighterPos.IntersectsWith(rightScreenBorder);
        }

        public static bool HasReached(this GameObject obj, Fighter enemy)
        {
            return enemy.Body.Contains(obj.Position.X, obj.Position.Y);
        }

        public static RectangleF Move(this RectangleF target, float dx, float dy)
        {
            var rect = target;
            rect.Offset(dx, dy);
            return rect;
        }

        public static Image Resize(this Image image, double newWidth, double newHeight)
        {
            Image result = new Bitmap((int)newWidth, (int)newHeight);
            using (var g = Graphics.FromImage(result))
                g.DrawImage(image, 0, 0, (int)newWidth, (int)newHeight);
            return result;
        }
    }
}
