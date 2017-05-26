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
                case Command.NormalAttack:
                    fighter.DoAttack();
                    break;
                case Command.Block:
                    fighter.DoBlock();
                    break;
                case Command.Jump:
                    fighter.Jump();
                    break;
                case Command.MoveLeft:
                    fighter.LookRight = false;
                    fighter.State = FighterMotionState.MovingLeft;
                    break;
                case Command.MoveRight:
                    fighter.LookRight = true;
                    fighter.State = FighterMotionState.MovingRight;
                    break;
                case Command.Down:
                    break;
                case Command.StrongAttack:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
