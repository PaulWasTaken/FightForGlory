using System;
using System.Windows.Forms;
using Game.BaseStructures.AbstractClasses;
using Game.Figters;
using Game.GameInformation;
using Game.Properties;

namespace Game.GameWindows
{
    public partial class ScreenSelector : Form
    {
        private bool isFirstPlayer = true;
        private bool isSecondPlayer = true;

        public Fighter FirstPlayer;
        public Fighter SecondPlayer;

        public ScreenSelector()
        {
            InitializeComponent();
            DoubleBuffered = true;
            var timer = new Timer {Interval = TimeSpan.FromMilliseconds(30).Milliseconds};
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isFirstPlayer)
            {
                FirstPlayer = new Necromancer("Necromancer", this.Width / 2 - 200, this.Height / 1.5f);
                isFirstPlayer = false;
                return;
            }
            if (isSecondPlayer)
            {
                SecondPlayer = new Necromancer("Necromancer", this.Width / 2 + 200, this.Height / 1.5f);
                isSecondPlayer = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isFirstPlayer)
            {
                FirstPlayer = new Paladin("Paladin", this.Width / 2 - 200, this.Height / 1.5f);
                isFirstPlayer = false;
                return;
            }
            if (isSecondPlayer)
            {
                SecondPlayer = new Paladin("Paladin", this.Width / 2 + 200, this.Height / 1.5f);
                isSecondPlayer = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (isFirstPlayer)
            {
                FirstPlayer = new Skeleton("Skeleton", this.Width / 2 - 200, this.Height / 1.5f);
                isFirstPlayer = false;
                return;
            }
            if (isSecondPlayer)
            {
                SecondPlayer = new Skeleton("Skeleton", this.Width / 2 + 200, this.Height / 1.5f);
                isSecondPlayer = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (isFirstPlayer)
            {
                FirstPlayer = new Unicorn("Unicorn", this.Width / 2 - 200, this.Height / 1.5f);
                isFirstPlayer = false;
                return;
            }
            if (isSecondPlayer)
            {
                SecondPlayer = new Unicorn("Unicorn", this.Width / 2 + 200, this.Height / 1.5f);
                isSecondPlayer = false;
            }
        }

        protected override void OnPaint(PaintEventArgs g)
        {
            if (!isFirstPlayer && !isSecondPlayer)
            {
                g.Graphics.DrawImage(Resources.PaladinRight, (int)(this.Width / 2), (int)(this.Height / 2));
            }            
        }

        private void TimerTick(object sender, EventArgs args)
        {
            if (isFirstPlayer || isSecondPlayer) return;
            var timer = new Timer {Interval = 10};
            timer.Tick += LetTheGameBegins;
            timer.Start();
        }

        private void LetTheGameBegins(object sender, EventArgs args)
        {
            Close();
        }
    }
}
