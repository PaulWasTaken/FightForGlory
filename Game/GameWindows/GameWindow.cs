using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.Enums;
using Game.GameInformation;

namespace Game.GameWindows
{
    public class GameWindow : Form
    {
        private readonly GameSettings settings;
        private GameState gameState;
        private readonly GameController gameController;
        public GameWindow(int width, int height)
        {
            DoubleBuffered = true;
            var backGround = new DirectoryInfo("Images").GetFiles("Background.jpg");
            StartPosition = FormStartPosition.CenterScreen;
            BackgroundImage = new Bitmap(Image.FromFile(backGround.First().FullName));
            var icon = new DirectoryInfo("Images").GetFiles("Swords.ico");
            MaximizeBox = false;
            Icon = new Icon(icon.First().FullName);
            Width = width;
            Height = height;
            Visible = false;

            settings = new GameSettings(width, height);

            using (var selector = new ScreenSelector())
            {
                selector.ShowDialog();
                gameState = new GameState(selector.FirstPlayer, selector.SecondPlayer);
            }

            gameController = new GameController(settings, gameState);

            settings.DictWithComboControllers[PlayerNumber.FirstPlayer] = gameState.FirstPlayer.GetCombos();
            settings.DictWithComboControllers[PlayerNumber.SecondPlayer] = gameState.SecondPlayer.GetCombos();

            var timer = new Timer {Interval = 20};
            timer.Tick += TimerTick;
            timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (gameState.Finished)
            {
                e.Graphics.DrawString(gameState.Lost.Item2 + " won!", new Font("Arial", 20), 
                    Brushes.Green, GameSettings.Resolution.X / 2 - 200, GameSettings.Resolution.Y / 2.0f);
                return;
            }
            if (gameState.Lost.Item1)
            {
                var timer = new Timer { Interval = 2000 };
                timer.Tick += RestartTheGame;
                timer.Start();
                gameState.Finished = true;
            }

            foreach (var fighter in gameState.Fighters)
            {
                e.Graphics.DrawImage(fighter.CurrentImage, fighter.Body.Location);
                DrawBars(fighter, e);
            }

            foreach (var obj in gameState.GameObjects)
            {
                obj.Move();
                if (obj.CheckState())
                {
                    gameState.GameObjects.Remove(obj);
                    break;
                }
                e.Graphics.DrawImage(obj.Picture, obj.Position.X, obj.Position.Y);
            }

            foreach (var strike in gameState.SpecialStrikes)
            {
                if (strike.IfReached())
                {
                    strike.FixPicture();
                    gameState.SpecialStrikes.Remove(strike);
                    break;
                }
            }
        }

        private void DrawBars(Fighter fighter, PaintEventArgs e)
        {
            if (fighter.Number == PlayerNumber.FirstPlayer)
            {
                e.Graphics.DrawRectangle(new Pen(Color.Black), 0, 0, GameSettings.Resolution.X / 2.0f - settings.XIndent, 20);
                e.Graphics.FillRectangle(Brushes.Red, 2, 2, 
                    fighter.Body.Width * fighter.HealthPoints / 100 * 8 - settings.XIndent, 18);

                e.Graphics.DrawRectangle(new Pen(Color.Black), 0, 20, GameSettings.Resolution.X / 2.0f - settings.XIndent, 20);
                e.Graphics.FillRectangle(Brushes.Blue, 2, 22, 
                    fighter.Body.Width * fighter.ManaPoints / 100 * 8 - settings.XIndent, 18);
            }
            else
            {
                e.Graphics.DrawRectangle(new Pen(Color.Black), GameSettings.Resolution.X / 2.0f, 0,
                    GameSettings.Resolution.X / 2.0f - settings.XIndent, 20);
                e.Graphics.FillRectangle(Brushes.Red, GameSettings.Resolution.X / 2.0f, 2, 
                    fighter.Body.Width * fighter.HealthPoints / 100 * 8 - settings.XIndent, 18);

                e.Graphics.DrawRectangle(new Pen(Color.Black), GameSettings.Resolution.X / 2.0f, 20,
                    GameSettings.Resolution.X / 2.0f - settings.XIndent, 20);
                e.Graphics.FillRectangle(Brushes.Blue, GameSettings.Resolution.X / 2.0f, 22, 
                    fighter.Body.Width * fighter.ManaPoints / 100 * 8 - settings.XIndent, 18);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            gameController.KeyDown(e);
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            gameController.KeyUp(e);
            base.OnKeyUp(e);
        }

        private void TimerTick(object sender, EventArgs args)
        {
            foreach (var fighter in gameState.Fighters)
            {
                if (gameState.SpecialStrikes.Count != 0)
                    break;
                /*
                if (fighter.State == FighterMotionState.MovingLeft)
                    fighter.Update((int)fighter.State * 10, settings.Resolution.X);
                if (fighter.State == FighterMotionState.MovingRight)
                    fighter.Update(10, settings.Resolution.X);
                    */
                fighter.Update((int)fighter.State * 10);
                fighter.ToTheGround();
                fighter.ManaRegeneration();
                if (fighter.HealthPoints <= 0)
                    gameState.Lost = Tuple.Create(true, fighter.Opponent.Number.ToString());
            }           
            Invalidate();
        }

        private void RestartTheGame(object sender, EventArgs args)
        {
            Application.Restart();
        }
    }
}