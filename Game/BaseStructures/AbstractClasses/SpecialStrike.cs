using Game.BaseStructures.Enums;

namespace Game.BaseStructures.AbstractClasses
{
    public abstract class SpecialStrike
    {
        public Fighter Source { get; set; }
        public float Damage { get; set; }
        public float Range { get; set; }
        public abstract bool IfReached();
        public void FixPicture()
        {
            //Source.CurrentImage = Source.PreviousImage;
            Source.State = FighterMotionState.NotMoving;
        }
    }
}
