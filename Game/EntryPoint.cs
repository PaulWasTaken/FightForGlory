using System;
using System.Windows.Forms;
using Game.GameWindows;
using MainMenu = Game.GameWindows.MainMenu;

namespace Game
{
    static class EntryPoint
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameWindow());
        }
    }
}
