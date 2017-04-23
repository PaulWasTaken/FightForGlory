using Game.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class ScreenSelector : Form
    {
        private bool IsFirstPlayer = true;
        private bool IsSecondPlayer = true;
        public ScreenSelector()
        {
            InitializeComponent();
            DoubleBuffered = true;
            var timer = new Timer();
            timer.Interval = TimeSpan.FromMilliseconds(30).Milliseconds;
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsFirstPlayer)
            {
                GameWindow.FirstPlayer = new Necromancer("Necromancer", this.Width / 2 - 200, this.Height / 1.5f);
                IsFirstPlayer = false;
                return;
            }
            if (IsSecondPlayer)
            {
                GameWindow.SecondPlayer = new Necromancer("Necromancer", this.Width / 2 + 200, this.Height / 1.5f);
                IsSecondPlayer = false;
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (IsFirstPlayer)
            {
                GameWindow.FirstPlayer = new Paladin("Paladin", this.Width / 2 - 200, this.Height / 1.5f);
                IsFirstPlayer = false;
                return;
            }
            if (IsSecondPlayer)
            {
                GameWindow.SecondPlayer = new Paladin("Paladin", this.Width / 2 + 200, this.Height / 1.5f);
                IsSecondPlayer = false;
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (IsFirstPlayer)
            {
                GameWindow.FirstPlayer = new Skeleton("Skeleton", this.Width / 2 - 200, this.Height / 1.5f);
                IsFirstPlayer = false;
                return;
            }
            if (IsSecondPlayer)
            {
                GameWindow.SecondPlayer = new Skeleton("Skeleton", this.Width / 2 + 200, this.Height / 1.5f);
                IsSecondPlayer = false;
                return;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (IsFirstPlayer)
            {
                GameWindow.FirstPlayer = new Unicorn("Unicorn", this.Width / 2 - 200, this.Height / 1.5f);
                IsFirstPlayer = false;
                return;
            }
            if (IsSecondPlayer)
            {
                GameWindow.SecondPlayer = new Unicorn("Unicorn", this.Width / 2 + 200, this.Height / 1.5f);
                IsSecondPlayer = false;
                return;
            }
        }

        protected override void OnPaint(PaintEventArgs g)
        {
            if (!IsFirstPlayer && !IsSecondPlayer)
            {
                g.Graphics.DrawImage(Resources.PaladinRight, (int)(this.Width / 2), (int)(this.Height / 2));
            }            
        }

        void TimerTick(object sender, EventArgs args)
        {
            if (!IsFirstPlayer && !IsSecondPlayer)
            {
                var timer = new Timer();
                timer.Interval = 10;
                timer.Tick += LetTheGameBegins;
                timer.Start();
            }
        }

        void LetTheGameBegins(object sender, EventArgs args)
        {
            Close();
        }
    }
}
