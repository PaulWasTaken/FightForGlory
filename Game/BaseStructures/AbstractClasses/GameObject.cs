using System.Drawing;
using Game.BaseStructures.Enums;

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

        public abstract bool CheckState(Fighter opponent);
        public PlayerNumber Source { get; set; }
        protected bool HasReached(Fighter enemy)
        {
            return enemy.Body.Contains(Position.X, Position.Y);
        }
    }
}
