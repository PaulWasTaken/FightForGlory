using System;
using System.Windows.Forms;
using Game.GameWindows;
using Ninject;

namespace Game
{
    public static class EntryPoint
    {
        [STAThread]
        public static void Main()
        {
            var container = BuildContainer();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var window = container.Get<GameWindow>();
            Application.Run(window);
        }

        private static StandardKernel BuildContainer()
        {
            var container = new StandardKernel();
            return container;
        }
    }
}
