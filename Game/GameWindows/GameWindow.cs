using System;
using System.Drawing;
using System.Windows.Forms;
using Game.BaseStructures.Enums;
using Game.Controllers;
using Game.Fighters;
using Game.GameInformation;

namespace Game.GameWindows
{
    public sealed class GameWindow : Form
    {
        private GameSettings settings;
        private GameState gameState;
        private GameController gameController;
        private CombatController combatController;
        private bool gameStarted;

        private readonly PointF firstPlayerLocation;
        private readonly PointF secondPlayerLocation;

        public GameWindow()
        {
            WindowState = FormWindowState.Maximized;
            //FormBorderStyle = FormBorderStyle.None;
            Width = SystemInformation.VirtualScreen.Width;
            Height = SystemInformation.VirtualScreen.Height;
            firstPlayerLocation = new PointF(Width / 2 - Width / 4, Height / 1.5f);
            secondPlayerLocation = new PointF(Width / 2 + Width / 4, Height / 1.5f);
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
            combatController = new CombatController(firstPlayer, secondPlayer);

            settings.AddControllersForPlayer(firstPlayer);
            settings.AddControllersForPlayer(secondPlayer);
            settings.AddControllersForObjects();

            var timer = new Timer {Interval = 50};  //Kinda fps
            timer.Tick += TimerTick;
            timer.Start();
        }

        private static Fighter CreateFighter(Type type, PointF location)
        {
            var constuctor = type.GetConstructor(new[] { typeof(string), typeof(PointF)});
            // ReSharper disable once PossibleNullReferenceException
            var fighter = (Fighter)constuctor.Invoke(new object[] { type.Name, location });
            return fighter;
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
                var timer = new Timer { Interval = 2000 };
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
                obj.Move();
                if (obj.CheckState(gameState.GetOpponent(obj.Source)))
                {
                    gameState.GameObjects.Remove(obj);
                    break;
                }
                var imageController = settings.GetObjImageController(obj.GetType().Name);
                var image = imageController.GetCurrentObjImage(obj);
                e.Graphics.DrawImage(image, obj.Size.X, obj.Size.Y);
            }
        }

        private void DrawBars(Fighter fighter, PaintEventArgs e)
        {
            if (fighter.Number == PlayerNumber.FirstPlayer)
                DrawHpAndManaBars(e.Graphics, fighter, 0, 0, GameSettings.Resolution.X / 2.0f, GameSettings.Resolution.Y / 45f);
            else
                DrawHpAndManaBars(e.Graphics, fighter, GameSettings.Resolution.X / 2.0f, 0, 
                                  GameSettings.Resolution.X / 2.0f, GameSettings.Resolution.Y / 45f);
        }

        private void DrawHpAndManaBars(Graphics drawer, Fighter fighter, float x, float y, float width, float height)
        {
            drawer.DrawRectangle(new Pen(Color.Black), x, y, width - settings.XIndent, height);
            drawer.FillRectangle(Brushes.Red, x + 2, y + 2,
                fighter.Body.Width * fighter.HealthPoints / 100 * 8 - settings.XIndent, height - 2);

            drawer.DrawRectangle(new Pen(Color.Black), x, y + height + 2, GameSettings.Resolution.X / 2.0f - settings.XIndent, height);
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
                fighter.Move((int)fighter.State * 10, gameState.GetOpponent(fighter.Number));
                settings.GetImageController(fighter.Number).UpdateFighterImage();
                fighter.ToTheGround();
                fighter.RegenerateMana();
                if (fighter.HealthPoints <= 0)
                    gameState.Lost = Tuple.Create(true, gameState.GetOpponent(fighter.Number).Number.ToString());
            }
            combatController.CheckForCombat(gameState.GameObjects);          
            Invalidate();
        }

        private void RestartTheGame(object sender, EventArgs args)
        {
            Application.Restart();
        }
    }
}