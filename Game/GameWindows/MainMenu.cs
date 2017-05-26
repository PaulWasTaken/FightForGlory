using System;
using System.Drawing;
using System.Windows.Forms;

namespace Game.GameWindows
{
    public sealed class MainMenu : UserControl
    {
        private readonly GameWindow parent;

        public MainMenu(GameWindow parent)
        {
            this.parent = parent;
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackColor = Color.Transparent;
            var layout = CreateLayout();
            Controls.Add(layout);
        }

        private FlowLayoutPanel CreateLayout()
        {
            var layout = new FlowLayoutPanel { Dock = DockStyle.Fill };
            var start = new Button
            {
                Text = @"Начать игру"
            };
            start.Click += OnStartClick;
            var exit = new Button
            {
                Text = @"Выход"
            };
            exit.Click += OnExitClick;
            layout.Controls.Add(start);
            layout.Controls.Add(exit);
            return layout;
        }

        private void OnExitClick(object sender, EventArgs e)
        {
            parent.Close();
        }

        private void OnStartClick(object sender, EventArgs e)
        {
            Visible = false;
            Enabled = false;
            parent.DrawCharactedSelectMenu();
        }
    }
}
