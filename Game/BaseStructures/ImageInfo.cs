using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Game.GameInformation;

namespace Game.BaseStructures
{
    public class ImageInfo
    {
        public Image Left { get; private set; }
        public Image Right { get; private set; }
        public Image AttackRight { get; private set; }
        public Image AttackLeft { get; private set; }
        public Image BlockLeft { get; private set; }
        public Image BlockRight { get; private set; }

        private Image MoveLeft { get; set; }
        private Image MoveRight { get; set; }

        private int counter = 1;

        private Image[] MovingRight;
        private Image[] MovingLeft;

        private static Image ResizeImage(Image imgToResize, Size size)
        {
            return new Bitmap(imgToResize, size);
        }

        public static ImageInfo CreateFigtherInfo(string name)
        {
            var properties = typeof(ImageInfo).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var size = new Size(GameSettings.Resolution.X / 10, (int)(GameSettings.Resolution.Y / 4.5));
            var info = new ImageInfo(name, properties, size);
            info.MovingLeft = new[] { info.Left, info.MoveLeft };
            info.MovingRight = new[] { info.Right, info.MoveRight };
            return info;
        }

        public static ImageInfo CreateGameObjectInfo(string name)
        {
            var left = typeof(ImageInfo).GetProperty("Left");
            var right = typeof(ImageInfo).GetProperty("Right");
            var size = new Size(GameSettings.Resolution.X / 10, (int)(GameSettings.Resolution.Y / 4.5));
            var info = new ImageInfo(name, new []{left, right}, size);
            return info;
        }

        private ImageInfo(string name, IEnumerable<PropertyInfo> properties, Size pictureSize)
        {
            foreach (var property in properties)
            {
                var propertyInfo = typeof(Properties.Resources).GetProperty(name + property.Name);
                var image = (Bitmap)propertyInfo.GetValue(null, null);
                property.SetValue(this, ResizeImage(image, pictureSize));
            }
        }

        public Image GetMovingImage(bool movingRight)
        {
            var image = movingRight ? MovingRight : MovingLeft;
            if (counter < 4)
            {
                counter += 1;
                return image.First();
            }
            if (counter < 6)
            {
                counter += 1;
                return image.Last();
            }
            counter = 1;
            return image.First();
        }
    }
}
