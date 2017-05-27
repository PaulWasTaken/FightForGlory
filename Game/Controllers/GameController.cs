﻿using System.Windows.Forms;
using Game.BaseStructures.Enums;
using Game.Commands;
using Game.GameInformation;

namespace Game.Controllers
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
                var comboController = settings.GetComboController(fighter.Number);
                if (gameState.SpecialStrikes.Count != 0)
                    break;
                if (!settings.Determinater[fighter.Number].ContainsKey(e.KeyData)) continue;
                var command = settings.Determinater[fighter.Number][e.KeyData];
                comboController.UpdateState(command);
                if (comboController.ComboCompleted)
                {
                    var res = comboController.PerformCombo();
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
                gameState.FirstPlayer.StandStill();

            if (settings.GetButtonCommand(PlayerNumber.SecondPlayer, Command.MoveLeft) == e.KeyData &&
                gameState.SecondPlayer.State == FighterMotionState.MovingLeft ||
                settings.GetButtonCommand(PlayerNumber.SecondPlayer, Command.MoveRight) == e.KeyData &&
                gameState.SecondPlayer.State == FighterMotionState.MovingRight)
                gameState.SecondPlayer.StandStill();
        }
    }
}
