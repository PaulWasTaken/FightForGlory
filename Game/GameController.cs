using System.Windows.Forms;
using Game.BaseStructures.Enums;
using Game.Commands;
using Game.GameInformation;

namespace Game
{
    public class GameController
    {
        private readonly GameSettings settings;
        private readonly GameState gameState;
        private readonly CommandProcessor commandProcessor;

        public GameController(GameSettings settings, GameState gameState)
        {
            this.settings = settings;
            this.gameState = gameState;
            commandProcessor = new CommandProcessor();
        }

        public void KeyDown(KeyEventArgs e)
        {
            foreach (var fighter in gameState.Fighters)
            {
                var comboPerformer = settings.DictWithComboControllers[fighter.Number];
                if (gameState.SpecialStrikes.Count != 0)
                    break;
                if (!settings.Determinater[fighter.Number].ContainsKey(e.KeyData)) continue;
                var command = settings.Determinater[fighter.Number][e.KeyData];
                if (comboPerformer.CheckForCombo(command))
                {
                    var res = comboPerformer.PerformCombo();
                    if (res != null)
                        gameState.GameObjects.Add(res);
                }
                commandProcessor.Perfrom(command, fighter);
                return;
            }
        }

        public void KeyUp(KeyEventArgs e)
        {
            if (settings.GetButtonCommand(PlayerNumber.FirstPlayer, Command.MoveLeft) == e.KeyData && 
                gameState.FirstPlayer.State == FighterMotionState.MovingLeft ||
                settings.GetButtonCommand(PlayerNumber.FirstPlayer, Command.MoveRight) == e.KeyData && 
                gameState.FirstPlayer.State == FighterMotionState.MovingRight)
                gameState.FirstPlayer.State = FighterMotionState.NotMoving;

            if (settings.GetButtonCommand(PlayerNumber.SecondPlayer, Command.MoveLeft) == e.KeyData &&
                gameState.SecondPlayer.State == FighterMotionState.MovingLeft ||
                settings.GetButtonCommand(PlayerNumber.SecondPlayer, Command.MoveRight) == e.KeyData &&
                gameState.SecondPlayer.State == FighterMotionState.MovingRight)
                gameState.SecondPlayer.State = FighterMotionState.NotMoving;
        }
    }
}
