using System;
using System.Drawing;
using System.Windows.Forms;
using Game.BaseStructures.Enums;
using Game.Fighters;

namespace Game.Factories
{
    public class FighterFactory
    {
        private readonly PointF firstPlayerLocation;
        private readonly PointF secondPlayerLocation;

        public FighterFactory()
        {
            var width = SystemInformation.VirtualScreen.Width;
            var height = SystemInformation.VirtualScreen.Height;
            firstPlayerLocation = new PointF(width / 2 - width / 4, height / 1.5f);
            secondPlayerLocation = new PointF(width / 2 + width / 4, height / 1.5f);
        }

        public Fighter CreateFighter(Type type, PlayerNumber number)
        {
            var point = number == PlayerNumber.FirstPlayer ? firstPlayerLocation : secondPlayerLocation;
            return CreateFighter(type, point);
        }

        private static Fighter CreateFighter(Type type, PointF location)
        {
            var constuctor = type.GetConstructor(new[] {typeof(string), typeof(PointF)});
            // ReSharper disable once PossibleNullReferenceException
            var fighter = (Fighter) constuctor.Invoke(new object[] {type.Name, location});
            return fighter;
        }
    }
}
