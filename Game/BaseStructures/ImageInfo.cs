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

        private readonly Image[] MovingRight;
        private readonly Image[] MovingLeft;

        private static Image ResizeImage(Image imgToResize, Size size)
        {
            return new Bitmap(imgToResize, size);
        }

        public ImageInfo(string name)
        {
            var properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var size = new Size(GameSettings.Resolution.X / 10, (int) (GameSettings.Resolution.Y / 4.5));
            foreach (var property in properties)
            {
                var propertyInfo = typeof(Properties.Resources).GetProperty(name + property.Name);
                var image = (Bitmap)propertyInfo.GetValue(null, null);
                property.SetValue(this, ResizeImage(image, size));
            }
            MovingLeft = new[] { Left, MoveLeft };
            MovingRight = new[] { Right, MoveRight };
        }

        public Image GetRight()
        {
            var image = GetRightMove();
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

        public Image GetLeft()
        {
            var image = GetLeftMove();
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

        private IEnumerable<Image> GetRightMove()
        {
            return MovingRight;
        }

        private IEnumerable<Image> GetLeftMove()
        {
            return MovingLeft;
        }
    }
}
