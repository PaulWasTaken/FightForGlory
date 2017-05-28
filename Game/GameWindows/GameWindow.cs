using System;
using System.Drawing;
using System.Windows.Forms;
using Game.BaseStructures.Enums;
using Game.Controllers;
using Game.Factories;
using Game.Fighters;
using Game.GameInformation;

namespace Game.GameWindows
{
    public sealed class GameWindow : Form
    {
        private GameSettings settings;
        private GameState gameState;
        private GameController gameController;
        private bool gameStarted;

        private readonly FighterFactory fighterFactory;
        private readonly GameSettingsFactory settingsFactory;
        private readonly GameStateFactory stateFactory;
        private readonly GameControllerFactory controllerFactory;

        public GameWindow(FighterFactory fighterFactory, GameSettingsFactory settingsFactory,
            GameStateFactory stateFactory, GameControllerFactory controllerFactory)
        {
            this.fighterFactory = fighterFactory;
            this.settingsFactory = settingsFactory;
            this.stateFactory = stateFactory;
            this.controllerFactory = controllerFactory;

            ConfigureWindow();

            DrawMainMenu();
        }

        private void ConfigureWindow()
        {
            WindowState = FormWindowState.Maximized;
            //FormBorderStyle = FormBorderStyle.None;
            Width = SystemInformation.VirtualScreen.Width;
            Height = SystemInformation.VirtualScreen.Height;
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            DoubleBuffered = true;
            Icon = Properties.Resources.Swords;
        }

        public void DrawMainMenu()
        {
            BackgroundImage = Properties.Resources.MMBackground.Resize(Width, Height);
            var mainMenu = new MainMenu();
            Controls.Add(mainMenu);
        }

        public void DrawCharactedSelectMenu()
        {
            Controls.Clear();
            var selectMenu = new CharacterSelectMenu();
            Controls.Add(selectMenu);
        }

        public void StartGame(Type[] players)
        {
            Controls.Clear();
            gameStarted = true;
            BackgroundImage = Properties.Resources.Background.Resize(Width, Height);

            settings = settingsFactory.Create(); // HELL YEAH STATIC PROPETIES

            var firstPlayer = fighterFactory.CreateFighter(players[0], PlayerNumber.FirstPlayer);
            var secondPlayer = fighterFactory.CreateFighter(players[1], PlayerNumber.SecondPlayer);

            //settings = settingsFactory.Create(); // STATIC FUCKING PROPETIES. OF COUSE YOU NEED TO CREATE SETTINGS FIRST. WHY? BECAUSE FUCK YOU

            gameState = stateFactory.Create(firstPlayer, secondPlayer);
            gameController = controllerFactory.Create(settings, gameState);

            settings.AddControllersForPlayer(firstPlayer);
            settings.AddControllersForPlayer(secondPlayer);
            settings.AddControllersForObjects();

            var timer = new Timer {Interval = 50}; //Kinda fps
            timer.Tick += TimerTick;
            timer.Start();
        }

        // Solves the problem of menu flickering
        protected override CreateParams CreateParams
        {
            get
            {
                var handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return handleParam;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!gameStarted)
                return;
            if (gameState.Finished)
            {
                e.Graphics.DrawString(gameState.Lost.Item2 + " won!", new Font("Arial", 20),
                    Brushes.Green, GameSettings.Resolution.X / 5f, GameSettings.Resolution.Y / 2.0f);
                return;
            }
            if (gameState.Lost.Item1)
            {
                var timer = new Timer {Interval = 2000};
                timer.Tick += RestartTheGame;
                timer.Start();
                gameState.Finished = true;
            }

            foreach (var fighter in gameState.Fighters)
            {
                e.Graphics.DrawImage(settings.GetImageController(fighter.Number).CurrentImage, fighter.Body.Location);
                DrawBars(fighter, e);
            }

            foreach (var obj in gameState.GameObjects)
            {
                var imageController = settings.GetObjImageController(obj.GetType().Name);
                var image = imageController.GetCurrentObjImage(obj);
                e.Graphics.DrawImage(image, obj.Size.Location);
            }
        }

        private void DrawBars(Fighter fighter, PaintEventArgs e)
        {
            if (fighter.Number == PlayerNumber.FirstPlayer)
                DrawHpAndManaBars(e.Graphics, fighter, 0, 0, GameSettings.Resolution.X / 2.0f,
                    GameSettings.Resolution.Y / 45f);
            else
                DrawHpAndManaBars(e.Graphics, fighter, GameSettings.Resolution.X / 2.0f, 0,
                    GameSettings.Resolution.X / 2.0f, GameSettings.Resolution.Y / 45f);
        }

        private void DrawHpAndManaBars(Graphics drawer, Fighter fighter, float x, float y, float width, float height)
        {
            drawer.DrawRectangle(new Pen(Color.Black), x, y, width - settings.XIndent, height);
            drawer.FillRectangle(Brushes.Red, x + 2, y + 2,
                fighter.Body.Width * fighter.HealthPoints / 100 * 8 - settings.XIndent, height - 2);

            drawer.DrawRectangle(new Pen(Color.Black), x, y + height + 2,
                GameSettings.Resolution.X / 2.0f - settings.XIndent, height);
            drawer.FillRectangle(Brushes.Blue, x + 2, y + height + 4,
                fighter.Body.Width * fighter.ManaPoints / 100 * 8 - settings.XIndent, height - 2);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (gameStarted)
                gameController.KeyDown(e);
            e.SuppressKeyPress = true;
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (gameStarted)
                gameController.KeyUp(e);
            e.SuppressKeyPress = true;
            base.OnKeyUp(e);
        }

        private void TimerTick(object sender, EventArgs args)
        {
            gameController.UpdateGameState();
            Invalidate();
        }

        private void RestartTheGame(object sender, EventArgs args)
        {
            Application.Restart();
        }
    }
}