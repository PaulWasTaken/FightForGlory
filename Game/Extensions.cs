using System.Drawing;

namespace Game
{
    public static class Extensions
    {
        public static RectangleF Move(this RectangleF target, float dx, float dy)
        {
            var rect = target;
            rect.Offset(dx, dy);
            return rect;
        }

        public static Image Resize(this Image image, double newWidth, double newHeight)
        {
            Image result = new Bitmap((int) newWidth, (int) newHeight);
            using (var g = Graphics.FromImage(result))
                g.DrawImage(image, 0, 0, (int) newWidth, (int) newHeight);
            return result;
        }
    }
}
