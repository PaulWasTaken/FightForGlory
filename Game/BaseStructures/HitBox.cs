using Game.GameWindows;

namespace Game.BaseStructures
{
    public class HitBox
    {
        public float TopLeftX { get; set; }
        public float TopLeftY { get; set; }
        public float BotRightX { get; set; }
        public float BotRightY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public HitBox(float x, float y)
        {
            TopLeftX = x + 20;
            TopLeftY = y;
            BotRightX = TopLeftX + Settings.Resolution.X / 16;
            BotRightY = y + Settings.Resolution.Y / 4.5f;
            Width = (int)(BotRightX - TopLeftX);
            Height = (int)(BotRightY - TopLeftY);
        }
    }
}
