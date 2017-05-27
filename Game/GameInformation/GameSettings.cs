using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Game.BaseStructures.Enums;
using Game.Commands;
using Game.Controllers;
using Game.Fighters;
using Game.GameObjects;

namespace Game.GameInformation
{
    public class GameSettings
    {
        public GameSettings(int width, int height)
        {
            Resolution = new Point(width, height);
            MakeReversed();
            //InitializeMover();
        }

        private void MakeReversed()
        {
            foreach (var player in Determinater.Keys)
            {
                ReversedDeterminater.Add(player, new Dictionary<Command, Keys>());
                foreach (var pair in Determinater[player])
                    ReversedDeterminater[player].Add(pair.Value, pair.Key);
            }
        }

        public Keys GetButtonCommand(PlayerNumber player, Command command)
        {
            return ReversedDeterminater[player][command];
        }

        private void InitializeMover()
        {
            // In case of changing управление.
        }

        public Dictionary<PlayerNumber, Dictionary<Command, Keys>> ReversedDeterminater =
            new Dictionary<PlayerNumber, Dictionary<Command, Keys>>();

        public Dictionary<PlayerNumber, Dictionary<Keys, Command>> Determinater =
            new Dictionary<PlayerNumber, Dictionary<Keys, Command>>
            {
            {PlayerNumber.FirstPlayer, new Dictionary<Keys, Command>
                {
                {Keys.A, Command.MoveLeft},
                {Keys.D, Command.MoveRight},
                {Keys.Space, Command.Jump},
                {Keys.S, Command.Down },
                {Keys.Z, Command.NormalAttack},
                {Keys.X, Command.StrongAttack},
                {Keys.ShiftKey|Keys.Shift, Command.Block}
            }
            },
            {PlayerNumber.SecondPlayer, new Dictionary<Keys, Command>()
            {
                {Keys.Left, Command.MoveLeft},
                {Keys.Right, Command.MoveRight},
                {Keys.Up, Command.Jump},
                {Keys.Down, Command.Down },
                {Keys.J, Command.NormalAttack},
                {Keys.K, Command.StrongAttack},
                {Keys.L, Command.Block}
            }
            }
        };

        public ComboController GetComboController(PlayerNumber player)
        {
            return dictWithComboControllers[player];
        }

        public ImageController GetImageController(PlayerNumber player)
        {
            return dictWithImageControllers[player];
        }

        public ImageController GetObjImageController(string name)
        {
            return objectImageControllers[name];
        }

        public void AddControllersForPlayer(Fighter player)
        {
            dictWithComboControllers[player.Number] = player.GetComboController();
            dictWithImageControllers[player.Number] = new ImageController(player);
        }

        private readonly Dictionary<string, ImageController> objectImageControllers = new Dictionary<string, ImageController>();
        private readonly Dictionary<PlayerNumber, ComboController> dictWithComboControllers = new Dictionary<PlayerNumber, ComboController>();
        private readonly Dictionary<PlayerNumber, ImageController> dictWithImageControllers = new Dictionary<PlayerNumber, ImageController>();

        public static Point Resolution { get; set; }
        public float XIndent => Resolution.X / 80f;
        public float YIndent => Resolution.Y / 10f;

        public void AddControllersForObjects()
        {
            objectImageControllers.Add(typeof(Wisp).Name, new ImageController());
            objectImageControllers.Add(typeof(Spear).Name, new ImageController());
            objectImageControllers.Add(typeof(Lightning).Name, new ImageController());
        }
    }
}
