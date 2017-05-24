using Game.BaseStructures.AbstractClasses;

namespace Game.Commands
{
    interface ICommandProcessor
    {
        void Perfrom(Command command, Fighter fighter);
    }
}
