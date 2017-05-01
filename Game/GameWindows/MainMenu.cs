using System;
using System.Windows.Forms;

namespace Game.GameWindows
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Visible = false;
            new GameWindow(SystemInformation.VirtualScreen.Width,
                           SystemInformation.VirtualScreen.Height).ShowDialog();
            Close();
        }
    }
}
