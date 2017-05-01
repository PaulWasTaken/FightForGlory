using System.Drawing;

namespace Game.BaseStructures.AbstractClasses
{
    public abstract class Object
    {
        public float Speed { get; set; }
        public Image Picture { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public abstract int Damage { get; }
        public void Move()
        {
            X += Speed;
        }

        public abstract bool CheckState();
        public Fighter Opponent { get; set; }
    }
}
