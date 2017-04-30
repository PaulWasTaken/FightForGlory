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
    class Settings : Form
    {
        public Settings(int width, int height)
        {
            DoubleBuffered = true;
            Lost = Tuple.Create(false, "");
            var backGround = new DirectoryInfo("Images").GetFiles("Background.jpg");
            StartPosition = FormStartPosition.CenterScreen;
            BackgroundImage = new Bitmap(Image.FromFile(backGround.First().FullName));
            var icon = new DirectoryInfo("Images").GetFiles("Swords.ico");
            MaximizeBox = false;
            Icon = new Icon(icon.First().FullName);
            Width = width;
            Height = height;
            Resolution = new Display(Width, Height);
            GameObjects = new List<Object>();
            SpecialStrikes = new List<SpecialStrike>();
            Visible = false;
            new ScreenSelector().ShowDialog();
            FirstPlayer.KnowYourEnemy(SecondPlayer); FirstPlayer.Number = PlayerNumber.FirstPlayer;
            SecondPlayer.KnowYourEnemy(FirstPlayer); SecondPlayer.Number = PlayerNumber.SecondPlayer;
            Fighters = new List<Fighter>() { FirstPlayer, SecondPlayer };
            InitializeMover();
        }

        private void InitializeMover()
        {
            ButtonHandler[FirstPlayer] = new Dictionary<Keys, Action>();
            Cooperator[FirstPlayer] = new HashSet<Keys>();
            foreach (var dict in Determinater[PlayerNumber.FirstPlayer])
            {
                Cooperator[FirstPlayer].Add(dict.Key);
                ButtonHandler[FirstPlayer].Add(dict.Key, dict.Value);
            }

            ButtonHandler[SecondPlayer] = new Dictionary<Keys, Action>();
            Cooperator[SecondPlayer] = new HashSet<Keys>();
            foreach (var dict in Determinater[PlayerNumber.SecondPlayer])
            {
                Cooperator[SecondPlayer].Add(dict.Key);
                ButtonHandler[SecondPlayer].Add(dict.Key, dict.Value);
            }
        }

        public Dictionary<PlayerNumber, Dictionary<Keys, Action>> Determinater = new Dictionary<PlayerNumber, Dictionary<Keys, Action>>()
        {
            {PlayerNumber.FirstPlayer, new Dictionary<Keys, Action>()
            { 
                {Keys.A, () => {FirstPlayer.ChooseYourSide(FighterMotionState.MovingLeft); FirstPlayer.ChangeMotionState(FighterMotionState.MovingLeft);}},
                {Keys.D, () => {FirstPlayer.ChooseYourSide(FighterMotionState.MovingRight); FirstPlayer.ChangeMotionState(FighterMotionState.MovingRight);}},
                {Keys.Space, () => {FirstPlayer.Jump();}},
                {Keys.Z, () => {FirstPlayer.DoAttack();}},
                {Keys.X, () => {FirstPlayer.DoBlock();}},
            }
            },
            {PlayerNumber.SecondPlayer, new Dictionary<Keys, Action>()
            {
                {Keys.Up, () => { SecondPlayer.Jump();}},
                {Keys.Left, () => {SecondPlayer.ChooseYourSide(FighterMotionState.MovingLeft); SecondPlayer.ChangeMotionState(FighterMotionState.MovingLeft);}},
                {Keys.Right, () => {SecondPlayer.ChooseYourSide(FighterMotionState.MovingRight); SecondPlayer.ChangeMotionState(FighterMotionState.MovingRight);}},
                {Keys.K, () => { SecondPlayer.DoAttack();}},
                {Keys.L, () => { SecondPlayer.DoBlock();}}            
            }
            }
        };

        public List<Fighter> Fighters;
        public static Fighter FirstPlayer { get; set; }
        public static Fighter SecondPlayer { get; set; }
        public static Display Resolution { get; set; }
        public static List<Object> GameObjects { get; set; }
        public static List<SpecialStrike> SpecialStrikes { get; set; }

        public Dictionary<Fighter, Dictionary<Keys, Action>> ButtonHandler = new Dictionary<Fighter, Dictionary<Keys, Action>>();
        public Dictionary<Fighter, HashSet<Keys>> Cooperator = new Dictionary<Fighter, HashSet<Keys>>();
        public Tuple<bool, string> Lost;
        public bool Finished;

        public float XIndent { get { return Resolution.X / 80; } }
        public float YIndent { get { return Resolution.Y / 10; } }
  
        public struct Display
        {
            public Display(int width, int height)
            {
                X = width;
                Y = height;
            }

            public int X;
            public int Y;
        }    
    }    
}
