using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Game
{
    class GameWindow : Settings
    {
        public GameWindow(int width, int height) : base(width, height)
        {
            var timer = new Timer();
            timer.Interval = 20;
            timer.Tick += TimerTick;
            timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Finished)
            {
                e.Graphics.DrawString(Lost.Item2 + " won!", new Font("Arial", 20), Brushes.Green, Resolution.X / 2 - 200, Resolution.Y / 2);
                return;
            }
            if (Lost.Item1)
            {
                var timer = new Timer() { Interval = 2000 };
                timer.Tick += RestartTheGame;
                timer.Start();
                Finished = true;
            }

            foreach (var fighter in Fighters)
            {
                e.Graphics.DrawImage(fighter.CurrentImage, new PointF(fighter.X, fighter.Y));
                DrawBars(fighter, e);
            }

            foreach (var obj in GameObjects)
            {
                obj.Move();
                if (obj.CheckState())
                {
                    GameObjects.Remove(obj);
                    break;
                }
                e.Graphics.DrawImage(obj.Picture, obj.X, obj.Y);
            }

            foreach (var strike in SpecialStrikes)
            {
                if (strike.IfReached())
                {
                    strike.FixPicture();
                    SpecialStrikes.Remove(strike);
                    break;
                }
            }
        }

        //private void DrawBars(Fighter fighter, PaintEventArgs e)
        //{
        //    e.Graphics.DrawRectangle(new Pen(Color.Black), fighter.Body.TopLeftX, fighter.Body.TopLeftY - 20, fighter.Body.Width, 10);
        //    e.Graphics.FillRectangle(Brushes.Red, fighter.Body.TopLeftX, fighter.Body.TopLeftY - 18, (float)(fighter.Body.Width * fighter.HealthPoints / 100), 8);

        //    e.Graphics.DrawRectangle(new Pen(Color.Black), fighter.Body.TopLeftX, fighter.Body.TopLeftY - 10, fighter.Body.Width, 10);
        //    e.Graphics.FillRectangle(Brushes.Blue, fighter.Body.TopLeftX, fighter.Body.TopLeftY - 8, (float)(fighter.Body.Width * fighter.ManaPoints / 100), 8);

        //    e.Graphics.DrawRectangle(new Pen(Color.Pink), fighter.Body.TopLeftX, fighter.Body.TopLeftY, fighter.Body.Width, fighter.Body.Height);
        //}

        private void DrawBars(Fighter fighter, PaintEventArgs e)
        {
            if (fighter.Number == PlayerNumber.FirstPlayer)
            {
                e.Graphics.DrawRectangle(new Pen(Color.Black), 0, 0, Resolution.X / 2 - XIndent, 20);
                e.Graphics.FillRectangle(Brushes.Red, 2, 2, (float)(fighter.Body.Width * fighter.HealthPoints / 100) * 8 - XIndent, 18);

                e.Graphics.DrawRectangle(new Pen(Color.Black), 0, 20, Resolution.X / 2 - XIndent, 20);
                e.Graphics.FillRectangle(Brushes.Blue, 2, 22, (float)(fighter.Body.Width * fighter.ManaPoints / 100) * 8 - XIndent, 18);
            }
            else
            {
                e.Graphics.DrawRectangle(new Pen(Color.Black), Resolution.X / 2, 0, Resolution.X / 2 - XIndent, 20);
                e.Graphics.FillRectangle(Brushes.Red, Resolution.X / 2, 2, (float)(fighter.Body.Width * fighter.HealthPoints / 100) * 8 - XIndent, 18);

                e.Graphics.DrawRectangle(new Pen(Color.Black), Resolution.X / 2, 20, Resolution.X / 2 - XIndent, 20);
                e.Graphics.FillRectangle(Brushes.Blue, Resolution.X / 2, 22, (float)(fighter.Body.Width * fighter.ManaPoints / 100) * 8 - XIndent, 18);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            foreach (var fighter in Fighters)
            {
                if (SpecialStrikes.Count != 0)
                    break;
                if (fighter.Combos.IncludedValues.Contains(e.KeyData))
                {
                    fighter.CheckForCombo(e);
                }
                if (Cooperator[fighter].Contains(e.KeyData))
                {
                    ButtonHandler[fighter][e.KeyData]();
                    return;
                }                
            }
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if ((e.KeyData == Keys.A && FirstPlayer.State == FighterMotionState.MovingLeft) ||
                (e.KeyData == Keys.D && FirstPlayer.State == FighterMotionState.MovingRight))
                FirstPlayer.State = FighterMotionState.NotMoving;
            if ((e.KeyData == Keys.Left && SecondPlayer.State == FighterMotionState.MovingLeft) ||
                (e.KeyData == Keys.Right && SecondPlayer.State == FighterMotionState.MovingRight))
                SecondPlayer.State = FighterMotionState.NotMoving;

            base.OnKeyUp(e);
        }
    

        void TimerTick(object sender, EventArgs args)
        {
            foreach (var fighter in Fighters)
            {
                if (SpecialStrikes.Count != 0)
                    break;
                if (fighter.State == FighterMotionState.MovingLeft)
                    fighter.Update(-10);
                if (fighter.State == FighterMotionState.MovingRight)
                    fighter.Update(10);
                fighter.ToTheGround();
                fighter.ManaRegeneration();
                if (fighter.HealthPoints <= 0)
                {
                    Lost = Tuple.Create(true, fighter.Opponent.Number.ToString());
                }
            }           
            Invalidate();
        }

        private void RestartTheGame(object sender, EventArgs args)
        {
            Application.Restart();
        }
    }
}