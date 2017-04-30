using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class GameMethods
    {
        public static Image ResizeBitmap(Image image, double newWidth, double newHeight)
        {
            Image result = new Bitmap((int)newWidth, (int)newHeight);
            using (var g = Graphics.FromImage((Image)result))
                g.DrawImage(image, 0, 0, (int)newWidth, (int)newHeight);
            return result;
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
    }
}
