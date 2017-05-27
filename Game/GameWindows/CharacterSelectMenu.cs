using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Game.Fighters;

namespace Game.GameWindows
{
    public sealed class CharacterSelectMenu : UserControl
    {
        private bool isFirstPlayer = true;
        private bool isSecondPlayer = true;
        private readonly GameWindow parent;

        private Type FirstPlayer;
        private Type SecondPlayer;

        public CharacterSelectMenu(GameWindow parent)
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
            var baseType = typeof(Fighter);
            var types = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => type != baseType && baseType.IsAssignableFrom(type));
            foreach (var type in types)
            {
                var button = new Button
                {
                    Text = type.Name,
                    AutoSize = true,
                };
                button.Click += (sender, eventArgs) => { OnClick(sender, eventArgs, type); };
                layout.Controls.Add(button);
            }
            return layout;
        }

        private void OnClick(object sender, EventArgs e, Type type)
        {
            if (isFirstPlayer)
            {
                isFirstPlayer = false;
                FirstPlayer = type;
                return;
            }
            if (!isSecondPlayer) return;
            isSecondPlayer = false;
            SecondPlayer = type;

            Visible = false;
            Enabled = false;
            parent.StartGame(new []{FirstPlayer, SecondPlayer});
        }
    }
}
