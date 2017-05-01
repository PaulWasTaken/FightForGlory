using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Game.GameWindows;

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

        private Image[] MovingRight { get; set; }
        private Image[] MovingLeft { get; set; }

        private void FormImageInfo(Image right, Image left, Image attackRight, Image atackLeft, Image blockRight,
            Image blockLeft, Image moveRight, Image moveLeft)
        {
            Right = right;
            Left = left;
            AttackRight = attackRight;
            AttackLeft = atackLeft;
            BlockRight = blockRight;
            BlockLeft = blockLeft;
            MoveRight = moveRight;
            MoveLeft = moveLeft;
            MovingLeft = new[] {left, moveLeft};
            MovingRight = new[] {right, moveRight};
        }

        public ImageInfo(string name)
        {
            var imgRight = new DirectoryInfo("Images").GetFiles(name + "Right.png");
            var imgLeft = new DirectoryInfo("Images").GetFiles(name + "Left.png");
            var imgAttackRight = new DirectoryInfo("Images").GetFiles(name + "AttackRight.png");
            var imgAttackLeft = new DirectoryInfo("Images").GetFiles(name + "AttackLeft.png");
            var imgBlockRight = new DirectoryInfo("Images").GetFiles(name + "BlockRight.png");
            var imgBlockLeft = new DirectoryInfo("Images").GetFiles(name + "BlockLeft.png");
            var imgMoveRight = new DirectoryInfo("Images").GetFiles(name + "MoveRight.png");
            var imgMoveLeft = new DirectoryInfo("Images").GetFiles(name + "MoveLeft.png");

            FormImageInfo(
                ResizeBitmap(Image.FromFile(imgRight.First().FullName), Settings.Resolution.X / 10,
                    Settings.Resolution.Y / 4.5),
                ResizeBitmap(Image.FromFile(imgLeft.First().FullName), Settings.Resolution.X / 10,
                    Settings.Resolution.Y / 4.5),
                ResizeBitmap(Image.FromFile(imgAttackRight.First().FullName), Settings.Resolution.X / 10,
                    Settings.Resolution.Y / 4.5),
                ResizeBitmap(Image.FromFile(imgAttackLeft.First().FullName), Settings.Resolution.X / 10,
                    Settings.Resolution.Y / 4.5),
                ResizeBitmap(Image.FromFile(imgBlockRight.First().FullName), Settings.Resolution.X / 10,
                    Settings.Resolution.Y / 4.5),
                ResizeBitmap(Image.FromFile(imgBlockLeft.First().FullName), Settings.Resolution.X / 10,
                    Settings.Resolution.Y / 4.5),
                ResizeBitmap(Image.FromFile(imgMoveRight.First().FullName), Settings.Resolution.X / 10,
                    Settings.Resolution.Y / 4.5),
                ResizeBitmap(Image.FromFile(imgMoveLeft.First().FullName), Settings.Resolution.X / 10,
                    Settings.Resolution.Y / 4.5)
            );
        }

        private Image ResizeBitmap(Image image, double newWidth, double newHeight)
        {
            Image result = new Bitmap((int) newWidth, (int) newHeight);
            using (var g = Graphics.FromImage((Image) result))
                g.DrawImage(image, 0, 0, (int) newWidth, (int) newHeight);
            return result;
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
