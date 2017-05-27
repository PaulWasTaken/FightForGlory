using System;
using Game.BaseStructures.Enums;
using Game.Fighters;

namespace Game.Commands
{
    public class CommandProcessor
    {
        public void Perfrom(Command command, Fighter fighter)
        {
            switch (command)
            {
                case Command.NormalAttack:
                    fighter.Attack();
                    break;
                case Command.Block:
                    fighter.Block();
                    break;
                case Command.Jump:
                    fighter.Jump();
                    break;
                case Command.MoveLeft:
                    fighter.Turn(MovingSide.Left);
                    break;
                case Command.MoveRight:
                    fighter.Turn(MovingSide.Right);
                    break;
                case Command.Down:
                    break;
                case Command.StrongAttack:
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
