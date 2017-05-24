using System;
using Game.BaseStructures.AbstractClasses;
using Game.BaseStructures.Enums;

namespace Game.Commands
{
    public class CommandProcessor : ICommandProcessor
    {
        public void Perfrom(Command command, Fighter fighter)
        {
            switch (command)
            {
                case Command.Attack:
                    fighter.DoAttack();
                    break;
                case Command.Block:
                    fighter.DoBlock();
                    break;
                case Command.Jump:
                    fighter.Jump();
                    break;
                case Command.MoveLeft:
                    fighter.ChooseYourSide(FighterMotionState.MovingLeft);
                    fighter.ChangeMotionState(FighterMotionState.MovingLeft);
                    break;
                case Command.MoveRight:
                    fighter.ChooseYourSide(FighterMotionState.MovingRight);
                    fighter.ChangeMotionState(FighterMotionState.MovingRight);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
