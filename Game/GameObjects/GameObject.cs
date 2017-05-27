using System.Drawing;
using Game.BaseStructures.Enums;
using Game.Fighters;
using Game.GameInformation;

namespace Game.GameObjects
{
    public abstract class GameObject
    {
        protected float Speed { get; set; }
        public RectangleF Size { get; set; }
        public abstract int Damage { get; }
        public abstract bool CheckState(Fighter opponent);
        public PlayerNumber Source { get; set; }
        protected bool HasReached(Fighter enemy)
        {
            return enemy.Body.Contains(Size.X, Size.Y + Size.Height / 2);
        }

        protected bool IsOutsideScreen()
        {
            return !(Size.X >= 0 && Size.X <= GameSettings.Resolution.X);
        }
        public void Move()
        {
            Size = Size.Move(Speed, 0);
        }
    }
}
