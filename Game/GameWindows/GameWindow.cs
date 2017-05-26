using System;
using System.Drawing;
using System.Windows.Forms;
using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.Enums;
using Game.Controllers;
using Game.GameInformation;

namespace Game.GameWindows
{
    public sealed class GameWindow : Form
    {
        private GameSettings settings;
        private GameState gameState;
        private GameController gameController;
        private bool gameStarted;

        private readonly PointF firstPlayerLocation;
        private readonly PointF secondPlayerLocation;

        public GameWindow()
        {
            Width = SystemInformation.VirtualScreen.Width;
            Height = SystemInformation.VirtualScreen.Height;
            firstPlayerLocation = new PointF(Width / 2 - 200, Height / 1.5f);
            secondPlayerLocation = new PointF(Width / 2 + 200, Height / 1.5f);
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            DoubleBuffered = true;
            Icon = Properties.Resources.Swords;
            DrawMainMenu();
        }

        // SHIT JUST GOT REAL
        // Actually solves the problem of menu flickering
        // No idea how it works https://stackoverflow.com/questions/8046560/how-to-stop-flickering-c-sharp-winforms
        protected override CreateParams CreateParams
        {
            get
            {
                var handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED       
                return handleParam;
            }
        }

        public void DrawMainMenu()
        {
            BackgroundImage = new Bitmap(Properties.Resources.MMBackground, new Size(Width, Height));

            var mainMenu = new MainMenu(this);
            Controls.Add(mainMenu);
        }

        public void DrawCharactedSelectMenu()
        {
            Controls.Clear();

            var selectMenu = new CharacterSelectMenu(this);
            Controls.Add(selectMenu);
        }

        public void StartGame(Type[] players)
        {
            Controls.Clear();
            gameStarted = true;
            BackgroundImage = new Bitmap(Properties.Resources.Background, new Size(Width, Height));
            settings = new GameSettings(Width, Height);
            var firstPlayer = CreateFighter(players[0], firstPlayerLocation);
            var secondPlayer = CreateFighter(players[1], secondPlayerLocation);
            gameState = new GameState(firstPlayer, secondPlayer);
            gameController = new GameController(settings, gameState);
            settings.DictWithComboControllers[PlayerNumber.FirstPlayer] = gameState.FirstPlayer.GetCombos();
            settings.DictWithComboControllers[PlayerNumber.SecondPlayer] = gameState.SecondPlayer.GetCombos();

            settings.DictWithImageChangers[PlayerNumber.FirstPlayer] = new ImageController(gameState.FirstPlayer);
            settings.DictWithImageChangers[PlayerNumber.SecondPlayer] = new ImageController(gameState.SecondPlayer);

            settings.DictWithImageChangers[PlayerNumber.FirstPlayer] = new ImageController(gameState.FirstPlayer);
            settings.DictWithImageChangers[PlayerNumber.SecondPlayer] = new ImageController(gameState.SecondPlayer);

            var timer = new Timer {Interval = 20};
            timer.Tick += TimerTick;
            timer.Start();
        }

        private static Fighter CreateFighter(Type type, PointF location)
        {
            var constuctor = type.GetConstructor(new[] { typeof(string), typeof(float), typeof(float) });
            // ReSharper disable once PossibleNullReferenceException
            return (Fighter)constuctor.Invoke(new object[] { type.Name, location.X, location.Y });
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!gameStarted)
                return;
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
                e.Graphics.DrawImage(settings.DictWithImageChangers[fighter.Number].CurrentImage, fighter.Body.Location);
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
                    fighter.Move((int)fighter.State * 10, settings.Resolution.X);
                if (fighter.State == FighterMotionState.MovingRight)
                    fighter.Move(10, settings.Resolution.X);
                    */
                fighter.Move((int)fighter.State * 10);
                settings.DictWithImageChangers[fighter.Number].UpdateFighterImage();
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