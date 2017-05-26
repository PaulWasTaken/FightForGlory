using System.Collections.Generic;
using System.Windows.Forms;
using Game.BaseStructures;
using Game.BaseStructures.Enums;
using Game.Commands;
using Game.Controllers;

namespace Game.GameInformation
{
    public class GameSettings
    {
        public GameSettings(int width, int height)
        {
            Resolution = new Display(width, height);
            MakeReversed();
            //InitializeMover();
        }

        private void MakeReversed()
        {
            foreach (var player in Determinater.Keys)
            {
                ReversedDeterminater.Add(player, new Dictionary<Command, Keys>());
                foreach (var pair in Determinater[player])
                {
                    ReversedDeterminater[player].Add(pair.Value, pair.Key);
                }  
            }
            
        }

        public Keys GetButtonCommand(PlayerNumber player, Command command)
        {
            return ReversedDeterminater[player][command];
        }

        private void InitializeMover()
        {
            // In case of changing управление. Лол
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

        public Dictionary<PlayerNumber, ComboController> DictWithComboControllers = new Dictionary<PlayerNumber, ComboController>();
        public Dictionary<PlayerNumber, ImageController> DictWithImageChangers = new Dictionary<PlayerNumber, ImageController>();

        public static Display Resolution { get; set; }
        public float XIndent => Resolution.X / 80f;
        public float YIndent => Resolution.Y / 10f;
    }
}
