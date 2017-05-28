using System.Windows.Forms;
using Game.GameInformation;

namespace Game.Factories
{
    public class GameSettingsFactory
    {
        public GameSettings Create()
        {
            var width = SystemInformation.VirtualScreen.Width;
            var height = SystemInformation.VirtualScreen.Height;
            return new GameSettings(width, height);
        }
    }
}
