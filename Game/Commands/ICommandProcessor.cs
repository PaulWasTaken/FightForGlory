using Game.BaseStructures.AbstractClasses;

namespace Game.Commands
{
    public interface ICommandProcessor
    {
        void Perfrom(Command command, Fighter fighter);
    }
}
