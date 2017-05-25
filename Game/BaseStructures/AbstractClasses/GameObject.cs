using System.Drawing;

namespace Game.BaseStructures.AbstractClasses
{
    public abstract class GameObject
    {
        public float Speed { get; set; }
        public Image Picture { get; set; }
        public PointF Position { get; set; }
        public abstract int Damage { get; }
        public void Move()
        {
            Position = new PointF(Position.X + Speed, Position.Y);
        }

        public abstract bool CheckState();
        public Fighter Opponent { get; set; }
    }
}
