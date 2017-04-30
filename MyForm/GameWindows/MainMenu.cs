using System;
using System.Windows.Forms;

namespace Game
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
            this.Visible = false;
            //new ScreenSelector().ShowDialog();
            new GameWindow(Size.Width, Size.Height).ShowDialog();
            Close();
        }
    }
}
