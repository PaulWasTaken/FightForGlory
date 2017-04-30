using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
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

        private void FormImageInfo(Image right, Image left, Image attackRight, Image atackLeft, Image blockRight, Image blockLeft, Image moveRight, Image moveLeft)
        {
            Right = right;
            Left = left;
            AttackRight = attackRight;
            AttackLeft = atackLeft;
            BlockRight = blockRight;
            BlockLeft = blockLeft;
            MoveRight = moveRight;
            MoveLeft = moveLeft;
            MovingLeft = new[] { left, moveLeft };
            MovingRight = new[] { right, moveRight };
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

            this.FormImageInfo(
                ResizeBitmap(Image.FromFile(imgRight.First().FullName), Settings.Resolution.X / 10, Settings.Resolution.Y / 4.5),
                ResizeBitmap(Image.FromFile(imgLeft.First().FullName), Settings.Resolution.X / 10, Settings.Resolution.Y / 4.5),
                ResizeBitmap(Image.FromFile(imgAttackRight.First().FullName), Settings.Resolution.X / 10, Settings.Resolution.Y / 4.5),
                ResizeBitmap(Image.FromFile(imgAttackLeft.First().FullName), Settings.Resolution.X / 10, Settings.Resolution.Y / 4.5),
                ResizeBitmap(Image.FromFile(imgBlockRight.First().FullName), Settings.Resolution.X / 10, Settings.Resolution.Y / 4.5),
                ResizeBitmap(Image.FromFile(imgBlockLeft.First().FullName), Settings.Resolution.X / 10, Settings.Resolution.Y / 4.5),
                ResizeBitmap(Image.FromFile(imgMoveRight.First().FullName), Settings.Resolution.X / 10, Settings.Resolution.Y / 4.5),
                ResizeBitmap(Image.FromFile(imgMoveLeft.First().FullName), Settings.Resolution.X / 10, Settings.Resolution.Y / 4.5)
                                    );

            //this.FormImageInfo(
            //    ResizeBitmap(Image.FromFile(imgRight.First().FullName), 160, 200),
            //    ResizeBitmap(Image.FromFile(imgLeft.First().FullName), 160, 200),
            //    ResizeBitmap(Image.FromFile(imgAttackRight.First().FullName), 160, 200),
            //    ResizeBitmap(Image.FromFile(imgAttackLeft.First().FullName), 160, 200),
            //    ResizeBitmap(Image.FromFile(imgBlockRight.First().FullName), 160, 200),
            //    ResizeBitmap(Image.FromFile(imgBlockLeft.First().FullName), 160, 200),
            //    ResizeBitmap(Image.FromFile(imgMoveRight.First().FullName), 160, 200),
            //    ResizeBitmap(Image.FromFile(imgMoveLeft.First().FullName), 160, 200)
            //                        );
        }

        private Image ResizeBitmap(Image image, double newWidth, double newHeight)
        {
            Image result = new Bitmap((int)newWidth, (int)newHeight);
            using (var g = Graphics.FromImage((Image)result))
                g.DrawImage(image, 0, 0, (int)newWidth, (int)newHeight);
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
            foreach (var img in MovingRight)
                yield return img;
        }

        private IEnumerable<Image> GetLeftMove()
        {
            return MovingLeft;
        }
    }

    public class HitBox
    {
        public float TopLeftX { get; set;}
        public float TopLeftY { get; set; }
        public float BotRightX { get; set; }
        public float BotRightY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public HitBox(float x, float y)
        {
            TopLeftX = x + 20;
            TopLeftY = y;
            BotRightX = TopLeftX + Settings.Resolution.X / 16;
            BotRightY = y + Settings.Resolution.Y / 4.5f;
            Width = (int)(BotRightX - TopLeftX);
            Height = (int)(BotRightY - TopLeftY);
        }
    }

    public class ComboDetector
    {
        public string Name { get; set; }
        public Node DefaultState { get; set; }
        public Node CurrentState { get; set; }
        private readonly HashSet<Keys> includedTops = new HashSet<Keys>();
        public HashSet<Keys> IncludedValues { get; private set; }

        public ComboDetector(string name)
        {
            DefaultState = new Node {Name = ComboName.Default};
            CurrentState = DefaultState;
            IncludedValues = new HashSet<Keys>();
        }

        public void Add(Keys[] combo, ComboName name)
        {
            DefaultState.NextState.Add(new Node(combo[0], name));
            includedTops.Add(combo[0]);
            IncludedValues.Add(combo[0]);
            var current = DefaultState.NextState.Last();
            for (var i = 1; i < combo.Length; i++)
            {
                IncludedValues.Add(combo[i]);
                current.NextState.Add(new Node(combo[i], name));
                current = current.NextState.Last();
            }
        }

        public bool CheckState(Keys value)
        {
            foreach (var state in CurrentState.NextState)
            {
                CurrentState = state.Value.CompareTo(value) == 0 ? state : DefaultState;
            }
            return value.CompareTo(CurrentState.Value) == 0 && CurrentState.NextState.Count == 0;
        }

        public void FindValue(Keys value)
        {
            if (!includedTops.Contains(value))
                return;
            foreach (var state in DefaultState.NextState)
            {
                if (state.Value.CompareTo(value) == 0)
                    CurrentState = state;
            }
        }
    }

    public class Node
    {
        public ComboName Name { get; set; }
        public List<Node> NextState { get; set; }
        public Keys Value { get; set; }

        public Node(Keys value, ComboName name)
        {
            Value = value;
            NextState = new List<Node>();
            Name = name;  
        }

        public Node()
        {
            NextState = new List<Node>();
        }
}

    //    public class ComboDetector<T> where T: IComparable
    //    {
    //        public string Name { get; set; }
    //        public Node<T> DefaultState { get; set; }
    //        public Node<T> CurrentState { get; set; }
    //        private HashSet<T> IncludedValues = new HashSet<T>();

    //        public ComboDetector(string name)
    //        {
    //            DefaultState = new Node<T>();
    //            Name = name;                
    //        }

    //        public void Add(List<T> combo)
    //        {
    //            DefaultState.NextState.Add(new Node<T>(combo[0]));
    //            IncludedValues.Add(combo[0]);
    //            var current = DefaultState.NextState.Last();
    //            for (var i = 1; i < combo.Count; i++)
    //            {
    //                current.NextState.Add(new Node<T>(combo[i]));
    //                current = current.NextState.Last();                    
    //            }
    //        }

    //        public bool CheckState(T value)
    //        {
    //            foreach (var state in CurrentState.NextState)
    //            {
    //                if (state.Value.CompareTo(value) == 0)
    //                    CurrentState = state;
    //                else
    //                    CurrentState = DefaultState;
    //            }
    //            if (value.CompareTo(CurrentState.Value) == 0 && CurrentState.NextState.Count == 0)
    //            {
    //                CurrentState = DefaultState;
    //                return true;
    //            }
    //            return false;
    //        }

    //        public void FindValue(T value)
    //        {
    //            if (!IncludedValues.Contains(value))
    //                return;
    //            foreach (var state in DefaultState.NextState)
    //            {
    //                if (state.Value.CompareTo(value) == 0)
    //                    CurrentState = state;
    //            }
    //        }
    //    }

    //    public class Node<T> where T : IComparable
    //    {
    //        public List<Node<T>> NextState { get; set; }
    //        public T Value { get; set; }

    //        public Node(T value)
    //        {
    //            Value = value;
    //            NextState = new List<Node<T>>();
    //        }

    //        public Node()
    //        {
    //            NextState = new List<Node<T>>();
    //        }
    //    }
    //}
}
