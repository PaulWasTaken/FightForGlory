using Game.BaseStructures.Enums;

namespace Game.BaseStructures.AbstractClasses
{
    public interface ISpecialStrike
    {
        Fighter Source { get; set; }
        float Damage { get; set; }
        float Range { get; set; }
        bool IfReached(Fighter opponent);
    }
}
